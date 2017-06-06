using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using OptionProcessingService.Enums;
using OptionProcessingService.Types;
using OptionProcessingService.Responses;
using OptionProcessingService.Requests;
using CommonObjects.Logger;
using CommonObjects.Unitity;

namespace OptionProcessingService
{
    static class DataProvider
    {

        public static event Action<Order, OrderExecutionResponse> OnExecution;

        private static Random random = new Random();
        private static int currentDay = DateTime.Now.DayOfYear;
        private static readonly DateTime UnixTime = new DateTime(1970, 1, 1);
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly Dictionary<string, double> LastPrice = new Dictionary<string, double>();
        private static double timerExpiry = (DateTime.Now.AddDays(1).Date.AddMinutes(10) - DateTime.Now).TotalMilliseconds;
        private static Timer AssetRefreshTimer = new Timer(timerExpiry);

        private static IList<Asset> _assets;
        private static IList<Order> _openOrders = new List<Order>();
        private static IList<Order> _orderHistory = new List<Order>();

        private const int SECOND = 1000;
        private const int MINUTE = 1000 * 60;
        private const int HOUR = 1000 * 60 * 60;

        private static readonly double PriceDifferenece;
        private static readonly int MinNumberOfFeed;
        public static object Utility { get; private set; }

        public static int StopExpiryLimitinMinutes { get; private set; }
        public static List<ExpiryTime> ExpiryList { get; private set; }

        static DataProvider()
        {
            PriceDifferenece= Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["PriceDifferenece"]);
            MinNumberOfFeed= Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MinNumberOfFeed"]);
            StopExpiryLimitinMinutes = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StopExpiryLimitinMinutes"]);
            populateExpiryList();
            RefreshAssets();
            AssetRefreshTimer.Elapsed += AssetRefreshTimerElapsed;
            AssetRefreshTimer.Enabled = true;
            GetOrdersFromDB();
        }
        private static void populateExpiryList()
        {
            ExpiryList = DB.GetExpiryAllDayTimestamp();
            FilterExpiry();
        }
        public static void FilterExpiry()
        {
            lock (ExpiryList)
            {
                var currentTimestamp = GetCurrentTimestamp();
                ExpiryList = ExpiryList.Where(x => x.ExpiryTimestamps > currentTimestamp).ToList<ExpiryTime>();
            }
        }
        private static void GetOrdersFromDB()
        {
            var orders = DB.GetAllOrders();
            for (var i = orders.Count - 1; i >= 0; i--)
            {
                if (orders[i].State == OrderState.Accepted)
                    _openOrders.Add(orders[i]);
                else
                    _orderHistory.Add(orders[i]);
            }
        }

        public static long GetCurrentTimestamp()
        {
            return (long)(DateTime.UtcNow - UnixTime).TotalMilliseconds;
        }

        public static long GetCurrentTimestamp(DateTime UTCTime)
        {
            return (long)(UTCTime - UnixTime).TotalMilliseconds;
        }

        public static long GetCurrentTimestampFromEST(DateTime ESTTime)
        {
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime UTCtime = TimeZoneInfo.ConvertTimeToUtc(ESTTime, est);
            return (long)(UTCtime - UnixTime).TotalMilliseconds;
        }

        public static long GetStartOfDayTimestamp()
        {
            DateTime startOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
            return DateTimeToTimestamp(startOfDay);
        }

        public static bool ValidateUserCredentials(string login, string password)
        {
            if (login == string.Empty || password == string.Empty) return false;
            var user = DB.GetUserByName(login);
            if (user == null) return false;
            if (user.Password != password) return false;
            return true;
        }

        public static Account GetAccountInfo(string username)
        {
            if (username == string.Empty) return null;
            return DB.GetAccountByUserName(username);
        }

        public static bool IsUsernameExists(string username)
        {
            if (username == string.Empty) return false;
            return DB.GetAccountByUserName(username) != null;
        }

        public static bool AddAccount(string login, string password, string email, CreditCard creditCard)
        {
            return DB.AddUser(login, password, email, creditCard);
        }

