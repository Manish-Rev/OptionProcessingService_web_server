using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class OrderCancelRequest : Message
    {
        public static string Type
		{
            get { return "OrderCancelRequest"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public string SrvOrdID { get; set; }

        public OrderCancelRequest()
        {
            MsgType = Type;
            ReqID = string.Empty;
            SrvOrdID = string.Empty;
        }
    }
}
