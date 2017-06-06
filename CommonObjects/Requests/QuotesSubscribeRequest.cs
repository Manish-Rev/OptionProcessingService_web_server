using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class QuotesSubscribeRequest : Message
    {
        public static string Type
		{
            get { return "QuotesSubscribeRequest"; }
		}

        [DataMember]
        public Symbol Symbol { get; set; }

        public QuotesSubscribeRequest()
		{
			MsgType = Type;
            Symbol = new Symbol();
		}
    }
}
