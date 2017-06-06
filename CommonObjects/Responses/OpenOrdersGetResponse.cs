using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class OpenOrdersGetResponse : Message
    {
        public static string Type
		{
            get { return "OpenOrdersGetResponse"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public IList<Order> Orders { get; set; }

        public OpenOrdersGetResponse()
		{
			MsgType = Type;
            ReqID = string.Empty;
            Orders = new List<Order>();
		}
    }
}
