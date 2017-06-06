
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonObjects.Unitity
{
    public static class TimestampUtility
    {
        private static readonly DateTime UnixTime = new DateTime(1970, 1, 1);
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GetCurrentTimestamp()
        {
            return (long)(DateTime.UtcNow - UnixTime).TotalMilliseconds;
        }

        public static long GetCurrentTimestamp(DateTime UTCTime)
        {
            return (long)(UTCTime - UnixTime).TotalMilliseconds;
        }

        public static long GetCurrentTimestampFromEST(DateTime ESTTime)
        {
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime UTCtime = TimeZoneInfo.ConvertTimeToUtc(ESTTime, est);
            return (long)(UTCtime - UnixTime).TotalMilliseconds;
        }

        public static long GetStartOfDayTimestamp()
        {
            DateTime startOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0, 0);
            return DateTimeToTimestamp(startOfDay);
        }
        public static long GetEndOfDayTimestamp()
        {
            DateTime startOfDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day+1, 0, 0, 0, 0);
            return DateTimeToTimestamp(startOfDay);
        }
        private static long DateTimeToTimestamp(DateTime value)
        {
            TimeSpan elapsedTime = value.ToUniversalTime() - Epoch;
            return (long)elapsedTime.TotalMilliseconds;
        }

        public static DateTime TimestampToDateTime(double value)
        {
            var epochLocal = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var x = epochLocal.AddMilliseconds(value).ToLocalTime();
            return x;
        }
        //public static string BuildSymbolName(string aStandard, Instrument aType)
        //{
        //    string ret = aStandard;

        //    switch (aType)
        //    {
        //        case Instrument.Equity:
        //            break;
        //        case Instrument.Option:
        //            break;
        //        case Instrument.Forex:
        //            ret = "^" + aStandard.Substring(0, 3) + aStandard.Substring(4, 3);
        //            break;
        //        default:
        //            break;
        //    }

        //    return ret;
        //}
        //public static  bool TryParseSymbol(string aSymbol, out string aStandardName, out Instrument aType)
        //{
        //    switch (aSymbol[0])
        //    {
        //        case '^':
        //            aStandardName = aSymbol.Substring(1, 3) + "/" + aSymbol.Substring(4, 3);
        //            aType = Instrument.Forex;
        //            break;
        //        default:
        //            aStandardName = aSymbol;
        //            aType = Instrument.Equity;
        //            break;
        //    }

        //    return true;
        //}
    }

    public enum Instrument
    {
        Unknown = 0,
        Equity = 1,
        Option = 2,
        Forex = 3
    }
}
