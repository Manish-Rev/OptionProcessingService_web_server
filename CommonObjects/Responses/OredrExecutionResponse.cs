using OptionProcessingService.Enums;
using OptionProcessingService.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class OredrExecutionResponse : Message
    {
        public static string Type
        {
            get { return "OredrExecutionResponse"; }
        }
        [DataMember]
        public string OrderID;
        //[DataMember]
        //public DateTime Time;
        [DataMember]
        public OrderState Status;
        [DataMember]
        public Order Order;
        //[DataMember]
        //public decimal LastPrice;
        //[DataMember]
        //public long LastQuantity;
        //[DataMember]
        //public long FilledQuantity;
        //[DataMember]
        //public long LeaveQuantity;
        //[DataMember]
        //public long CancelledQuantity;
        //[DataMember]
        //public decimal AverrageFillPrice;
        [DataMember]
        public string Message;

        public OredrExecutionResponse()
        {
            MsgType = Type;
            OrderID = string.Empty;
            Status = OrderState.Accepted;
            Message = string.Empty;
            Order = new Order();
        }

        //public OredrExecutionResponse(string orderID, DateTime time, OrderState status, decimal lastPrice, long lastQuantity, long filledQuantity, long leaveQuantity, long cancelledQuantity, string message)
        //    : base()
        //{
        //    MsgType = "Execution";
        //    OrderID = orderID;
        //    Time = time;
        //    Status = status;
        //    LastPrice = lastPrice;
        //    LastQuantity = lastQuantity;
        //    FilledQuantity = filledQuantity;
        //    LeaveQuantity = leaveQuantity;
        //    AverrageFillPrice = lastPrice;
        //    Message = message;
        //    CancelledQuantity = cancelledQuantity;
        //}
    }
}
