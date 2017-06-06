using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class QuotesSubscribeResponse : Message
    {
        public static string Type
		{
            get { return "QuotesSubscribeResponse"; }
		}

        [DataMember]
        public IList<Quote> Quotes { get; set; }

        public QuotesSubscribeResponse()
		{
			MsgType = Type;
            Quotes = new List<Quote>();
		}
    }
}
