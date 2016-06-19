using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 空气质量综合指数数据
    /// </summary>
    public class AirDayAQCIData: AirDayData, IAQCIResult
    {
        /// <summary>
        /// 空气质量综合指数
        /// </summary>
        public decimal? AQCI { get; set; }
        /// <summary>
        /// 首要污染物
        /// </summary>
        public string PrimaryPollutant { get; set; }

        /// <summary>
        /// 计算空气质量综合指数
        /// </summary>
        public void GetAQCI()
        {
            AQCIResult aqci = DataConvert.ToAQCIResult(this);
            aqci.GetAQCI();
            AQCI = aqci.AQCI;
            PrimaryPollutant = aqci.PrimaryPollutant;
        }
    }
}
