using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Types
{
    [DataContract]
    public class Bars
    {
        [DataMember]
        public Symbol Symbol { get; set; }

        [DataMember]
        public IList<Bar> BarsData { get; set; }

        public Bars(Symbol symbol, IList<Bar> bars)
        {
            Symbol = symbol;
            BarsData = bars;
        }
    }
}
