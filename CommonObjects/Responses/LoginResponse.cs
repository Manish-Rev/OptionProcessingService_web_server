using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class LoginResponse : Message
    {
        public static string Type
        {
            get { return "LoginResponse"; }
        }

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public string Error { get; set; }
        
        public LoginResponse()
        {
            MsgType = Type;
            ReqID = string.Empty;
            Error = string.Empty;
        }
    }
}