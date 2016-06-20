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
        public static string EmptyValueString { get; private set; }

        public static string NationalCityAQIPublishLive { get; private set; }
        public static string NationalCityAQIPublishHistory { get; private set; }
        public static string NationalCityDayAQIPublishLive { get; private set; }
        public static string NationalCityDayAQIPublishHistory { get; private set; }
        public static string NationalCityDayAQIPublishRankData { get; private set; }
        public static string NationalCityDayAQCIPublishRankData { get; private set; }
        public static string AQIDataPublishLive { get; private set; }
        public static string AQIDataPublishHistory { get; private set; }
        public static string DistrictDayAQIPublishRankData { get; set; }
        public static string DistrictDayAQCIPublishRankData { get; set; }

        static ConfigHelper()
        {
            EmptyValueString = System.Configuration.ConfigurationManager.AppSettings["EmptyValueString"];
            NationalCityAQIPublishLive = "NationalCityAQIPublishLive";
            NationalCityAQIPublishHistory = "NationalCityAQIPublishHistory";
            NationalCityDayAQIPublishLive = "NationalCityDayAQIPublishLive";
            NationalCityDayAQIPublishHistory = "NationalCityDayAQIPublishHistory";
            NationalCityDayAQIPublishRankData = "NationalCityDayAQIPublishRankData";
            NationalCityDayAQCIPublishRankData = "NationalCityDayAQCIPublishRankData";
            AQIDataPublishLive = "AQIDataPublishLive";
            AQIDataPublishHistory = "AQIDataPublishLive";
            DistrictDayAQIPublishRankData = "DistrictDayAQIPublishRankData";
            DistrictDayAQCIPublishRankData = "DistrictDayAQCIPublishRankData";
        }
    }
}