        public static IList<Asset> GetAssets()
        {
            return _assets;
        }
        public static Asset GetAssets(string Symbol)
        {
            try
            {
                //long currentTimestamp = CommonObjects.Unitity.TimestampUtility.GetCurrentTimestamp();
                var asset = _assets.Where(x => x.Symbol.Name == Symbol).FirstOrDefault<Asset>();
                //asset.ExpiryTimeList = asset.ExpiryTimeList.Where(x => x.ExpiryTimestamps >= currentTimestamp).ToList<ExpiryTime>();
                //if(asset.ExpiryTimeList.Count()>4)
                //asset.ExpiryTimeList = asset.ExpiryTimeList.Take(4).ToList<ExpiryTime>();
                return asset;
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
            }
            return new Asset();
        }

        public static Quote GetQuoteUpdate(string symbol)
        {
           // DataFeedProcessor.SubscribeSymbolList(symbol);
            //System.Threading.Thread.Sleep(1000);
            //DataFeedProcessor.UnSubscribeSymbolList(symbol);
            //List<Tick> symbolTick=DataFeedProcessor.Ticks.Where(x => x.Symbol.Symbol == symbol).ToList<Tick>();
            //double price = symbolTick.Average(x => x.Price);
            return new Quote()
            {
                Symbol = new Symbol { Name = symbol },
                Timestamp = GetCurrentTimestamp(),
                Price = _GetRandomPrice(symbol)
                //Price = price
            };
        }

        public static string ValidateOrder(Account account, Asset asset, OrderPlaceRequest request)
        {
            string msg = string.Empty;
            if (asset == null)
            {
                msg = "Not a valid Asset";
                return msg;
            }
            if (asset.Symbol == null)
            {
                msg = "Not a valid Symbol";
                return msg;
            }
            //shashi-START
            Logger.Info("asset.Symbol.Name: " + asset.Symbol.Name);
            
            //foreach (var x in DataFeedProcessor.LastQuotesList)
            //{
                try
                {
                    var x = DataFeedProcessor.LastQuotesList.Where(y => y.Key == asset.Symbol.Name).FirstOrDefault();
                    if (x.Key != null && x.Value!=null)
                    Logger.Info(" Sumbol: " + x.Key + " x.Value.Price: " + x.Value.Price + " x.Value.Symbol : " + x.Value.Symbol + " x.Value.Timestamp Time : " + TimestampUtility.TimestampToDateTime(x.Value.Timestamp));
                }
                catch(Exception ex)
                {
                    Logger.Error(ex);
                }
            //}
            //shashi-END
            Quote lastquote;
            DataFeedProcessor.LastQuotesList.TryGetValue(asset.Symbol.Name, out lastquote);
            request.CurrentMarketPrice = Math.Round((double)request.CurrentMarketPrice, 5, MidpointRounding.AwayFromZero);
            if (lastquote != null && Math.Abs(request.CurrentMarketPrice - lastquote.Price) > 1000) // PriceDifferenece)
            {
                msg = "Price has been changed, Please submit request again";
            }
            else if(lastquote==null || lastquote.Price==0)
            {
                msg = "No last price found";
            }
            else if(request.Investment > account.Balance)
            {
                msg = msg + "Not a enough balance ";
            }
            else if (request.OptionType ==  OptionType.Classic && !CheckAssetRelevance(asset, request.ExpiryTimestamp))
            {
                msg = msg+ "Order time alredy expired ";
            }
            return msg;
        }

        public static Order PlaceOrder(Account account, Asset asset, OrderPlaceRequest request)
        {
            var order = new Order
            {
                ID = Guid.NewGuid().ToString(),
                UserID = account.ID,
                Symbol = asset.Symbol,
                OptionType = request.OptionType,
                OrderType = request.OrderType,
                ExpressExpiryInSeconds = request.ExpressExpiryInSeconds,
                IsClosable = asset.IsClosable,
                Investment = request.Investment,
                ReturnCoefficient = asset.ReturnCoefficient,
                MarketPrice= Math.Round((double)request.CurrentMarketPrice, 5, MidpointRounding.AwayFromZero),
                State = OrderState.Accepted,
                CreationTimestamp = GetCurrentTimestamp(),
                ExpiryTimestamp = request.ExpiryTimestamp,
                ExecutionTimestamp = 0,
                ExpiryPrice=0,
                Successful = false
            };

            lock (_openOrders)
            {
                _openOrders.Add(order);
            }

            DB.AddOrder(order);

            lock (account)
            {
                account.Balance -= request.Investment;
            }

            DB.UpdateAccount(account);

            return order;
        }

