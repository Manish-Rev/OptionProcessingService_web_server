﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class OrderHistoryGetRequest : Message
    {
        public static string Type
		{
            get { return "OrderHistoryGetRequest"; }
		}

        [DataMember]
        public string ReqID { get; set; }
        
        public OrderHistoryGetRequest()
		{
			MsgType = Type;
            ReqID = string.Empty;
		}
    }
}
