using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class OrderPlaceResponse : Message
    {
        public static string Type
		{
            get { return "OrderPlaceResponse"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public string ClOrdID { get; set; }

        [DataMember]
        public string SrvOrdID { get; set; }

        [DataMember]
        public DateTime? Expiry { get; set; }

        [DataMember]
        public string Error { get; set; }

        public OrderPlaceResponse()
		{
			MsgType = Type;
            ReqID = string.Empty;
            SrvOrdID = string.Empty;
            Expiry = null;
            Error = string.Empty;
		}
    }
}
