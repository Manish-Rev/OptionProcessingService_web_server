using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class OrderCancelResponse : Message
    {
        public static string Type
		{
            get { return "OrderCancelResponse"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public string SrvOrdID { get; set; }

        [DataMember]
        public string Error { get; set; }

        public OrderCancelResponse()
		{
			MsgType = Type;
            ReqID = string.Empty;
            SrvOrdID = string.Empty;
            Error = string.Empty;
		}
    }
}
