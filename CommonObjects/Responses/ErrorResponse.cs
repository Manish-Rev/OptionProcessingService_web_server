using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class ErrorResponse : Message
    {
        public static string Type
		{
            get { return "ErrorResponse"; }
		}

        [DataMember]
        public string Error { get; set; }

        public ErrorResponse(string text)
		{
			MsgType = Type;
            Error = text;
		}
    }
}
