using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Enums
{
    [DataContract]
    public enum SecurityType
    {
        [EnumMember(Value = "Currency")]
        Currency = 1,

        [EnumMember(Value = "Stock")]
        Stock = 2,

        [EnumMember(Value = "Bond")]
        Bond = 3,

        [EnumMember(Value = "Commodities")]
        Commodities = 4,

        [EnumMember(Value = "Stock Indices Futures")]
        StockIndicesFutures = 5
            
    }
}
