using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    public interface IAQIResult
    {
        /// <summary>
        /// 空气质量指数
        /// </summary>
        int? AQI { get; set; }
        string PrimaryPollutant { get; set; }
        string Type { get; set; }
        string Effect { get; set; }
        string Measure { get; set; }

        void GetAQI();
    }
}
