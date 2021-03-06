﻿using CommonObjects.Logger;
using OptionProcessingService.ServerMess;
using OptionProcessingService.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace OptionProcessingService.FeedClient
{
    public class DataServerClient: IWCFServiceCallback, IFeedClient
    {
        public event Action<IList<Quote>> OnNewQuoteResponse;

        private WCFServiceClient _client;
        private InstanceContext _context;
        private readonly System.ServiceModel.Channels.Binding _binding = new NetTcpBinding("NetTcpBinding_IWCFService");
        private EndpointAddress _endpoint;
        private DataFeed[] _DataFeeds;
        //private List<Tick> newTicks = new List<Tick>();
        private Dictionary<string, Quote> lastQuotes = new Dictionary<string, Quote>();
        string dataserverip;
        int dataserverport;
        string username;
        string password;
        public DataFeed[] DataFeed
        {
            get { return _DataFeeds; }
            private set { _DataFeeds = value; }
        }

        //public List<Tick> NewTicks
        //{
        //    get { return newTicks; }
        //    set { newTicks = value; }
        //}
        public Dictionary<string, Quote>  LastQuotesList
        {
            get { return lastQuotes; }
            private set  {  lastQuotes = value; }
        }
        public DataServerClient()
        {
            this.dataserverip = System.Configuration.ConfigurationSettings.AppSettings["dataserver_IP"];
            this.dataserverport = Int32.Parse(System.Configuration.ConfigurationSettings.AppSettings["dataserver_port"]);
            this.username = System.Configuration.ConfigurationSettings.AppSettings["dataserver_username"];
            this.password = System.Configuration.ConfigurationSettings.AppSettings["dataserver_password"];
            Start();
        }
        public void Start()
        { 
            _endpoint = new EndpointAddress(string.Format("net.tcp://{0}:{1}/DataServer_Service",dataserverip, dataserverport));
            _context = new InstanceContext(this);
            _client = new WCFServiceClient(_context, _binding, _endpoint);
            try
            {
                Console.WriteLine("Connecting to DataServer");
                Logger.Info("Connecting to DataServer");
                var loginRequest = new LoginRequest { Login = username, Password = password };
                _client.Login(loginRequest);
                Console.WriteLine("Connected to DataServer");
                Logger.Info("Connected to DataServer");
                _client.MessageIn(new DataFeedListRequest());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //public void refreshSymbolTicker(List<SymbolItem> symbolList)
        //{
        //    lock (newTicks)
        //    {
        //        foreach (var symbol in symbolList)
        //        {
        //            newTicks.RemoveAll(x => x.Symbol.Symbol == symbol.Symbol);
        //        }
        //    }
        //}
        public void SubscribeSymbolList(List<SymbolItem> symbolList)
        {
            //refreshSymbolTicker(symbolList);
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
        public void GetHistory(string symbol,string datafeed,int barcount, Periodicity peri,DateTime from,DateTime to)
        {
            int interval = 1;
            Periodicity period = Periodicity.Minute;
            switch (peri)
            {
                case Periodicity.Minute://minute
                    //use default
                    break;
                case Periodicity.Hour:
                    interval = 60;
                    break;
                case Periodicity.Day:
                    period = Periodicity.Day;
                    break;
                case Periodicity.Week:
                    interval = 7;
                    period = Periodicity.Day;
                    break;
                case Periodicity.Tick:
                    interval = 4;
                    period = Periodicity.Tick;
                    break;
                case Periodicity.Range:
                    interval = 6;
                    period = Periodicity.Range;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
            }

            _client.MessageIn(new HistoryRequest()
            {
                Selection = new HistoryParameters()
                {
                    Symbol = new SymbolItem() { DataFeed = datafeed, Symbol = symbol, Type = Instrument.Equity },
                    Id = Guid.NewGuid().ToString(),
                    Periodicity = period,
                    Interval = interval,
                    BarsCount = barcount,
                    From = from,
                    To = to
                }
            });

        }
        #region IWCFServiceCallback

        void IWCFServiceCallback.MessageOut(ResponseMessage message)
        {
            if (message is LoginResponse)
            {

            }
            else if (message is DataFeedListResponse)
            {
                _DataFeeds = ((DataFeedListResponse)message).DataFeeds;
                //InitDataFeedControl();
            }
            else if (message is NewTickResponse)
            {
                NewTickResponse tickMessage = (NewTickResponse)message;
                List<Quote> QuoteList = new List<Quote>();
                foreach (var tick in tickMessage.Tick)
                {
                    Console.WriteLine(string.Format("{0}:{1} - Price:{2}  Volume:{3}", tick.Symbol.DataFeed, tick.Symbol.Symbol, tick.Price, tick.Volume));
                    Quote quote = new Quote();
                    quote.Symbol.Name = tick.Symbol.Symbol;
                    quote.Timestamp = DataProvider.GetCurrentTimestamp(tick.Date);
                    quote.Price = tick.Price;
                    //lock (newQuote)
                    //{
                    //    //newTicks.Add(tick);
                    //    newQuote.Add(quote);
                    //}
                    lock(lastQuotes)
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
                if (OnNewQuoteResponse != null && QuoteList!=null)
                {
                    OnNewQuoteResponse(QuoteList);
                }
            }
            else if (message is HistoryResponse)
            {
                HistoryResponse histMessage = (HistoryResponse)message;
                Console.WriteLine("History Message" + Environment.NewLine);
                foreach (var bar in histMessage.Bars)
                {
                    Console.WriteLine(string.Format("{0} O:{1} H:{2} L:{3} C:{4} V:{5}", bar.Date.ToString(), bar.Open, bar.High, bar.Low, bar.Close, bar.Volume) + Environment.NewLine);
                }
                //if (OnHistoryResponse != null && histMessage != null)
                //{
                //    OnHistoryResponse(histMessage);
                //}
                //ScrollToEnd();
            }
            else if (message is HeartbeatResponse)
            {

            }
            else if (message is ErrorInfo)
            {

            }
            else
            {

            }
        }
        public void NewTick(ddfplus.Quote tick)
        {
            throw new NotImplementedException();
        }
        public void Logout()
        { }
            #endregion

        }
}
