using OptionProcessingService.Enums;
using System;
using System.Collections.Generic;

namespace OptionProcessingService
{
    sealed class AppGlobals
    {

        #region Fields

        private static readonly AppGlobals _Instance = new AppGlobals();
        private Dictionary<OptionType, Dictionary<short, short>> _SettlementPriceDuration = new Dictionary<OptionType, Dictionary<short, short>>();

        #endregion

        #region Properties

        public static AppGlobals Instance
        {
            get { return _Instance; }
        }
        internal Dictionary<OptionType, Dictionary<short, short>> SettlementPriceDuration
        {
            get { return _SettlementPriceDuration; }
        }

        #endregion

        private AppGlobals()
        {
            _SettlementPriceDuration.Add(OptionType.Classic, new Dictionary<short, short>());
            _SettlementPriceDuration[OptionType.Classic].Add(0, Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["SettlementPriceDuration"]));
            _SettlementPriceDuration.Add(OptionType.Express, new Dictionary<short, short>());
            _SettlementPriceDuration[OptionType.Express].Add(30, Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["SettlementPriceDurationExpress30"]));
            _SettlementPriceDuration[OptionType.Express].Add(60, Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["SettlementPriceDurationExpress60"]));
            _SettlementPriceDuration[OptionType.Express].Add(120, Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["SettlementPriceDurationExpress120"]));
            _SettlementPriceDuration[OptionType.Express].Add(300, Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings["SettlementPriceDurationExpress300"]));
        }

    }

}
