using CommonObjects.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingSessionReset
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
                Logger.Info("Stated refreshing Data");
                DB.UpdateAssetsNew();
                DB.UpdateExpiryAllDayTimestamp();
                Logger.Info("Completed refreshing Data");
            }
            catch(Exception ex)
            {
                Logger.Error(ex);
                Logger.Info("Failed refreshing Data");
            }           
        }
    }
}
