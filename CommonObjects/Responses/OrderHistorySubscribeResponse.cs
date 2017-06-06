using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class OrderHistorySubscribeResponse : Message
    {
        public static string Type
		{
            get { return "OrderHistorySubscribeResponse"; }
		}
        
        [DataMember]
        public IList<Order> Orders { get; set; }

        public OrderHistorySubscribeResponse()
		{
			MsgType = Type;
            Orders = new List<Order>();
		}
    }
}
