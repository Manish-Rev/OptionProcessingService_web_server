using CommonObjects.Logger;
using OptionProcessingService.FeedClient;
using OptionProcessingService.ServerMess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionProcessingService
{
    public static class DataFeedProcessor
    {
        private static IFeedClient dataServerClient;
        private static List<Types.Quote> quoteList;
        private static string feedName;
        public static IFeedClient DataServerClient
        {
            get {return dataServerClient;}
        }
        public static void Start()
        {
            try {
                feedName = System.Configuration.ConfigurationSettings.AppSettings["FeedName"];
                if (feedName == "Simulation DataFeed" || feedName == "DDF")
                {
                    dataServerClient = new Connector();
                }
                //else if (feedName == "Simulation DataFeed" || feedName == "DDF")
                //{
                //    dataServerClient = new DataServerClient();
                //    //dataServerClient.Start();
                //}
                else
                {
                    dataServerClient = new DDFAppClient();
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static void Stop()
        {
            dataServerClient.Logout();
        }
        public static void SubscribeSymbolList(SymbolItem sysmbol)
        {
            try
            {
                dataServerClient.SubscribeSymbolList(new List<SymbolItem> { sysmbol });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        static bool isstarted = false;
        public static void GetFeed()
        {
            try
            {
                if (!isstarted)
                {
                    Logger.Info("START Getting feed at: " + System.DateTime.UtcNow);
                    quoteList = new List<Types.Quote>();
                    isstarted = true;
                    dataServerClient.OnNewQuoteResponse += DataServerClient_OnNewQuoteResponse;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private static void DataServerClient_OnNewQuoteResponse(IList<Types.Quote> obj)
        {
            try
            {
                foreach (var x in obj)
                {
                    lock (quoteList)
                    {
                        quoteList.Add(x);
                        Logger.Info("Recived for : " + x.Symbol.Name + " Price : " + x.Price);
                    }
                }
            }catch(Exception ex)
            {
                Logger.Error(ex);
            }
        }

        public static void UnSubscribeSymbolList(SymbolItem sysmbol)
        {
            try
            {
                dataServerClient.UnSubscribeSymbolList(new List<SymbolItem> { sysmbol });
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        public static void StopFeed()
        {
            try
            {
                Logger.Info("STOP Getting feed at: " + System.DateTime.UtcNow);
                isstarted = false;
                dataServerClient.OnNewQuoteResponse -= DataServerClient_OnNewQuoteResponse;
            }catch(Exception ex)
            {
                Logger.Error(ex);
            }
        }
        public static Dictionary<string, Types.Quote> LastQuotesList
        {
            get
            {
                return dataServerClient.LastQuotesList;
            }

        }
        public static List<Types.Quote> QuoteList
        {
            get
            {
                return quoteList;
            }
        }

        public static List<Types.Quote> GetQuoteList(string name)
        {
            if (quoteList == null)
            {
                DataFeedProcessor.GetFeed();
                return null;
            }
            lock (quoteList)
            {
                return quoteList.Where(x => x.Symbol.Name == name || x.Symbol.Feed1ShortName == name || x.Symbol.Feed2ShortName == name).ToList<Types.Quote>();
            }
        }

    }

}
