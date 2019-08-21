using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPODES.WebAPI
{
    public class LogHelper
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger("LogHelper");

        public static void Debug(string msg)
        {
            logger.Debug(msg);
        }

        public static void Info(string msg)
        {
            logger.Info(msg);
        }

        public static void Warn(string msg)
        {
            logger.Warn(msg);
        }

        public static void Error(string msg)
        {
            logger.Error(msg);
        }

        public static void Fatal(string msg)
        {
            logger.Fatal(msg);
        }
    }
}
