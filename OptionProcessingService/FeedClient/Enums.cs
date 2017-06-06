using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OptionProcessingService.FeedClient
{
    public enum ConnectionStatus
    {
        Connected,
        Disconnected,
        ConnectionLost,
        DisconnectedByServer
    }
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T val)
        {
            Value = val;
        }

        public T Value { get; set; }
    }

    public class EventArgs<T1, T2> : EventArgs
    {
        public EventArgs(T1 val1, T2 val2)
        {
            Value1 = val1;
            Value2 = val2;
        }

        public T1 Value1 { get; private set; }
        public T2 Value2 { get; private set; }
    }
}
