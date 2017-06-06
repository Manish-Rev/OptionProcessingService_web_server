using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Enums
{
    [DataContract]
    public enum OrderSide
    {
        [EnumMember(Value = "above")]
        Above = 0,

        [EnumMember(Value = "below")]
        Below = 1
    }
}
