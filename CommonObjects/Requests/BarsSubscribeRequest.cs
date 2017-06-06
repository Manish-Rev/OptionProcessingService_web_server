using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Enums;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class BarsSubscribeRequest : Message
    {
        public static string Type
        {
            get { return "BarsSubscribeRequest"; }
        }

        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public Symbol Symbol { get; set; }

        [DataMember]
        public Periodicity Periodicity { get; set; }

        [DataMember]
        public int Interval { get; set; }

        public BarsSubscribeRequest()
        {
            MsgType = Type;
            ID = string.Empty;
        }
    }
}
