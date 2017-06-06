using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class LogoutRequest : Message
    {
        public static string Type
		{
            get { return "LogoutRequest"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        public LogoutRequest()
		{
			MsgType = Type;
            ReqID = string.Empty;
		}
    }
}
