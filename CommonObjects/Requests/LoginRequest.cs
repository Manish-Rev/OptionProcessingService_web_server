using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class LoginRequest : Message
    {
        public static string Type
        {
            get { return "LoginRequest"; }
        }

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }
        
        public LoginRequest()
        {
            MsgType = Type;
            ReqID = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
        }
    }
}