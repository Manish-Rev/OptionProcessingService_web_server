using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using OptionProcessingService.Types;

namespace OptionProcessingService.Responses
{
    [DataContract]
    public class AssetsGetResponse : Message
    {
        public static string Type
		{
            get { return "AssetsGetResponse"; }
		}

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public IList<Asset> Assets { get; set; }

        public AssetsGetResponse()
		{
			MsgType = Type;
            ReqID = string.Empty;
            Assets = new List<Asset>();
		}
    }

    [DataContract]
    public class AssetsGetBySymbolResponse : Message
    {
        public static string Type
        {
            get { return "AssetsGetBySymbolResponse"; }
        }

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public Asset Assets { get; set; }

        public AssetsGetBySymbolResponse()
        {
            MsgType = Type;
            ReqID = string.Empty;
            Assets = new Asset();
        }
    }
}
