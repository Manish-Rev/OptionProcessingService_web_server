using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Types
{
    [DataContract]
    public class Quote
    {
        [DataMember]
        public Symbol Symbol { get; set; }

        [DataMember]
        public long Timestamp { get; set; }

        [DataMember]
        public double Price { get; set; }

        public Quote()
        {
            Symbol = new Symbol();
        }
    }
}
