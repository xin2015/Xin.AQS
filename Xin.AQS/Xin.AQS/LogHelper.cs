using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// log4net日志
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        public static ILog Logger { get; set; }

        static LogHelper()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo("log4net.config"));
            Logger = LogManager.GetLogger("Logger");
        }
    }
}
