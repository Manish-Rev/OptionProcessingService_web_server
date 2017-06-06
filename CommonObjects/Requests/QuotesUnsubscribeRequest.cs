using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class QuotesUnsubscribeRequest : Message
    {
        public static string Type
		{
            get { return "QuotesUnsubscribeRequest"; }
		}

        [DataMember]
        public Symbol Symbol { get; set; }

        public QuotesUnsubscribeRequest()
		{
			MsgType = Type;
            Symbol = new Symbol();
		}
    }
}
