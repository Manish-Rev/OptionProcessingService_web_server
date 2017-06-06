using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class RegistrationRequest : Message
    {
        public static string Type
        {
            get { return "RegistrationRequest"; }
        }

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public CreditCard Card { get; set; }

        public RegistrationRequest()
        {
            MsgType = Type;
            ReqID = string.Empty;
            Login = string.Empty;
            Password = string.Empty;
            Email = string.Empty;
            Card = new CreditCard();
        }
    }
}