using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonObjects.Logger
{
    public static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Error(Exception Error)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    log.Error(Error);
                });
            }
            catch(Exception ex)
            {
                //log.Error(Error);
            }
        }
        public static void Error(string Msg,Exception Error)
        {
            try
            {
                Task.Factory.StartNew(() =>
            {
                log.Error(Msg);
                log.Error(Error);
            });
            }
            catch (Exception ex)
            {
                //log.Error(Error);
            }
        }
        public static void Info(string Info)
        {
            try { 
                Task.Factory.StartNew(() =>
                {
                    log.Info(Info);
                });
            }
            catch (Exception ex)
            {
                //log.Error(Error);
            }
        }
        public static void Log(string UserName,string OrderType,string reqMsg , string resMsg,bool IsError=false)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append(" UserName : " + UserName);
                    builder.Append(" OrderType : " + OrderType);
                    builder.Append(" RequestMessage : " + reqMsg);
                    builder.Append(" ResponseMessage : " + resMsg);
                    if (IsError)
                        log.Error(builder);
                    else
                        log.Info(builder);
                });
            }
            catch (Exception ex)
            {
                //log.Error(Error);
            }
        }
    }
}
