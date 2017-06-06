using OptionProcessingService.Enums;
using System;
using System.Runtime.Serialization;

namespace OptionProcessingService.Types
{

    [DataContract]
    public class Order
    {

        #region Properties

        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public Symbol Symbol { get; set; }
        [DataMember]
        public OptionType OptionType { get; set; }
        [DataMember]
        public OrderType OrderType { get; set; }
        [DataMember]
        public short ExpressExpiryInSeconds { get; set; }
        [DataMember]
        public bool IsClosable { get; set; }     
        [DataMember]
        public double Investment { get; set; }     
        [DataMember]
        public double ReturnCoefficient { get; set; }
        [DataMember]
        public double MarketPrice { get; set; }
        [DataMember]
        public double ExpiryPrice { get; set; }
        [DataMember]
        public OrderState State { get; set; }
        [DataMember]
        public long CreationTimestamp { get; set; }
        [DataMember]
        public long ExpiryTimestamp { get; set; }
        [DataMember]
        public DateTime? ExpiryTime { get; set; }
        [DataMember]
        public double ProfitLoss { get; set; }
        [DataMember]
        public long ExecutionTimestamp { get; set; }
        [DataMember]
        public long ExecuteTimestamp { get; set; }
        [DataMember]
        public bool Successful { get; set; }
        [DataMember]
        public string Reason { get; set; }

        #endregion

        #region Constructors/Destructors

        public Order()
        {
            ID = string.Empty;
            UserID = string.Empty;
            Symbol = new Symbol();
            Reason = string.Empty;
        }

        #endregion

    }

}
