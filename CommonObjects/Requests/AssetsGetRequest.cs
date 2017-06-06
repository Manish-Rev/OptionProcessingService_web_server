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
    public class AssetsGetRequest : Message
    {

        #region Properties

        public static string Type
		{
            get { return "AssetsGetRequest"; }
		}
        [DataMember]
        public string ReqID { get; set; }
        [DataMember]
        public OptionType OptionType { get; set; }

        #endregion

        #region Constructors/Destructors

        public AssetsGetRequest()
		{
			MsgType = Type;
            ReqID = string.Empty;
            OptionType = OptionType.Classic;
		}

        #endregion

    }

    [DataContract]
    public class AssetsGetBySymbolRequest : Message
    {
        public static string Type
        {
            get { return "AssetsGetBySymbolRequest"; }
        }

        #region Properties

        [DataMember]
        public string ReqID { get; set; }

        [DataMember]
        public string Symbol { get; set; }

        #endregion

        #region Constructors/Destructors

        public AssetsGetBySymbolRequest()
        {
            MsgType = Type;
            ReqID = string.Empty;
            Symbol = string.Empty;
        }

        #endregion

    }

}
