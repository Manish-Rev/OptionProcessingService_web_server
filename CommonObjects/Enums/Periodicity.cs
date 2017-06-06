using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Enums
{
    [DataContract]
    public enum Periodicity
    {
        [EnumMember(Value = "i")]
        MINUTE = 0,

        [EnumMember(Value = "h")]
        HOUR = 1,

        [EnumMember(Value = "d")]
        DAY = 2,

        [EnumMember(Value = "w")]
        WEEK = 3,

        [EnumMember(Value = "m")]
        MONTH = 4,

        [EnumMember(Value = "y")]
        YEAR = 5,
    }
}
