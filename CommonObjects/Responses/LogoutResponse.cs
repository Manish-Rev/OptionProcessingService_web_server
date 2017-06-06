using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class LogoutResponse : Message
    {
        public static string Type
		{
            get { return "LogoutResponse"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        public LogoutResponse()
		{
			MsgType = Type;
            ReqID = string.Empty;
		}
    }
}
