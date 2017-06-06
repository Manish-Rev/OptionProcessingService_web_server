using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Enums
{

    [DataContract]
    public enum OrderType
    {
        [EnumMember(Value = "Call")]
        Call = 0,
        [EnumMember(Value = "Put")]
        Put = 1,
        [EnumMember(Value = "TouchUp")]
        TouchUp = 2,
        [EnumMember(Value = "NoTouchUp")]
        NoTouchUp = 3,
        [EnumMember(Value = "TouchDown")]
        TouchDown = 4,
        [EnumMember(Value = "NoTouchDown")]
        NoTouchDown = 5,
        [EnumMember(Value = "InRange")]
        InRange = 6,
        [EnumMember(Value = "OutOfRange")]
        OutOfRange = 7
    } 

}