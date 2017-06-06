using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;
using OptionProcessingService.Enums;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class OpenOrdersSubscribeResponse : Message
    {
        public static string Type
		{
            get { return "OpenOrdersSubscribeResponse"; }
		}

        [DataMember]
        public IList<Order> Orders { get; set; }

        public OpenOrdersSubscribeResponse()
		{
			MsgType = Type;
            Orders = new List<Order>();
		}
    }
    
}
