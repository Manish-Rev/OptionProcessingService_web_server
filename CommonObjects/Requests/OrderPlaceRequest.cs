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
    public class OrderPlaceRequest : Message
    {

        #region Properties

        public static string Type
		{
            get { return "OrderPlaceRequest"; }
		}
        [DataMember]
        public string ReqID { get; set; }      
        [DataMember]
        public string ClOrdID { get; set; }
        [DataMember]
        public string OptionID { get; set; }
        [DataMember]
        public Symbol Symbol { get; set; }
        [DataMember]
        public OptionType OptionType { get; set; }
        [DataMember]
        public OrderType OrderType { get; set; }
        [DataMember]
        public short ExpressExpiryInSeconds { get; set; }
        [DataMember]
        public double Investment { get; set; }
        [DataMember]
        public double ReturnCoefficient { get; set; }     
        [DataMember]
        public long ExpiryTimestamp { get; set; }
        [DataMember]
        public double CurrentMarketPrice { get; set; }

        #endregion

        #region Constructors/Destructors

        public OrderPlaceRequest()
        {
            MsgType = Type;
            ReqID = string.Empty;
            ClOrdID = string.Empty;
            OptionID = string.Empty;
            Symbol = new Symbol();
            OptionType = OptionType.Classic;
            OrderType = OrderType.Put;
            CurrentMarketPrice = 0.0;
        }

        #endregion

    }

}
