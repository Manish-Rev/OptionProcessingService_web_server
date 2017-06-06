using System;
using System.Collections.Generic;
using OptionProcessingService;
using OptionProcessingService.FeedClient;
using CommonObjects.Logger;
using System.Linq;
using OptionProcessingService.ServerMess;
using CommonObjects.Unitity;
using OptionProcessingService.Enums;

namespace OptionProcessingService
{
    public class OPClient
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            try
            {
                var feedName = System.Configuration.ConfigurationSettings.AppSettings["FeedName"];
                DataFeedProcessor.Start();
                //DataFeedProcessor.SubscribeSymbolList("AAPL");

                //System.Threading.Thread.Sleep(1000 * 10);

                //IList<OptionProcessingService.Types.SymbolNew> symbollist = DB.GetSymbolsNew();
                //foreach (var dbsymbol in symbollist)
                //{
                    
                //    //var symbol=TimestampUtility.BuildSymbolName(dbsymbol.ShortName, dbsymbol.Type == SecurityType.Forex ?
                //    //CommonObjects.Unitity.Instrument.Forex : CommonObjects.Unitity.Instrument.Equity);
                //    DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol=dbsymbol.ShortName, Type = ServerMess.Instrument.Equity, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.Where(x => x.Name == feedName).FirstOrDefault().Name });
                //    System.Console.WriteLine("Subscribing for Symbol : " + dbsymbol.ShortName);
                //}
                string serverIP = System.Configuration.ConfigurationSettings.AppSettings["server_ip"];
                int serverPort = Int32.Parse(System.Configuration.ConfigurationSettings.AppSettings["server_port"]);

                Console.WriteLine("IP: {0}, Port: {1}", serverIP, serverPort);
                Server server = new Server(serverIP, serverPort);
                server.Start();

                Console.WriteLine("Started. Press Enter to exit.");


                Console.Read();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("Logout from server");
            DataFeedProcessor.Stop();
            Console.ReadLine();
        }
        public Server OPServer { get; set; }

        //public OPClient()
        //{
        //    try
        //    {
        //        var feedName = System.Configuration.ConfigurationSettings.AppSettings["FeedName"];
        //        DataFeedProcessor.Start();

        //        //System.Threading.Thread.Sleep(1000 * 10);
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "AAPL.BZ", Type = ServerMess.Instrument.Equity, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "EUR/USD", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "USD/CAD", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "USD/RUB", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "BTC/USD", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "GOOG.BZ", Type = ServerMess.Instrument.Equity, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "AMZN.BZ", Type = ServerMess.Instrument.Equity, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "AUD/CAD", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "AUD/CHF", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "AUD/NZD", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "AUD/USD", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "AUD/JPY", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "CAD/CHF", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });
        //        //DataFeedProcessor.SubscribeSymbolList(new ServerMess.SymbolItem() { Symbol = "CAD/JPY", Type = ServerMess.Instrument.Forex, DataFeed = DataFeedProcessor.DataServerClient.DataFeed.FirstOrDefault(x => x.Name == feedName).Name });

        //        string serverIP = System.Configuration.ConfigurationSettings.AppSettings["server_ip"];
        //        int serverPort = Int32.Parse(System.Configuration.ConfigurationSettings.AppSettings["server_port"]);

        //        OPServer = new Server(serverIP, serverPort);
        //        OPServer.Start();
        //        Logger.Info("Server Started");
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}     
    }   
}
