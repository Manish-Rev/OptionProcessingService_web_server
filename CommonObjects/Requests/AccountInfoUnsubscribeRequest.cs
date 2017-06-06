using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class AccountInfoUnsubscribeRequest : Message
    {
        public static string Type
		{
            get { return "AccountInfoUnsubscribeRequest"; }
		}

        public AccountInfoUnsubscribeRequest()
		{
			MsgType = Type;
		}
    }
}
