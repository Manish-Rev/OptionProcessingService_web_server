using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class RegistrationResponse : Message
    {
        public static string Type
        {
            get { return "RegistrationResponse"; }
        }

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public string Error { get; set; }

        public RegistrationResponse()
        {
            MsgType = Type;
            ReqID = string.Empty;
            Error = string.Empty;
        }
    }
}