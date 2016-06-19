using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 空气质量综合指数计算结果
    /// </summary>
    public class AQCIResult
    {
        /// <summary>
        /// 二氧化硫（SO2）24小时平均浓度（μg/m³）
        /// </summary>
        public decimal? SO2 { get; set; }
        /// <summary>
        /// 二氧化硫（SO2）24小时平均浓度空气质量指数
        /// </summary>
        public decimal? ISO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）24小时平均浓度（μg/m³）
        /// </summary>
        public decimal? NO2 { get; set; }
        /// <summary>
        /// 二氧化氮（NO2）24小时平均浓度空气质量指数
        /// </summary>
        public decimal? INO2 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）24小时平均浓度（μg/m³）
        /// </summary>
        public decimal? PM10 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于10μm）24小时平均浓度空气质量指数
        /// </summary>
        public decimal? IPM10 { get; set; }
        /// <summary>
        /// 一氧化碳（CO）24小时平均浓度第95百分位数（mg/m³）
        /// </summary>
        public decimal? CO { get; set; }
        /// <summary>
        /// 一氧化碳（CO）24小时平均浓度第95百分位数空气质量指数
        /// </summary>
        public decimal? ICO { get; set; }
        /// <summary>
        /// 臭氧（O3）最大8小时滑动平均浓度第90百分位数（μg/m³）
        /// </summary>
        public decimal? O38H { get; set; }
        /// <summary>
        /// 臭氧（O3）最大8小时滑动平均浓度第90百分位数空气质量指数
        /// </summary>
        public decimal? IO38H { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）24小时平均浓度（μg/m³）
        /// </summary>
        public decimal? PM25 { get; set; }
        /// <summary>
        /// 颗粒物（粒径小于等于2.5μm）24小时平均浓度空气质量指数
        /// </summary>
        public decimal? IPM25 { get; set; }
        /// <summary>
        /// 空气质量综合指数
        /// </summary>
        public decimal? AQCI { get; set; }
        /// <summary>
        /// 首要污染物
        /// </summary>
        public string PrimaryPollutant { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public AQCIResult()
        {
            PrimaryPollutant = ConfigHelper.EmptyValueString;
        }

        /// <summary>
        /// 计算空气质量综合指数
        /// </summary>
        public void GetAQCI()
        {
            AQCICalculate.CalculateAQCI(this);
        }
    }
}
