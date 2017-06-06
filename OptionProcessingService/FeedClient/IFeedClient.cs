using OptionProcessingService.ServerMess;
using OptionProcessingService.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionProcessingService.FeedClient
{
    public interface IFeedClient
    {
        Dictionary<string, Quote> LastQuotesList
        {
            get;
        }
        event Action<IList<Quote>> OnNewQuoteResponse;
        DataFeed[] DataFeed
        {
            get;
        }
        void SubscribeSymbolList(List<SymbolItem> symbolList);
        void UnSubscribeSymbolList(List<SymbolItem> symbolList);
        void NewTick(ddfplus.Quote quote);
        void Logout();
    }
}
