using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class AccountInfoSubscribeResponse : Message
    {
        public static string Type
		{
            get { return "AccountInfoSubscribeResponse"; }
		}

        [DataMember]
        public Account AccountData { get; set; }

        public AccountInfoSubscribeResponse()
		{
			MsgType = Type;
            AccountData = new Account();
		}
    }
}
