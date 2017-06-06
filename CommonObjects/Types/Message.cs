using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace OptionProcessingService.Types
{
    [DataContract]
	public class Message
	{
		[DataMember]
		public string MsgType { get; set; }

        public Message()
        {
            MsgType = string.Empty;
        }
	}
}
