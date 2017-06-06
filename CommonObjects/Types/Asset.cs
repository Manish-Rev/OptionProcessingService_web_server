using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace OptionProcessingService.Types
{

    [DataContract]
    public class ExpiryTime
    {
        [DataMember]
        public long ExpiryTimestamps{get;set;}
        [DataMember]
        public string ExpiryDatetime { get; set; }
    }

    [DataContract]
    public class Asset
    {

        #region Properties

        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public Symbol Symbol { get; set; }
        [DataMember]
        public double ReturnCoefficient { get; set; }
        [DataMember]
        public bool IsClosable { get; set; }
        [DataMember]
        public DateTime? UTCStartDateTime { get; set; }
        [DataMember]
        public DateTime? UTCEndDateTime { get; set; }
        [DataMember]
        public IList<ExpiryTime> ExpiryTimeList { get; set; }

        #endregion

        #region Constructor/Destructor

        public Asset()
        {
            ID = string.Empty;
            Symbol = new Symbol();
            ExpiryTimeList = new List<ExpiryTime>();
        }

        #endregion

    }

}
