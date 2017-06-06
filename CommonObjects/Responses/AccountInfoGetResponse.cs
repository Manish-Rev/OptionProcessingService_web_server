using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class AccountInfoGetResponse : Message
    {
        public static string Type
		{
            get { return "AccountInfoGetResponse"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public Account AccountData { get; set; }

        public AccountInfoGetResponse()
		{
			MsgType = Type;
            ReqID = string.Empty;
            AccountData = new Account();
		}
    }
}