        public static Order CancelOrder(Account account, string ordId)
        {
            lock (_openOrders)
            {
                for (var i = 0; i < _openOrders.Count; i++)
                {
                    var order = _openOrders[i];
                    if (order.ID != ordId) continue;
                    if (!order.IsClosable) continue;

                    _openOrders.RemoveAt(i);
                    order.State = OrderState.Cancelled;

                    lock (_orderHistory)
                    {
                        _orderHistory.Add(order);
                    }

                    DB.UpdateOrder(order);

                    account.Balance += order.Investment;

                    DB.UpdateAccount(account);

                    return order;
                }
            }
            return null;
        }
        public static bool DecideIfBetPlayed(Symbol symbol, OptionType optionType, OrderType orderType, double marketPriceAtThatMoment,double currentMarketPrice)
        {
            switch (optionType)
            {
                case OptionType.Classic:
                case OptionType.Express:
                    {
                        Console.WriteLine("Order Type: " + orderType + "; Current price: " + currentMarketPrice + "; Market at that moment: " + marketPriceAtThatMoment);
                        if ((orderType == OrderType.Call && (currentMarketPrice > marketPriceAtThatMoment)) || (orderType == OrderType.Put && (currentMarketPrice < marketPriceAtThatMoment)))
                        {
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }
        public static IList<Order> FillOrdersByExpiryTimestamp(long expTimestamp)
        {
            List<Order> executedOrders = new List<Order>();
            lock (_openOrders)
            {
                for (var i = _openOrders.Count - 1; i >= 0; i--)
                {
                    var order = _openOrders[i];
                    if (order.OptionType == OptionType.Classic)
                    {
                        if (order.ExpiryTimestamp != expTimestamp)
                        {
                            continue;
                        }
                    }
                    else if (order.OptionType == OptionType.Express)
                    {
                        expTimestamp = DataProvider.GetCurrentTimestamp() + (AppGlobals.Instance.SettlementPriceDuration[OptionType.Express][order.ExpressExpiryInSeconds] * 1000);
                        if (order.ExpiryTimestamp > expTimestamp)
                        {
                            continue;
                        }
                    }
                    List<Quote> symbolTick = DataFeedProcessor.GetQuoteList(order.Symbol.Name);
                    if (symbolTick == null)
                    {
                        continue;
                    }
                    _openOrders.RemoveAt(i);
                    double currentMarketPrice = 0;
                    if (symbolTick.Count > 0) //&& symbolTick.Count > MinNumberOfFeed)
                    {
                        currentMarketPrice = symbolTick.Average(x => x.Price);
                        order.Successful = DecideIfBetPlayed(order.Symbol, order.OptionType, order.OrderType, order.MarketPrice, currentMarketPrice);
                        order.State = OrderState.Executed;
                    }
                    else
                    {
                        order.Reason = "Server unable to recieve required feeds. Count = " + symbolTick.Count;
                        order.State = OrderState.Rejected;
                    }
                    order.ExecuteTimestamp = GetCurrentTimestamp();
                    lock (_orderHistory)
                    {
                        _orderHistory.Add(order);
                    }
                    DB.UpdateOrder(order);
                    var account = DB.GetAccountByUserID(order.UserID);
                    if (order.Successful)
                    {

                        account.Balance += order.Investment + order.Investment * order.ReturnCoefficient;
                        DB.UpdateAccount(account);

                    }
                    else if (order.State == OrderState.Rejected)
                    {
                        account.Balance += order.Investment;
                        DB.UpdateAccount(account);
                    }
                    FireExecution(order, new OrderExecutionResponse() { OrderID = order.ID, Status = order.Successful ? OrderState.Executed : OrderState.Rejected, Message = order.Reason });
                    executedOrders.Add(order);
                }
            }
            return executedOrders;
        }

        public static IList<Order> GetOpenOrders(Account account)
        {
            List<Order> orders = new List<Order>();
            foreach (var order in _openOrders)
            {
                if (order.UserID == account.ID)
                {
                    orders.Add(order);
                }
            }
            return orders;
        }

        public static IList<Order> GetOrderHistory(Account account)
        {
            List<Order> orders = new List<Order>();
            foreach (var order in _orderHistory)
            {
                if (order.UserID == account.ID)
                {
                    orders.Add(order);
                }
            }
            return orders;
        }

        public static Bars GetHistory(Symbol symbol, Enums.Periodicity periodicity, int interval, long startTimestamp)
        {
            return new Bars(symbol, GenerateRandomBars(symbol.Name));
        }

        public static Bars GetHistory(Symbol symbol, Enums.Periodicity periodicity, int interval, int barsCount)
        {
            return new Bars(symbol, GenerateRandomBars(symbol.Name, barsCount));
        }

        public static Types.Bar GetBarUpdate(BarsSubscription barOptions)
        {
            long updateTimestamp = GetCurrentTimestamp();
            long needDiff = barOptions.Interval * _PeriodicityToMillis(barOptions.Periodicity);
            long oldTimestamp = barOptions.LastUpdateTimestamp;

            double lastPrice = _GetRandomPrice(barOptions.Symbol.Name);
            if (barOptions.LastBar == null)
            {
                barOptions.LastBar = _GetRandomBar(lastPrice, updateTimestamp);
            }

            if (oldTimestamp + needDiff <= updateTimestamp)
            {
                barOptions.LastUpdateTimestamp = updateTimestamp;
                oldTimestamp = updateTimestamp;
                barOptions.LastBar = _GetRandomBar(lastPrice, updateTimestamp);
            }
            else
            {
                barOptions.LastBar.Close = lastPrice;
                if (barOptions.LastBar.Close > barOptions.LastBar.High)
                    barOptions.LastBar.High = barOptions.LastBar.Close;
                if (barOptions.LastBar.Close < barOptions.LastBar.Low)
                    barOptions.LastBar.Low = barOptions.LastBar.Close;

                barOptions.LastBar.UpdateTimestamp = updateTimestamp;
            }
            return barOptions.LastBar;
        }


        public static Asset GetAssetById(string id)
        {
            var a = _assets.Where(n => n.ID == id);
            if (a.Count() > 0)
            {
                return a.First();
            }
            else
            {
                return null;
            }
        }

        private static bool CheckAssetRelevance(Asset asset, long suggestedTimestamp)
        {
            foreach (var timestamp in asset.ExpiryTimeList)
            {
                if (timestamp.ExpiryTimestamps == suggestedTimestamp && timestamp.ExpiryTimestamps > GetCurrentTimestamp())
                    return true;
            }
            return false;
        }

        private static void AssetRefreshTimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (currentDay != DateTime.Now.DayOfYear)
            {
                currentDay = DateTime.Now.DayOfYear;
                RefreshAssets();
            }
        }

        private static void RefreshAssets()
        {
            // DB.UpdateAssets(); // TODO: remove in future
            _assets = DB.GetAssets();
            FilterAssets();
        }

        public static void FilterAssets()
        {
            lock (_assets)
            {
                var currentTimestamp = GetCurrentTimestamp();
                for (var i = _assets.Count - 1; i >= 0; i--)
                {
                    var asset = _assets[i];
                    for (var j = asset.ExpiryTimeList.Count - 1; j >= 0; j--)
                    {
                        if (asset.ExpiryTimeList[j].ExpiryTimestamps <= currentTimestamp)
                        {
                            asset.ExpiryTimeList.RemoveAt(j);
                            //asset.ExpiryDatetime.RemoveAt(j);
                        }
                    }
                    if (asset.ExpiryTimeList.Count == 0)
                        _assets.RemoveAt(i);
                }
            }
        }

        private static IList<Types.Bar> GenerateRandomBars(string symbol, int barsCount = 100)
        {
            List<Types.Bar> bars = new List<Types.Bar>();
            try
            {
                string text = System.IO.File.ReadAllText(Path.Combine("Data", "aapl-1day.csv"));
                var lines = text.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                var maxBarsCount = barsCount < lines.Length ? barsCount : lines.Length;
                DateTime lastDate = DateTime.UtcNow;
                double lastPrice = _GetRandomPrice(symbol);
                for (var i = 0; i < maxBarsCount; i++)
                {
                    var timestamp = (long)(lastDate - new DateTime(1970, 1, 1)).TotalMilliseconds;
                    var newBar = _GetRandomBar(lastPrice, timestamp);
                    bars.Add(newBar);

                    lastPrice = newBar.Open;
                    lastDate = lastDate.AddMinutes(-1);
                }
                bars.Reverse();
            }
            catch (Exception e)
            {

            }
            return bars;
        }

        private static Types.Bar _GetRandomBar(double closePrice, long timestamp)
        {
            Types.Bar newBar = new Types.Bar();
            newBar.Close = closePrice;
            newBar.Open = Math.Abs(closePrice * (random.Next() % 2 == 0 ? 1 : -1) + random.NextDouble() * 5);
            newBar.High = Math.Max(newBar.Open, newBar.Close) + random.NextDouble() * 2;
            newBar.Low = Math.Min(newBar.Open, newBar.Close) - random.NextDouble() * 2;
            newBar.Volume = _GetRandomQuantity();
            newBar.Timestamp = timestamp;
            newBar.UpdateTimestamp = timestamp;

            return newBar;
        }

        private static double _GetRandomPrice(string symbol)
        {
            double prevPrice, newPrice;
            if (LastPrice.TryGetValue(symbol, out prevPrice))
            {
                newPrice = Math.Abs(prevPrice * (random.Next() % 2 == 0 ? 1 : -1) + random.NextDouble());
            }
            else
            {
                newPrice = random.NextDouble() * 100;
            }
            newPrice = Math.Round(newPrice, 2);
            LastPrice[symbol] = newPrice;

            return newPrice;
        }

        private static int _GetRandomQuantity(int max = 50)
        {
            return random.Next(1, max);
        }

        private static int _GetRandomVolume()
        {
            return random.Next(100, 10000);
        }

        private static double _GetRandomDouble()
        {
            return Math.Round(random.NextDouble(), 2);
        }

        private static double _GetRandomPrice()
        {
            return Math.Round(random.NextDouble() * 100, 2);
        }

        private static int _GetRandomInt(int min = 0, int max = 1000)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private static long _PeriodicityToMillis(Enums.Periodicity periodicity)
        {
            switch (periodicity)
            {
                case Enums.Periodicity.MINUTE:
                    return 60 * 1000;
                case Enums.Periodicity.HOUR:
                    return 60 * 60 * 1000;
                case Enums.Periodicity.DAY:
                    return 24 * 60 * 60 * 1000;
                case Enums.Periodicity.WEEK:
                    return 7 * 24 * 60 * 60 * 1000;
                case Enums.Periodicity.MONTH:
                    return (long)TimeSpan.FromDays(30).TotalMilliseconds;
                case Enums.Periodicity.YEAR:
                    return (long)TimeSpan.FromDays(365).TotalMilliseconds;
                default:
                    return 60 * 1000;
            }
        }

        private static long DateTimeToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value.ToUniversalTime() - Epoch;
            return (long)elapsedTime.TotalMilliseconds;
        }

        public static DateTime TimestampToDateTime(double value)
        {
            var epochLocal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var x = epochLocal.AddMilliseconds(value).ToLocalTime();
            return x;
        }

        private static void FireExecution(Order o, OrderExecutionResponse execution)
        {
            //User user = _users.FirstOrDefault(u => u.Accounts.FirstOrDefault(a => a.Name == account) != null);
            if (o != null && OnExecution != null)
            {
                OnExecution(o, execution);
            }
            //else
            //{
            //    Log.WriteApplicationException(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, new System.Diagnostics.StackFrame(0, true).GetFileLineNumber(), new Exception("Invalid user"));
            //}
        }

        /*private static IList<Asset> GenerateDailyAssets()
        {
            long startOfDayTimestamp = GetStartOfDayTimestamp();
            return new List<Asset>
            {
                new Asset
                {
                    ID = "0",
                    Type = AssetType.CallPut,
                    Symbol = new Symbol{ Name = "EUR/USD", Type = SecurityType.Forex },
                    ReturnCoefficient = 1.8,
                    IsClosable = true,
                    ExpiryTimestamps = new List<long>
                    {
                        startOfDayTimestamp+HOUR*1,
                        startOfDayTimestamp+HOUR*2,
                        startOfDayTimestamp+HOUR*3,
                        startOfDayTimestamp+HOUR*4,
                        startOfDayTimestamp+HOUR*5,
                        startOfDayTimestamp+HOUR*6,
                        startOfDayTimestamp+HOUR*7,
                        startOfDayTimestamp+HOUR*8,
                        startOfDayTimestamp+HOUR*9,
                        startOfDayTimestamp+HOUR*10,
                        startOfDayTimestamp+HOUR*11,
                        startOfDayTimestamp+HOUR*12,
                        startOfDayTimestamp+HOUR*13,
                        startOfDayTimestamp+HOUR*14,
                        startOfDayTimestamp+HOUR*15,
                        startOfDayTimestamp+HOUR*16,
                        startOfDayTimestamp+HOUR*17,
                        startOfDayTimestamp+HOUR*18,
                        startOfDayTimestamp+HOUR*19,
                        startOfDayTimestamp+HOUR*20,
                        startOfDayTimestamp+HOUR*21,
                        startOfDayTimestamp+HOUR*22,
                        startOfDayTimestamp+HOUR*23,
                        startOfDayTimestamp+HOUR*24
                    }
                },
                new Asset
                {
                    ID = "0",
                    Type = AssetType.CallPut,
                    Symbol = new Symbol{ Name = "Tesco", Type = SecurityType.Stocks },
                    ReturnCoefficient = 1.8,
                    IsClosable = true,
                    ExpiryTimestamps = new List<long>
                    {
                        startOfDayTimestamp+HOUR*1,
                        startOfDayTimestamp+HOUR*2,
                        startOfDayTimestamp+HOUR*3,
                        startOfDayTimestamp+HOUR*4,
                        startOfDayTimestamp+HOUR*5,
                        startOfDayTimestamp+HOUR*6,
                        startOfDayTimestamp+HOUR*7,
                        startOfDayTimestamp+HOUR*8,
                        startOfDayTimestamp+HOUR*9,
                        startOfDayTimestamp+HOUR*10,
                        startOfDayTimestamp+HOUR*11,
                        startOfDayTimestamp+HOUR*12,
                        startOfDayTimestamp+HOUR*13,
                        startOfDayTimestamp+HOUR*14,
                        startOfDayTimestamp+HOUR*15,
                        startOfDayTimestamp+HOUR*16,
                        startOfDayTimestamp+HOUR*17,
                        startOfDayTimestamp+HOUR*18,
                        startOfDayTimestamp+HOUR*19,
                        startOfDayTimestamp+HOUR*20,
                        startOfDayTimestamp+HOUR*21,
                        startOfDayTimestamp+HOUR*22,
                        startOfDayTimestamp+HOUR*23,
                        startOfDayTimestamp+HOUR*24
                    }
                },
                new Asset
                {
                    ID = "0",
                    Type = AssetType.CallPut,
                    Symbol = new Symbol{ Name = "Gold", Type = SecurityType.Commodities },
                    ReturnCoefficient = 1.8,
                    IsClosable = true,
                    ExpiryTimestamps = new List<long>
                    {
                        startOfDayTimestamp+HOUR*1,
                        startOfDayTimestamp+HOUR*2,
                        startOfDayTimestamp+HOUR*3,
                        startOfDayTimestamp+HOUR*4,
                        startOfDayTimestamp+HOUR*5,
                        startOfDayTimestamp+HOUR*6,
                        startOfDayTimestamp+HOUR*7,
                        startOfDayTimestamp+HOUR*8,
                        startOfDayTimestamp+HOUR*9,
                        startOfDayTimestamp+HOUR*10,
                        startOfDayTimestamp+HOUR*11,
                        startOfDayTimestamp+HOUR*12,
                        startOfDayTimestamp+HOUR*13,
                        startOfDayTimestamp+HOUR*14,
                        startOfDayTimestamp+HOUR*15,
                        startOfDayTimestamp+HOUR*16,
                        startOfDayTimestamp+HOUR*17,
                        startOfDayTimestamp+HOUR*18,
                        startOfDayTimestamp+HOUR*19,
                        startOfDayTimestamp+HOUR*20,
                        startOfDayTimestamp+HOUR*21,
                        startOfDayTimestamp+HOUR*22,
                        startOfDayTimestamp+HOUR*23,
                        startOfDayTimestamp+HOUR*24      
                    }
                },
                new Asset
                {
                    ID = "0",
                    Type = AssetType.CallPut,
                    Symbol = new Symbol{ Name = "Dow Jones", Type = SecurityType.Indices },
                    ReturnCoefficient = 1.8,
                    IsClosable = false,
                    ExpiryTimestamps = new List<long>
                    {
                        startOfDayTimestamp+HOUR*1,
                        startOfDayTimestamp+HOUR*2,
                        startOfDayTimestamp+HOUR*3,
                        startOfDayTimestamp+HOUR*4,
                        startOfDayTimestamp+HOUR*5,
                        startOfDayTimestamp+HOUR*6,
                        startOfDayTimestamp+HOUR*7,
                        startOfDayTimestamp+HOUR*8,
                        startOfDayTimestamp+HOUR*9,
                        startOfDayTimestamp+HOUR*10,
                        startOfDayTimestamp+HOUR*11,
                        startOfDayTimestamp+HOUR*12,
                        startOfDayTimestamp+HOUR*13,
                        startOfDayTimestamp+HOUR*14,
                        startOfDayTimestamp+HOUR*15,
                        startOfDayTimestamp+HOUR*16,
                        startOfDayTimestamp+HOUR*17,
                        startOfDayTimestamp+HOUR*18,
                        startOfDayTimestamp+HOUR*19,
                        startOfDayTimestamp+HOUR*20,
                        startOfDayTimestamp+HOUR*21,
                        startOfDayTimestamp+HOUR*22,
                        startOfDayTimestamp+HOUR*23,
                        startOfDayTimestamp+HOUR*24 
                    }
                },
                new Asset
                {
                    ID = "0",
                    Type = AssetType.CallPut,
                    Symbol = new Symbol{ Name = "AUD/USD", Type = SecurityType.Forex },
                    ReturnCoefficient = 1.8,
                    IsClosable = true,
                    ExpiryTimestamps = new List<long>
                    {
                        startOfDayTimestamp+HOUR*1,
                        startOfDayTimestamp+HOUR*2,
                        startOfDayTimestamp+HOUR*3,
                        startOfDayTimestamp+HOUR*4,
                        startOfDayTimestamp+HOUR*5,
                        startOfDayTimestamp+HOUR*6,
                        startOfDayTimestamp+HOUR*7,
                        startOfDayTimestamp+HOUR*8,
                        startOfDayTimestamp+HOUR*9,
                        startOfDayTimestamp+HOUR*10,
                        startOfDayTimestamp+HOUR*11,
                        startOfDayTimestamp+HOUR*12,
                        startOfDayTimestamp+HOUR*13,
                        startOfDayTimestamp+HOUR*14,
                        startOfDayTimestamp+HOUR*15,
                        startOfDayTimestamp+HOUR*16,
                        startOfDayTimestamp+HOUR*17,
                        startOfDayTimestamp+HOUR*18,
                        startOfDayTimestamp+HOUR*19,
                        startOfDayTimestamp+HOUR*20,
                        startOfDayTimestamp+HOUR*21,
                        startOfDayTimestamp+HOUR*22,
                        startOfDayTimestamp+HOUR*23,
                        startOfDayTimestamp+HOUR*24      
                    }
                },
                new Asset
                {
                    ID = "0",
                    Type = AssetType.CallPut,
                    Symbol = new Symbol{ Name = "USD/CAD", Type = SecurityType.Forex },
                    ReturnCoefficient = 1.8,
                    IsClosable = false,
                    ExpiryTimestamps = new List<long>
                    {
                        startOfDayTimestamp+HOUR*1,
                        startOfDayTimestamp+HOUR*2,
                        startOfDayTimestamp+HOUR*3,
                        startOfDayTimestamp+HOUR*4,
                        startOfDayTimestamp+HOUR*5,
                        startOfDayTimestamp+HOUR*6,
                        startOfDayTimestamp+HOUR*7,
                        startOfDayTimestamp+HOUR*8,
                        startOfDayTimestamp+HOUR*9,
                        startOfDayTimestamp+HOUR*10,
                        startOfDayTimestamp+HOUR*11,
                        startOfDayTimestamp+HOUR*12,
                        startOfDayTimestamp+HOUR*13,
                        startOfDayTimestamp+HOUR*14,
                        startOfDayTimestamp+HOUR*15,
                        startOfDayTimestamp+HOUR*16,
                        startOfDayTimestamp+HOUR*17,
                        startOfDayTimestamp+HOUR*18,
                        startOfDayTimestamp+HOUR*19,
                        startOfDayTimestamp+HOUR*20,
                        startOfDayTimestamp+HOUR*21,
                        startOfDayTimestamp+HOUR*22,
                        startOfDayTimestamp+HOUR*23,
                        startOfDayTimestamp+HOUR*24    
                    }
                }
            };
        } */

    }
}