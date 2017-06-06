using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class BarsUnsubscribeRequest : Message
    {
        public static string Type
        {
            get { return "BarsUnsubscribeRequest"; }
        }

        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public Symbol Symbol { get; set; }

        public BarsUnsubscribeRequest()
        {
            MsgType = Type;
            ID = string.Empty;
        }
    }
}
