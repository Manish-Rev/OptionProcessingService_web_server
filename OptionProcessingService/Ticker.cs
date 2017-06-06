using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using OptionProcessingService.Enums;
using OptionProcessingService.Responses;
using OptionProcessingService.Types;
using System.Threading.Tasks;
using CommonObjects.Logger;

namespace OptionProcessingService
{

    class Ticker
    {

        #region Constants

        private const int ORDER_FILLER_TIMER_DELAY_MILLIS = 900;
        private const int QOUTES_SUBSCRIBE_TIMER_DELAY_MILLIS = 1000;
        private const int BARS_SUBSCRIBE_TIMER_DELAY_MILLIS = 1000;

        #endregion

        #region Fields

        private Timer OrderFillerTimer = new Timer(ORDER_FILLER_TIMER_DELAY_MILLIS);
        private Timer OrderFeedSubscriberTimer = new Timer(ORDER_FILLER_TIMER_DELAY_MILLIS);
        private Timer QuotesSubscribeTimer     = new Timer(QOUTES_SUBSCRIBE_TIMER_DELAY_MILLIS);
        private Timer BarsSubscribeTimer = new Timer(BARS_SUBSCRIBE_TIMER_DELAY_MILLIS);

        #endregion

        public Ticker()
        {
            OrderFillerTimer.Elapsed += OrderFillerTimerElapsed;
            OrderFeedSubscriberTimer.Elapsed += OrderFeedSubscriberTimer_Elapsed;
            QuotesSubscribeTimer.Elapsed += QuotesSubscribeTimerElapsed;
            BarsSubscribeTimer.Elapsed += BarsSubscribeTimerElapsed;
        }
        bool IsSubscribed = false;

