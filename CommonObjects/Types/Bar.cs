using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Types
{
    [DataContract]
    public class Bar
    {
        [DataMember]
        public double Open { get; set; }

        [DataMember]
        public double High { get; set; }

        [DataMember]
        public double Low { get; set; }

        [DataMember]
        public double Close { get; set; }

        [DataMember]
        public long Volume { get; set; }

        [DataMember]
        public long Timestamp { get; set; }

        [DataMember]
        public long UpdateTimestamp { get; set; }

        public Bar()
        {

        }
    }
}
