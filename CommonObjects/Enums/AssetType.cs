using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Enums
{
    [DataContract]
    public enum AssetType
    {
        [EnumMember(Value = "callput")]
        CallPut = 0,

        [EnumMember(Value = "abovebelow")]
        AboveBelow = 1,

        [EnumMember(Value = "touchnotouch")]
        TouchNoTouch = 2,

        [EnumMember(Value = "boundary")]
        Boundary = 3,

        [EnumMember(Value = "highyieldboundary")]
        HighYieldBoundary = 4
    }
}