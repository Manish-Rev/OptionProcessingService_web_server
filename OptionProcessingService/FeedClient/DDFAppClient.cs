using CommonObjects.Logger;
using CommonObjects.Unitity;
using OptionProcessingService.Enums;
using OptionProcessingService.ServerMess;
using OptionProcessingService.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionProcessingService.FeedClient
{
    //public class DDFAppClient: IFeedClient
    //{
    //}
    public class DDFAppClient : IFeedClient
    {
        //public event Action<NewTickResponse> OnNewTickResponse;
        public event Action<IList<Quote>> OnNewQuoteResponse;
        //public event Action<HistoryResponse> OnHistoryResponse;


        //private WCFServiceClient _client;
        //private InstanceContext _context;
        //private readonly System.ServiceModel.Channels.Binding _binding = new NetTcpBinding("NetTcpBinding_IWCFService");
        //private EndpointAddress _endpoint;
        private DataFeed[] _DataFeeds;
        private List<Tick> newTicks = new List<Tick>();
        //private List<Quote> newQuote = new List<Quote>();
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

        public List<Tick> NewTicks
        {
            get { return newTicks; }
            set { newTicks = value; }
        }
        //public List<Quote> NewQuote
        //{
        //get { return newQuote; }
        //set { newQuote = value; }
        //}
        public Dictionary<string, Quote> LastQuotesList
        {
            get { return lastQuotes; }
            private set { lastQuotes = value; }
        }
        public DDFAppClient()
        {
            //this.dataserverip = System.Configuration.ConfigurationSettings.AppSettings["dataserver_IP"];
            //this.dataserverport = Int32.Parse(System.Configuration.ConfigurationSettings.AppSettings["dataserver_port"]);
            //this.username = System.Configuration.ConfigurationSettings.AppSettings["dataserver_username"];
            //this.password = System.Configuration.ConfigurationSettings.AppSettings["dataserver_password"];
            //Start();
        }


        public void refreshSymbolTicker(List<SymbolItem> symbolList)
        {
            lock (newTicks)
            {
                foreach (var symbol in symbolList)
                {
                    newTicks.RemoveAll(x => x.Symbol.Symbol == symbol.Symbol);
                }
            }
        }
        public void SubscribeSymbolList(List<SymbolItem> symbolList)
        {
            refreshSymbolTicker(symbolList);
            foreach (var symbol in symbolList)
            {
                //_client.MessageIn(new SubscribeRequest() { Symbol = symbol });

            }
        }

        public void UnSubscribeSymbolList(List<SymbolItem> symbolList)
        {
            foreach (var symbol in symbolList)
            {
                //_client.MessageIn(new UnsubscribeRequest() { Symbol = symbol });
            }
        }

        //#region IWCFServiceCallback

        public void NewTick(ddfplus.Quote tick)
        {
            //if (message is NewTickResponse)
            //{
            //    NewTickResponse tickMessage = (NewTickResponse)message;
            List<Quote> QuoteList = new List<Quote>();
            //    foreach (var tick in tickMessage.Tick)
            //    {
            //        Console.WriteLine(string.Format("{0}:{1} - Price:{2}  Volume:{3}", tick.Symbol.DataFeed, tick.Symbol.Symbol, tick.Price, tick.Volume));

            string aSymbolName = tick.Symbol;
            //CommonObjects.Unitity.Instrument aType;
            //Session session = e.Quote.Sessions[Sessions.Combined];
            //if (!TimestampUtility.TryParseSymbol(tick.Symbol, out aSymbolName, out aType))
            //{
            //    return;
            //}
            Quote quote = new Quote();
            quote.Symbol.Name = aSymbolName;
            //quote.Symbol.Type = aType == CommonObjects.Unitity.Instrument.Forex ? SecurityType.Forex : SecurityType.Stocks;
            quote.Timestamp = DataProvider.GetCurrentTimestamp(tick.Timestamp);
            quote.Price = Math.Round((double)tick.Ask, 5, MidpointRounding.AwayFromZero); //Math.Round((double)tick.Ask, 5, MidpointRounding.AwayFromZero)
            lock (lastQuotes)
            {
                Quote tempquote;
                if (!lastQuotes.TryGetValue(aSymbolName, out tempquote))
                {
                    // lastQuotes.Add(tick.Symbol.Symbol, quote);

                    lastQuotes.Add(aSymbolName, quote);
                }
                else
                {
                    //if (tempquote.Timestamp < quote.Timestamp)
                    //lastQuotes[tick.Symbol.Symbol] = quote;
                    lastQuotes[aSymbolName] = quote;
                }
            }
            QuoteList.Add(quote);

            //}
            if (OnNewQuoteResponse != null && QuoteList != null)
            {
                OnNewQuoteResponse(QuoteList);
            }
        }
        public void Logout()
        { }
    }
}
// #endregion


//}
