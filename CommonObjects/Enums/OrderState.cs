using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Enums
{
    [DataContract]
    public enum OrderState
    {
        [EnumMember(Value = "accepted")]
        Accepted = 0,
        
        [EnumMember(Value = "executed")]
        Executed = 1,

        [EnumMember(Value = "canceled")]
        Cancelled = 2,

        [EnumMember(Value = "rejected")]
        Rejected = 3
    }
}
