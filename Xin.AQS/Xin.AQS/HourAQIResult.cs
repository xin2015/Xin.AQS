using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 空气质量指数实时报
    /// </summary>
    public class HourAQIResult : AQIResult
    {
        /// <summary>
        /// 二氧化硫（SO2）1小时平均浓度（μg/m³）
        /// </summary>
        public decimal? SO2 { get; set; }
        /// <summary>
        /// 二氧化硫（SO2）1小时平均浓度空气质量分指数
        /// </summary>
        public int? ISO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）1小时平均浓度（μg/m³）
        /// </summary>
        public decimal? NO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）1小时平均浓度空气质量分指数
        /// </summary>
        public int? INO2 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）1小时平均浓度（μg/m³）
        /// </summary>
        public decimal? PM10 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）1小时平均浓度空气质量分指数
        /// </summary>
        public int? IPM10 { get; set; }
        /// <summary>
        /// 一氧化碳（CO）1小时平均浓度（mg/m³）
        /// </summary>
        public decimal? CO { get; set; }
        /// <summary>
        /// 一氧化碳（CO）1小时平均浓度空气质量分指数
        /// </summary>
        public int? ICO { get; set; }
        /// <summary>
        /// 臭氧（O3）1小时平均浓度（μg/m³）
        /// </summary>
        public decimal? O3 { get; set; }
        /// <summary>
        /// 臭氧（O3）1小时平均浓度空气质量分指数
        /// </summary>
        public int? IO3 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）1小时平均浓度（μg/m³）
        /// </summary>
        public decimal? PM25 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）1小时平均浓度空气质量分指数
        /// </summary>
        public int? IPM25 { get; set; }

        /// <summary>
        /// 计算AQI
        /// </summary>
        public override void GetAQI()
        {
            AQICalculate.CalculateHourAQI(this);
        }
    }
}
