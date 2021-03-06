﻿using OptionProcessingService.Types;
using CommonObjects.Logger;
using CommonObjects.Unitity;
using OptionProcessingService.Requests;
using OptionProcessingService.Responses;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperWebSocket;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OptionProcessingService
{
    public class Server
    {
        private class SusbcibedItem
        {
            public Symbol Symbol;
            public string Currency;
        }

        private class UserSession
        {
            public string UserName;
            public WebSocketSession Session;
            //public List<SusbcibedItem> Subscribers;
        }


        private readonly DateTime UNIX_TIME = new DateTime(1970, 1, 1);

        private WebSocketServer ws;
        private Ticker ticker;
        //private List<WebSocketSession> _connectedUsers = new List<WebSocketSession>();
        private List<UserSession> _connectedUsers = new List<UserSession>();

        public Server(string ip, int port)
        {
            ws = new WebSocketServer();
            ws.Setup(
             new RootConfig(),
                new ServerConfig
                {
                    Name = "OptionProcessingService",
                    Ip = ip,
                    Port = port,
                    Mode = SocketMode.Tcp,
                    LogAllSocketException = true,
                    LogCommand = true,
                    LogBasicSessionActivity = true
                }, new SuperSocket.SocketEngine.SocketServerFactory()
            );

            ws.NewSessionConnected += new SessionHandler<WebSocketSession>(OnSessionConnected);
            ws.NewMessageReceived += new SessionHandler<WebSocketSession, string>(OnMessageReceived);
            ws.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>(OnSessionClosed);
            DataProvider.OnExecution += DataProvider_OnExecution;
            ticker = new Ticker();
        }

        private void DataProvider_OnExecution(Order order, OrderExecutionResponse arg2)
        {
            var client = ClientsCollection.GetByID(order.UserID);
            if (client == null) return;
            client.AccountInfo = DataProvider.GetAccountInfo(client.Username);
            arg2.Order = order;
            var res = JsonSerializeHelper.Serialize(arg2);
            client.SocketSession.Send(res);
            Logger.Log(client.Username, "OredrExecutionResponse", string.Empty, res);
            sendSubcriptionNotification(client.SocketSession, client, null);
        }

        public void Start()
        {
            if (ws == null)
                throw new InvalidOperationException("Ws is null");
            if (!ws.Start())
                throw new Exception("Error. Socket doesn't opened");
            ticker.Start();
        }

        public void Stop()
        {
            if (ws == null) return;
            ticker.Stop();
            ws.Stop();
        }

        private void OnSessionConnected(WebSocketSession session)
        {
            ClientsCollection.Add(new Client(session));
            Console.WriteLine("Connected {0}", session.SessionID);
        }

        private void OnSessionClosed(WebSocketSession session, CloseReason r)
        {
            ClientsCollection.Remove(session);
            Console.WriteLine("Closed {0}", r.ToString());
        }

        private void OnMessageReceived(WebSocketSession session, string m)
        {
            try
            {
                var client = ClientsCollection.Get(session);
                if (client == null) return;

                var r = JsonSerializeHelper.Deserialize<Message>(m);
                switch (r.MsgType)
                {
                    case "RegistrationRequest":
                        {
                            if (client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<RegistrationRequest>(m);
                            var responseString = "";

                            if (string.IsNullOrEmpty(request.Login) || string.IsNullOrEmpty(request.Password))
                            {
                                responseString = JsonSerializeHelper.Serialize(new ErrorResponse("Bad request"));
                            }
                            else if (DataProvider.IsUsernameExists(request.Login))
                            {
                                responseString = JsonSerializeHelper.Serialize(new RegistrationResponse
                                {
                                    ReqID = request.ReqID,
                                    Error = "Someone with this username already exists"
                                });
                            }
                            else
                            {
                                responseString = DataProvider.AddAccount(request.Login, request.Password, request.Email, request.Card)
                                   ? JsonSerializeHelper.Serialize(new RegistrationResponse { ReqID = request.ReqID })
                                   : JsonSerializeHelper.Serialize(new RegistrationResponse { ReqID = request.ReqID, Error = "Undefined error" });
                            }

                            session.Send(responseString);
                            break;
                        }

                    case "LoginRequest":
                        {
                            if (client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("User Already logged in to server")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<LoginRequest>(m);
                            var responseString = "";

                            if (DataProvider.ValidateUserCredentials(request.Login, request.Password))
                            {
                                client.Username = request.Login;
                                client.Password = request.Password;
                                client.IsLoggedIn = true;
                                client.AccountInfo = DataProvider.GetAccountInfo(client.Username);
                                responseString = JsonSerializeHelper.Serialize(new LoginResponse
                                {
                                    ReqID = request.ReqID
                                });

                                _connectedUsers.Add(new UserSession()
                                {
                                    UserName = client.Username
                                    ,
                                    Session = session
                                });

                            }
                            else
                            {
                                responseString = JsonSerializeHelper.Serialize(new LoginResponse
                                {
                                    ReqID = request.ReqID,
                                    Error = "Invalid credentials"
                                });
                            }

                            session.Send(responseString);
                            break;
                        }

                    case "LogoutRequest":
                        {
                            var request = JsonSerializeHelper.Deserialize<LogoutRequest>(m);
                            var responseString = client.IsLoggedIn
                                 ? JsonSerializeHelper.Serialize(new LogoutResponse { ReqID = request.ReqID })
                                 : JsonSerializeHelper.Serialize(new ErrorResponse("No user session availble with server"));
                            session.Send(responseString);
                            session.Close();
                            break;
                        }

                    case "QuotesSubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<QuotesSubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.AddQuote(request.Symbol.Name);
                            break;
                        }

                    case "QuotesUnsubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<QuotesUnsubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.RemoveQuote(request.Symbol.Name);
                            break;
                        }

                    case "AccountInfoGetRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<AccountInfoGetRequest>(m);
                            var response = new AccountInfoGetResponse
                            {
                                ReqID = request.ReqID,
                                AccountData = client.AccountInfo,
                            };
                            session.Send(JsonSerializeHelper.Serialize(response));
                            break;
                        }

                    case "AccountInfoSubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<AccountInfoSubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.AccountInfo = true;
                            break;
                        }

                    case "AccountInfoUnsubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<AccountInfoUnsubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.AccountInfo = false;
                            break;
                        }

                    case "AssetsGetRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<AssetsGetRequest>(m);
                            var response =
                            new AssetsGetResponse
                            {
                                ReqID = request.ReqID,
                                Assets = DataProvider.GetAssets()
                            };
                            session.Send(JsonSerializeHelper.Serialize(response));
                            break;
                        }
                    case "AssetsGetBySymbolRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<AssetsGetBySymbolRequest>(m);
                            var timestamp = (TimestampUtility.GetCurrentTimestamp() / 1000) + DataProvider.StopExpiryLimitinMinutes * 60;
                            var asset = DataProvider.GetAssets(request.Symbol);
                            Asset temp = new Asset();
                            if (asset != null)
                            {
                                temp.ID = asset.ID;
                                temp.Symbol = asset.Symbol;
                                temp.ExpiryTimeList = asset.ExpiryTimeList.Where(x => (x.ExpiryTimestamps / 1000) > timestamp)
                                    .Take(4)
                                    .ToList<ExpiryTime>();
                            }
                            var response =
                            new AssetsGetBySymbolResponse
                            {
                                ReqID = request.ReqID,
                                Assets = temp
                            };
                            session.Send(JsonSerializeHelper.Serialize(response));
                            break;
                        }

                    case "BarsSubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<BarsSubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.AddBarSubscription(request.ID, request.Symbol.Name, request.Periodicity, request.Interval);
                            break;
                        }
                    case "BarsUnsubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<BarsUnsubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.RemoveBarSubscription(request.ID);
                            break;
                        }

                    case "HistoryGetRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<HistoryGetRequest>(m);
                            var bars = request.BarsCount == 0
                                ? DataProvider.GetHistory(request.Symbol, request.Periodicity, request.Interval, request.StartTimestamp)
                                : DataProvider.GetHistory(request.Symbol, request.Periodicity, request.Interval, request.BarsCount);
                            var response = new HistoryGetResponse(request.ReqID, bars);
                            session.Send(JsonSerializeHelper.Serialize(response));
                            break;
                        }

                    case "OrderPlaceRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }
                            var request = JsonSerializeHelper.Deserialize<OrderPlaceRequest>(m);
                            string res = string.Empty;
                            var asset = DataProvider.GetAssetById(request.OptionID);
                            var valResult = DataProvider.ValidateOrder(client.AccountInfo, asset, request);
                            if (!String.IsNullOrEmpty(valResult))
                            {

                                var response = new OrderPlaceResponse
                                {
                                    ReqID = request.ReqID,
                                    ClOrdID = request.ClOrdID,
                                    Error = valResult
                                };
                                res = JsonSerializeHelper.Serialize(response);
                                session.Send(res);
                            }
                            else
                            {
                                if (request.OptionType == Enums.OptionType.Express)
                                {
                                    request.ExpiryTimestamp = DataProvider.GetCurrentTimestamp() + (request.ExpressExpiryInSeconds * 1000);
                                }
                                DateTime expiry = TimestampUtility.TimestampToDateTime(request.ExpiryTimestamp);
                                var order = DataProvider.PlaceOrder(client.AccountInfo, asset, request);
                                var response = new OrderPlaceResponse
                                {
                                    ReqID = request.ReqID,
                                    ClOrdID = request.ClOrdID,
                                    SrvOrdID = order.ID,
                                    Expiry = expiry
                                };
                                res = JsonSerializeHelper.Serialize(response);
                                session.Send(res);
                                sendSubcriptionNotification(session, client, order);
                            }
                            Logger.Log(client.Username, "OrderPlaceRequest", m, res, !String.IsNullOrEmpty(valResult));
                            break;
                        }

                    case "OrderCancelRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<OrderCancelRequest>(m);
                            var order = DataProvider.CancelOrder(client.AccountInfo, request.SrvOrdID);

                            if (order == null)
                            {
                                var response = new OrderCancelResponse
                                {
                                    ReqID = request.ReqID,
                                    SrvOrdID = request.SrvOrdID,
                                    Error = "Error"
                                };
                                session.Send(JsonSerializeHelper.Serialize(response));
                            }
                            else
                            {
                                var response = new OrderCancelResponse
                                {
                                    ReqID = request.ReqID,
                                    SrvOrdID = request.SrvOrdID
                                };
                                session.Send(JsonSerializeHelper.Serialize(response));

                                //if (ClientsCollection.Get(session).Subscriptions.OpenOrders)
                                //{
                                //    session.Send(JsonSerializeHelper.Serialize(new OpenOrdersSubscribeResponse
                                //    {
                                //        Orders = new List<Order> { order }
                                //    }));
                                //}

                                //if (ClientsCollection.Get(session).Subscriptions.OrderHistory)
                                //{
                                //    session.Send(JsonSerializeHelper.Serialize(new OrderHistorySubscribeResponse
                                //    {
                                //        Orders = new List<Order>{ order }
                                //    }));
                                //}

                                //if (ClientsCollection.Get(session).Subscriptions.AccountInfo)
                                //{
                                //    session.Send(JsonSerializeHelper.Serialize(new AccountInfoSubscribeResponse
                                //    {
                                //        AccountData = client.AccountInfo
                                //    }));
                                //}
                                sendSubcriptionNotification(session, client, order);
                            }
                            break;
                        }
                    case "OpenOrdersGetRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<OpenOrdersGetRequest>(m);
                            var response = new OpenOrdersGetResponse
                            {
                                ReqID = request.ReqID,
                                Orders = DataProvider.GetOpenOrders(client.AccountInfo)
                            };
                            session.Send(JsonSerializeHelper.Serialize(response));
                            break;
                        }
                    case "OpenOrdersSubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<OpenOrdersSubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.OpenOrders = true;
                            break;
                        }
                    case "OpenOrdersUnsubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<OpenOrdersUnsubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.OpenOrders = false;
                            break;
                        }

                    case "OrderHistoryGetRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<OrderHistoryGetRequest>(m);
                            var response =
                                new OrderHistoryGetResponse
                                {
                                    ReqID = request.ReqID,
                                    Orders = DataProvider.GetOrderHistory(client.AccountInfo)
                                };
                            session.Send(JsonSerializeHelper.Serialize(response));
                            break;
                        }

                    case "OrderHistorySubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<OrderHistorySubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.OrderHistory = true;
                            break;
                        }

                    case "OrderHistoryUnsubscribeRequest":
                        {
                            if (!client.IsLoggedIn)
                            {
                                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                                return;
                            }

                            var request = JsonSerializeHelper.Deserialize<OrderHistoryUnsubscribeRequest>(m);
                            ClientsCollection.Get(session).Subscriptions.OrderHistory = false;
                            break;
                        }

                    default:
                        {
                            session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request")));
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                session.Send(JsonSerializeHelper.Serialize(new ErrorResponse("Bad request : " + e.Message)));
            }
        }
        private void sendSubcriptionNotification(WebSocketSession session, Client client, Order order = null)
        {

            if (ClientsCollection.Get(session).Subscriptions.OpenOrders && order != null)
            {
                IList<Order> updateOrders = new List<Order> { order };
                session.Send(JsonSerializeHelper.Serialize(new OpenOrdersSubscribeResponse
                {
                    Orders = updateOrders
                }));
            }
            if (ClientsCollection.Get(session).Subscriptions.OrderHistory && order != null)
            {
                session.Send(JsonSerializeHelper.Serialize(new OrderHistorySubscribeResponse
                {
                    Orders = new List<Order> { order }
                }));
            }

            if (ClientsCollection.Get(session).Subscriptions.AccountInfo)
            {
                session.Send(JsonSerializeHelper.Serialize(new AccountInfoSubscribeResponse
                {
                    AccountData = client.AccountInfo
                }));
            }

        }
    }
}
