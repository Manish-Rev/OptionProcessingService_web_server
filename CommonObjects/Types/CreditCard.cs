using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Types
{
    [DataContract]
    public class CreditCard
    {
        [DataMember]
        public string OwnerName { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public int ExpiryMonth { get; set; }

        [DataMember]
        public int ExpiryYear { get; set; }

        [DataMember]
        public string CVV { get; set; }

        public CreditCard()
        {
            OwnerName = string.Empty;
            Number = string.Empty;
            CVV = string.Empty;
        }
    }
}
