using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class BarsSubscribeResponse : Message
    {
        public static string Type
        {
            get { return "BarsSubscribeResponse"; }
        }

        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public Symbol Symbol { get; set; }

        [DataMember]
        public IList<Bar> Bars { get; set; }

        public BarsSubscribeResponse()
        {
            MsgType = Type;
            Symbol = new Symbol();
            Bars = new List<Bar>();
        }
    }
}