        private void OrderFeedSubscriberTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var assets = DataProvider.GetAssets();
                if (DoesResetTempExoirt)
                {
                    //var timestamp = DataProvider.GetCurrentTimestamp();
                    var expiryList = DataProvider.ExpiryList.Where(x => x.ExpiryTimestamps > DataProvider.GetCurrentTimestamp()).ToList<ExpiryTime>();
                    if (expiryList != null && expiryList.Count > 0)
                    {
                        if (DoesResetTempExoirt)
                        {
                            tempLastExpiry = expiryList.FirstOrDefault<ExpiryTime>();
                            DoesResetTempExoirt = false;
                        }

                        if (tempLastExpiry != null && (((tempLastExpiry.ExpiryTimestamps / 1000) - AppGlobals.Instance.SettlementPriceDuration[OptionType.Classic][0]) <= (DataProvider.GetCurrentTimestamp() / 1000)) && !IsSubscribed)
                        {
                            DoesResetTempExoirt = false;
                            IsSubscribed = true;
                            //TaskFactory.Task
                            DataFeedProcessor.GetFeed();

                            //isupdated = false;
                        }
                        else if (tempLastExpiry != null && (((tempLastExpiry.ExpiryTimestamps / 1000) + AppGlobals.Instance.SettlementPriceDuration[OptionType.Classic][0]) <= (DataProvider.GetCurrentTimestamp() / 1000)) && IsSubscribed)
                        {
                            IsSubscribed = false;
                            DataFeedProcessor.StopFeed();
                            //DoesResetTempExoirt = true;
                            tempLastExpiry = DataProvider.ExpiryList.Where(x => x.ExpiryTimestamps > DataProvider.GetCurrentTimestamp()).FirstOrDefault<ExpiryTime>();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public void Start()
        {
            OrderFillerTimer.Enabled = true;
            OrderFeedSubscriberTimer.Enabled = true;
            QuotesSubscribeTimer.Enabled = true;
            BarsSubscribeTimer.Enabled = true;
        }

        public void Stop()
        {
            OrderFillerTimer.Enabled = false;
            OrderFeedSubscriberTimer.Enabled = false;
            QuotesSubscribeTimer.Enabled = false;
            BarsSubscribeTimer.Enabled = false;
        }
        ExpiryTime tempLastExpiry;
        bool DoesResetTempExoirt = true;
        private void OrderFillerTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var assets = DataProvider.GetAssets();
                bool flag = false;
                lock (assets)
                {
                    List<ExpiryTime> timestamps = new List<ExpiryTime>();
                    foreach (var asset in assets)
                    {
                        // OptionType.Classic
                        timestamps = asset.ExpiryTimeList.Where(n => (n.ExpiryTimestamps / 1000) + AppGlobals.Instance.SettlementPriceDuration[OptionType.Classic][0] <= DataProvider.GetCurrentTimestamp() / 1000).ToList();
                        foreach (var timestamp in timestamps)
                        {
                            if ((timestamp.ExpiryTimestamps / 1000) + AppGlobals.Instance.SettlementPriceDuration[OptionType.Classic][0] <= DataProvider.GetCurrentTimestamp() / 1000)
                            {
                                flag = true;
                                FillOrdersByExpiryTimestamp(timestamp.ExpiryTimestamps);
                                break;
                            }
                        }
                        // OptionType.Express
                        FillOrdersByExpiryTimestamp(DataProvider.GetCurrentTimestamp() / 1000);
                    }
                }
                if (flag)
                {
                    DataProvider.FilterAssets();
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void FillOrdersByExpiryTimestamp(long timestamp)
        {
            var orders = DataProvider.FillOrdersByExpiryTimestamp(timestamp);
            //shashi
            //SendUpdatesAfterOrdersFilled(orders);
        }
        //shashi
        //private void SendUpdatesAfterOrdersFilled(IList<Order> filledOrders)
        //{
        //    while (filledOrders.Count > 0)
        //    {
        //        var userID = filledOrders[0].UserID;
        //        var clientOrders = new List<Order>();
        //        for (var i = filledOrders.Count - 1; i >= 0; i--)
        //        {
        //            if (filledOrders[i].UserID == userID)
        //            {
        //                clientOrders.Add(filledOrders[i]);
        //                filledOrders.RemoveAt(i);
        //            }
        //        }
        //        var client = ClientsCollection.GetByID(userID);
        //        if (client == null) return;

        //        if (client.Subscriptions.OpenOrders)
        //        {
        //            client.SocketSession.Send(JsonSerializeHelper.Serialize(new OpenOrdersSubscribeResponse { Orders = clientOrders }));
        //        }
        //        if (MustSendAccountUpdate(clientOrders) && client.Subscriptions.AccountInfo)
        //        {
        //            foreach (var order in clientOrders)
        //            {
        //                if (order.Successful)
        //                {
        //                    client.AccountInfo.Balance += order.Investment*order.ReturnCoefficient;
        //                }
        //            }
        //            client.SocketSession.Send(JsonSerializeHelper.Serialize(new AccountInfoSubscribeResponse { AccountData = client.AccountInfo }));
        //        }
        //        if (client.Subscriptions.OrderHistory)
        //        {
        //            client.SocketSession.Send(JsonSerializeHelper.Serialize(new OrderHistorySubscribeResponse { Orders = clientOrders }));
        //        }
        //    }
        //}

        private bool MustSendAccountUpdate(IList<Order> orders)
        {
            foreach (var order in orders)
            {
                if (order.Successful)
                {
                    return true;
                }
            }
            return false;
        }

        private void QuotesSubscribeTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                var subscribers = ClientsCollection.GetAll();
                foreach (var subscriber in subscribers)
                {
                    List<string> q = new List<string>();
                    lock (subscriber.Subscriptions.Quotes)
                    {
                        q.AddRange(subscriber.Subscriptions.Quotes);
                    }

                    if (q.Count == 0) continue;

                    var quotes = new List<Quote>();
                    foreach (var symbol in q)
                    {
                        quotes.Add(DataProvider.GetQuoteUpdate(symbol));
                    }
                    if (quotes.Count > 0)
                        subscriber.SocketSession.Send(JsonSerializeHelper.Serialize(new QuotesSubscribeResponse { Quotes = quotes }));
                }
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private void BarsSubscribeTimerElapsed(object sender, ElapsedEventArgs e)
        {
            try { 
            var subscribers = ClientsCollection.GetAll();
            foreach (var subscriber in subscribers)
            {
                List<BarsSubscription> bars = new List<BarsSubscription>();
                lock (subscriber.Subscriptions.Bars)
                {
                    bars.AddRange(subscriber.Subscriptions.Bars);
                }
                foreach (var barOptions in bars)
                {
                    var response = new BarsSubscribeResponse
                    {
                        ID = barOptions.ID,
                        Symbol = barOptions.Symbol,
                        Bars = new List<Bar> { DataProvider.GetBarUpdate(barOptions) }
                    };
                    subscriber.SocketSession.Send(JsonSerializeHelper.Serialize(response));
                }
            }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
