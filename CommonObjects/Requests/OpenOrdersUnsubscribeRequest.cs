using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class OpenOrdersUnsubscribeRequest : Message
    {
        public static string Type
		{
            get { return "OpenOrdersUnsubscribeRequest"; }
		}

        public OpenOrdersUnsubscribeRequest()
		{
			MsgType = Type;
		}
    }
}
