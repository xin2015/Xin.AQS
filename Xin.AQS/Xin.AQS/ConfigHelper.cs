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

        #region 数据库表名
        /// <summary>
        /// 全国城市实时AQI发布Live表
        /// </summary>
        public static string NationalCityAQIPublishLive { get; private set; }
        /// <summary>
        /// 全国城市实时AQI发布History表
        /// </summary>
        public static string NationalCityAQIPublishHistory { get; private set; }
        /// <summary>
        /// 全国城市日均AQI发布Live表
        /// </summary>
        public static string NationalCityDayAQIPublishLive { get; private set; }
        /// <summary>
        /// 全国城市日均AQI发布History表
        /// </summary>
        public static string NationalCityDayAQIPublishHistory { get; private set; }
        /// <summary>
        /// 全国城市日均AQI发布排名表
        /// </summary>
        public static string NationalCityDayAQIPublishRankData { get; private set; }
        /// <summary>
        /// 全国城市日均AQCI发布排名表
        /// </summary>
        public static string NationalCityDayAQCIPublishRankData { get; private set; }
        /// <summary>
        /// 站点实时AQI发布Live表
        /// </summary>
        public static string AQIDataPublishLive { get; private set; }
        /// <summary>
        /// 站点实时AQI发布History表
        /// </summary>
        public static string AQIDataPublishHistory { get; private set; }
        /// <summary>
        /// 城市实时AQI发布表
        /// </summary>
        public static string CityAQIPublish { get; private set; }
        /// <summary>
        /// 城市日均AQI发布表
        /// </summary>
        public static string CityDayAQIPublish { get; private set; }
        /// <summary>
        /// 区域日均AQI发布history表
        /// </summary>
        public static string DistrictDayAQIPublishHistoryData { get; set; }
        /// <summary>
        /// 区域日均AQI发布排名表
        /// </summary>
        public static string DistrictDayAQIPublishRankData { get; set; }
        /// <summary>
        /// 区域日均AQCI发布排名表
        /// </summary>
        public static string DistrictDayAQCIPublishRankData { get; set; }
        #endregion
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
            AQIDataPublishHistory = "AQIDataPublishHistory";
            CityAQIPublish = "CityAQIPublish";
            CityDayAQIPublish = "CityDayAQIPublish";
            DistrictDayAQIPublishHistoryData = "DistrictDayAQIPublishHistoryData";
            DistrictDayAQIPublishRankData = "DistrictDayAQIPublishRankData";
            DistrictDayAQCIPublishRankData = "DistrictDayAQCIPublishRankData";
        }
    }
}
