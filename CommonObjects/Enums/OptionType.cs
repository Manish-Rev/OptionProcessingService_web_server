using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Enums
{

    [DataContract]
    public enum OptionType
    {
        [EnumMember(Value = "Classic")]
        Classic = 0,

        [EnumMember(Value = "Express")]
        Express = 1,

        [EnumMember(Value = "Touch")]
        Touch = 2,

        [EnumMember(Value = "Range")]
        Range = 3
    }

}