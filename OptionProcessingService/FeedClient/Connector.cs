#region License

// /* WARNING! This program and source code is owned and licensed by 
//    Modulus Financial Engineering, Inc. http://www.modulusfe.com
//    Viewing or use this code requires your acceptance of the license
//    agreement found at http://www.modulusfe.com/support/license.pdf
//    Removal of this comment is a violation of the license agreement.
//    Copyright 20013-2014 by Modulus Financial Engineering, Inc. */

#endregion

using CommonObjects.Logger;
using OptionProcessingService.ServerMess;
using OptionProcessingService.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
//using M4.Common.Classes;
//using M4.DataFeed.DataServer;
//using M4.Interfaces.Indicators;
//using NLog;

namespace OptionProcessingService.FeedClient
{
    public class Connector : IFeedClient
    {
        //IFeedClient members
        private DataFeed[] _DataFeeds;
        private Dictionary<string, Quote> lastQuotes = new Dictionary<string, Quote>();
        public event Action<IList<Quote>> OnNewQuoteResponse;
        private string feedName;
        public DataFeed[] DataFeed
        {
            get { return _DataFeeds; }
            private set { _DataFeeds = value; }
        }
        public Dictionary<string, Quote> LastQuotesList
        {
            get { return lastQuotes; }
            private set { lastQuotes = value; }
        }
        public void SubscribeSymbolList(List<SymbolItem> symbolList)
        {
            foreach (var symbol in symbolList)
            {
                _client.MessageIn(new SubscribeRequest() { Symbol = symbol });

            }
        }
        public void UnSubscribeSymbolList(List<SymbolItem> symbolList)
        {
            foreach (var symbol in symbolList)
            {
                _client.MessageIn(new UnsubscribeRequest() { Symbol = symbol });
            }
        }
        public void NewTick(ddfplus.Quote tick)
        {
            throw new NotImplementedException();
        }
        public Connector()
        {
            this._lastHost = System.Configuration.ConfigurationSettings.AppSettings["dataserver_IP"];
            this._lastPort = Int32.Parse(System.Configuration.ConfigurationSettings.AppSettings["dataserver_port"]);
            this._lastUser = System.Configuration.ConfigurationSettings.AppSettings["dataserver_username"];
            this._lastPassword = System.Configuration.ConfigurationSettings.AppSettings["dataserver_password"];
            this.feedName = System.Configuration.ConfigurationSettings.AppSettings["FeedName"];
            Login(_lastUser, _lastPassword, _lastHost, _lastPort);
        }
        //
        private const string DisconnectByServer = "close";

        private const int HeartbeatInterval = 15;
        private readonly Binding _binding = new NetTcpBinding("NetTcpBinding_IWCFService");

        //private static Logger _logger = Logger;
        internal ConnectionStatus LastConnectionStatus = ConnectionStatus.Disconnected;

        private CallbackObject _callbackObject;
        private WCFServiceClient _client;
        private InstanceContext _context;

        private EndpointAddress _endpoint;

        private Thread _heartbeatThread;
        private bool _isAlive;
        private string _lastHost;
        private string _lastPassword;
        private int _lastPort;
        private string _lastUser;
        internal event EventHandler<EventArgs<Tick[]>> Ticks;
        internal event EventHandler<EventArgs<ServerMess.DataFeed[]>> DataFeedsList;
        internal event EventHandler<EventArgs<HistoryResponse>> HistoryResponse;
        internal event EventHandler<EventArgs<string>> ErrorInfoResponse;

        internal event Action<ConnectionStatus, string> ConnectionStatusChanged;

        private void Initialize()
        {
            Logger.Info("Connector Initialize");
            _callbackObject = new CallbackObject(this);
            _context = new InstanceContext(_callbackObject);
            _client = new WCFServiceClient(_context, _binding, _endpoint);
        }

