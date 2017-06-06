using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Types
{

    [DataContract]
    public class Account
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public double Balance { get; set; }

        [DataMember]
        public string Email { get; set; }
        
        public Account()
        {
            ID = string.Empty;
            Username = string.Empty;
            Email = string.Empty;
        }
    }

}
