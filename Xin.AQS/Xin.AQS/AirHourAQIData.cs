using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    public class AirHourAQIData : AirHourData, IAQIResult
    {
        public int? AQI { get; set; }

        public string PrimaryPollutant { get; set; }

        public string Type { get; set; }

        public string Effect { get; set; }

        public string Measure { get; set; }

        public void GetAQI()
        {
            HourAQIResult har = DataConvert.ToHourAQIResult(this);
            har.GetAQI();
            GetAQIResult(har);
        }

        /// <summary>
        /// 将AQI计算结果赋值
        /// </summary>
        /// <param name="src">AQI计算结果</param>
        private void GetAQIResult(AQIResult src)
        {
            AQI = src.AQI;
            PrimaryPollutant = src.PrimaryPollutant;
            Type = src.Type;
            Effect = src.Effect;
            Measure = src.Measure;
        }
    }
}