        private void DeInitialize()
        {
            Logger.Info("Connector Deinitialize");
            StopHeartBeat();

            try
            {
                if (_client != null)
                {
                    if (_client.State != CommunicationState.Faulted)
                        _client.Close();
                    else
                        _client.Abort();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("DeInitialize", ex);
            }
            _client = null;

            try
            {
                if (_context != null)
                {
                    if (_context.State != CommunicationState.Faulted)
                    {
                        try
                        {
                            _context.Close(TimeSpan.FromSeconds(5));
                        }
                        catch (TimeoutException ex)
                        {
                            Logger.Error("DeInitialize connector", ex);
                        }
                    }
                    else
                        _context.Abort();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("DeInitialize", ex);
            }
            _context = null;

            if (_callbackObject != null)
                _callbackObject.Dispose();

            _callbackObject = null;
        }

        private void HeartbeatThreadBody()
        {
            do
            {
                Task.Factory.StartNew(() => Send(new HeartbeatRequest()));

                for (var i = 0; i < HeartbeatInterval; i++)
                {
                    Thread.Sleep(1000);

                    if (!_isAlive)
                        return;
                }
            } while (true);
        }

        internal void Send(RequestMessage message)
        {
            if (LastConnectionStatus != ConnectionStatus.Connected)
                return;

            try
            {
                _client.MessageIn(message);
            }
            catch (Exception ex)
            {
                Logger.Error("Client request", ex);

                if (_client != null && _client.State != CommunicationState.Opened)
                    UpdateConnectionStatus(ConnectionStatus.ConnectionLost, "Connection lost");
            }
        }

        #region Login/Logout

        /// <summary>
        ///     Login to server
        /// </summary>
        /// <returns>Return empty string in case of success of error message</returns>
        public string Login(string userName, string password, string host, int port)
        {
            Logger.Info("Login");
            _endpoint = new EndpointAddress(string.Format("net.tcp://{0}:{1}/DataServer_Service", host, port));
            _lastUser = userName;
            _lastPassword = password;
            _lastHost = host;
            _lastPort = port;

            DeInitialize();
            Initialize();

            try
            {
                var loginRequest = new LoginRequest {Login = userName, Password = password};

                _client.Login(loginRequest);
            }
            catch (FaultException<DataServerException> ex)
            {
                Logger.Error("Login attempt failed. Reason: " + ex.Detail.Reason, ex);
                return ex.Detail.Reason;
            }
            catch (EndpointNotFoundException ex)
            {
                Logger.Error("", ex);
                return ex.Message;
            }
            catch (CommunicationException ex)
            {
                Logger.Error("", ex);
                return ex.Message;
            }
            catch (Exception ex)
            {
                Logger.Error("Login attempt failed.", ex);
                return "Internal error";
            }

            Logger.Info("User logged in : "+ userName);
            Logger.Info("Get DataFeed from server");
            _client.MessageIn(new DataFeedListRequest());
            _isAlive = true;

            _heartbeatThread = new Thread(HeartbeatThreadBody) {Name = "WCF Heartbeat", IsBackground = true};

            Logger.Info("Start heartbeat thread");
            _heartbeatThread.Start();

            UpdateConnectionStatus(ConnectionStatus.Connected, null);

            return string.Empty;
        }

        /// <summary>
        ///     Call login function with currently used parameters
        /// </summary>
        public string ReLogin()
        {
            tryingtoConnect = true;
            return Login(_lastUser, _lastPassword, _lastHost, _lastPort);
        }
        bool tryingtoConnect = false;
        public string Reconnect()
        {
            while (true)
            {
                Logger.Info("Reconnect");
                Logout(ConnectionStatus.Disconnected, string.Empty);
                if (ReLogin() == string.Empty)
                {
                    Logger.Info("Connected");
                    tryingtoConnect = false;
                    break;
                }
            }
            return string.Empty;
        }
        public void Logout()
        {
            Logout();
        }
        public void Logout(ConnectionStatus status = ConnectionStatus.Disconnected, string reason = null)
        {
            if (_client != null)
            {
                if (_client.State == CommunicationState.Opened && LastConnectionStatus == ConnectionStatus.Connected)
                {
                    try
                    {
                        //UnSubscribeSymbolList(subscribesymbolList);//No need
                        _client.LogOut();
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(string.Empty, ex);
                    }
                }
            }

            DeInitialize();
            UpdateConnectionStatus(status, reason);
        }

        private void UpdateConnectionStatus(ConnectionStatus status, string reason)
        {
            if (status == LastConnectionStatus)
                return;
            LastConnectionStatus = status;

            if (string.IsNullOrEmpty(reason))
                Logger.Info(string.Format("Connection status changed: {0}", status));
            else
                Logger.Info(string.Format("Connection status changed: {0}; reason: {1}", status, reason));

            //if (ConnectionStatusChanged != null)
            //    ConnectionStatusChanged(status, reason);

            switch (status)
            {
                case ConnectionStatus.DisconnectedByServer:
                case ConnectionStatus.ConnectionLost:
                    Logger.Info("Status changed to : "+ status + " Reconnecting");
                    Reconnect();
                    break;
            }
        }

        private void StopHeartBeat()
        {
            _isAlive = false;
            if (_heartbeatThread == null || !_heartbeatThread.IsAlive)
                return;
            _heartbeatThread.Join();
            _heartbeatThread = null;
        }

        #endregion

        #region Server Callbacks

        private void LoginResponseCallback()
        {
            
        }

        private void DataFeedsResponseCallback(DataFeedListResponse message)
        {
            //if (DataFeedsList != null)
            //    DataFeedsList(this, new EventArgs<ServerMess.DataFeed[]>(message.DataFeeds));
            Logger.Info("Recieved data feed from server");
            DataFeed = message.DataFeeds;
            if (DataFeed != null && DataFeed.Count() > 0)
            {
                SubscribeSymbolfromDB();
            }
            else
            {
                Logger.Error("DataFeedsResponseCallback", new Exception("DataFeed List is not available"));
            }
        }
        List<SymbolItem> subscribesymbolList = new List<SymbolItem>();
        private void SubscribeSymbolfromDB()
        {
            IList<OptionProcessingService.Types.Symbol> symbollist = DB.GetSymbols();
            
            foreach (var dbsymbol in symbollist)
            {

                //var symbol=TimestampUtility.BuildSymbolName(dbsymbol.ShortName, dbsymbol.Type == SecurityType.Forex ?
                //CommonObjects.Unitity.Instrument.Forex : CommonObjects.Unitity.Instrument.Equity);
                subscribesymbolList.Add(new ServerMess.SymbolItem()
                {
                    Symbol = dbsymbol.ShortName,
                    Type = dbsymbol.Type==Enums.SecurityType.Currency?Instrument.Forex:Instrument.Equity,
                    DataFeed = DataFeed.Where(x => x.Name == feedName).FirstOrDefault().Name
                });
                System.Console.WriteLine("Subscribing for Symbol : " + dbsymbol.ShortName);
                Logger.Info("Subscribing for Symbol : " + dbsymbol.ShortName);
            }
            SubscribeSymbolList(subscribesymbolList);
        }

        private void TickResponseCallback(NewTickResponse message)
        {
            ////            tracer.ProcessTicks("NewTickResponse", message.Tick);
            //if (Ticks != null)
            //    Ticks(this, new EventArgs<Tick[]>(message.Tick));

            NewTickResponse tickMessage = (NewTickResponse)message;
            List<Quote> QuoteList = new List<Quote>();
            foreach (var tick in tickMessage.Tick)
            {
                Console.WriteLine(string.Format("{0}:{1} - Price:{2}  Volume:{3}", tick.Symbol.DataFeed, tick.Symbol.Symbol, tick.Price, tick.Volume));
                Quote quote = new Quote();
                quote.Symbol.Name = tick.Symbol.Symbol;
                quote.Timestamp = DataProvider.GetCurrentTimestamp(tick.Date);
                quote.Price = tick.Price;
                lock (lastQuotes)
                {
                    Quote tempquote;
                    if (!lastQuotes.TryGetValue(tick.Symbol.Symbol, out tempquote))
                    {
                        lastQuotes.Add(tick.Symbol.Symbol, quote);
                    }
                    else
                    {
                        if (tempquote.Timestamp < quote.Timestamp)
                            lastQuotes[tick.Symbol.Symbol] = quote;
                    }
                }
                QuoteList.Add(quote);

            }
            if (OnNewQuoteResponse != null && QuoteList != null)
            {
                OnNewQuoteResponse(QuoteList);
            }
        }

        private void HistoryResponseCallback(HistoryResponse message)
        {
            //            tracer.ProcessBars("HistoryResponse " + message.ID, message.Bars);
            if (HistoryResponse != null)
                HistoryResponse(this, new EventArgs<HistoryResponse>(message));
        }

        private void HeartbeatResponseCallback(HeartbeatResponse message)
        {
            Logger.Info("Heartbeat");
            if (string.Equals(message.Text, DisconnectByServer))
                Logout(ConnectionStatus.DisconnectedByServer, "Disconnected by server");
        }

        private void ErrorInfoResponseCallback(ErrorInfo message)
        {
            Logger.Info("ErrorInfo message: " + message.Error);
            //if (ErrorInfoResponse != null)
            //    ErrorInfoResponse(this, new EventArgs<string>(message.Error));
        }

        #endregion

        private class CallbackObject : IWCFServiceCallback, IDisposable
        {
            //private readonly Logger _logger = LogManager.GetCurrentClassLogger();
            private Connector _referenceHolder;

            public CallbackObject(Connector referenceHolder)
            {
                _referenceHolder = referenceHolder;
            }

            public void Dispose()
            {
                _referenceHolder = null;
            }

            public void MessageOut(ResponseMessage message)
            {
                if (message is LoginResponse)
                    _referenceHolder.LoginResponseCallback();
                else if (message is DataFeedListResponse)
                    _referenceHolder.DataFeedsResponseCallback((DataFeedListResponse)message);
                else if (message is NewTickResponse)
                    _referenceHolder.TickResponseCallback((NewTickResponse)message);
                else if (message is HistoryResponse)
                    _referenceHolder.HistoryResponseCallback((HistoryResponse)message);
                else if (message is HeartbeatResponse)
                    _referenceHolder.HeartbeatResponseCallback((HeartbeatResponse)message);
                else if (message is ErrorInfo)
                    _referenceHolder.ErrorInfoResponseCallback((ErrorInfo)message);
                else
                    Logger.Info("Unknown income message type: " +message.GetType().Name);
            }
        }
    }
}