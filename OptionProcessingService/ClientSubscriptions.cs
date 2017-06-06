using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptionProcessingService.Enums;
using OptionProcessingService.Types;

namespace OptionProcessingService
{
    class ClientSubscriptions
    {
        public bool AccountInfo { get; set; }
        public bool OpenOrders { get; set; }
        public bool OrderHistory { get; set; }
        public IList<string> Quotes { get; private set; }
        public IList<BarsSubscription> Bars { get; private set; }
        
        public ClientSubscriptions()
        {
            AccountInfo = false;
            OpenOrders = false;
            OrderHistory = false;
            Quotes = new List<string>();
            Bars = new List<BarsSubscription>();
        }
        
        public void AddQuote(string symbol)
        {
            lock (Quotes)
            {
                if (Quotes.IndexOf(symbol) == -1)
                {
                    Quotes.Add(symbol);
                }
            }
        }

        public void RemoveQuote(string symbol)
        {
            lock (Quotes)
            {
                Quotes.Remove(symbol);
            }
        }

        public void AddBarSubscription(string id, string symbol, Periodicity periodicity, int interval)
        {
            lock (Bars)
            {
                Bars.Add(new BarsSubscription
                {
                    ID = id,
                    Symbol = new Symbol { Name = symbol },
                    Periodicity = periodicity,
                    Interval = interval,
                    LastUpdateTimestamp = DataProvider.GetCurrentTimestamp()
                });
            }
        }

        public void RemoveBarSubscription(string id)
        {
            lock (Bars)
            {
                for (int i = 0; i < Bars.Count; i++)
                {
                    if (Bars[i].ID == id)
                    {
                        Bars.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
