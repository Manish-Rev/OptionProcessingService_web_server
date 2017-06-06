using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Enums;
using OptionProcessingService.Types;

namespace OptionProcessingService.Requests
{
    [DataContract]
    public class HistoryGetRequest : Message
    {
        public static string Type
        {
            get { return "HistoryGetRequest"; }
        }

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public Symbol Symbol { get; set; }

        [DataMember]
        public int BarsCount { get; set; }

        [DataMember]
        public long StartTimestamp { get; set; }

        [DataMember]
        public Periodicity Periodicity { get; set; }

        [DataMember]
        public int Interval { get; set; }

        public HistoryGetRequest()
        {
            MsgType = Type;
            ReqID = string.Empty;
        }
    }
}
