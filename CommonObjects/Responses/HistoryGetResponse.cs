using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class HistoryGetResponse : Message
    {
        public static string Type
        {
            get { return "HistoryGetResponse"; }
        }

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public Bars Bars { get; set; }

        public HistoryGetResponse(string reqID, Bars bars)
        {
            MsgType = Type;
            ReqID = reqID;
            Bars = bars;
        }
    }
}
