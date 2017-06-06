using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OptionProcessingService.Enums;
using OptionProcessingService.Types;

namespace OptionProcessingService
{
    class BarsSubscription
    {
        public string ID { get; set; }
        public Symbol Symbol { get; set; }
        public Periodicity Periodicity { get; set; }
        public int Interval { get; set; }
        public long LastUpdateTimestamp { get; set; }
        public Bar LastBar { get; set; }

        public BarsSubscription()
        {
            ID = string.Empty;
            Symbol = new Symbol();
        }
    }
}
