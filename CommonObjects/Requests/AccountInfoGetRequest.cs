using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class AccountInfoGetRequest : Message
    {
        public static string Type
		{
            get { return "AccountInfoGetRequest"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        public AccountInfoGetRequest()
		{
			MsgType = Type;
            ReqID = string.Empty;
		}
    }
}
