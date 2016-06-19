using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 配置工具类
    /// </summary>
    public class ConfigHelper
    {
        /// <summary>
        /// 空值字符串（不要轻易改动）
        /// </summary>
        public static string EmptyValueString { get; set; }

        static ConfigHelper()
        {
            EmptyValueString = System.Configuration.ConfigurationManager.AppSettings["EmptyValueString"];
        }
    }
}
