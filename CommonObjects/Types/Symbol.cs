using OptionProcessingService.Enums;
using System.Runtime.Serialization;

namespace OptionProcessingService.Types
{

    [DataContract]
    public class Symbol
    {

        #region Properties

        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string ShortName{ get; set; }
        [DataMember]
        public string Feed1ProviderName { get; set; }
        [DataMember]
        public string Feed1Name { get; set; }
        [DataMember]
        public string Feed1ShortName { get; set; }
        [DataMember]
        public string Feed2ProviderName { get; set; }
        [DataMember]
        public string Feed2Name { get; set; }
        [DataMember]
        public string Feed2ShortName { get; set; }
        [DataMember]
        public SecurityType Type { get; set; }

        #endregion

        #region Constructors/Destructors

        public Symbol()
        {
            ID = string.Empty;
            Name = string.Empty;
            ShortName = string.Empty;
            Feed1Name = string.Empty;
            Feed2ShortName = string.Empty;
            Feed1Name = string.Empty;
            Feed2ShortName = string.Empty;
            Feed1ProviderName = string.Empty;
            Feed2ProviderName = string.Empty;
        }

        #endregion

    }

}
