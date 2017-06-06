using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class OpenOrdersGetRequest : Message
    {
        public static string Type
		{
            get { return "OpenOrdersGetRequest"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        public OpenOrdersGetRequest()
		{
			MsgType = Type;
            ReqID = string.Empty;
		}
    }
}
