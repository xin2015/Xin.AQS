using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    public class AirDayAQIData: AirDayData, IAQIResult
    {
        public int? AQI { get; set; }

        public string PrimaryPollutant { get; set; }

        public string Type { get; set; }

        public string Effect { get; set; }

        public string Measure { get; set; }

        public void GetAQI()
        {
            DayAQIResult dar = DataConvert.ToDayAQIResult(this);
            dar.GetAQI();
            GetAQIResult(dar);
        }

        public void GetAQIResult(AQIResult src)
        {
            AQI = src.AQI;
            PrimaryPollutant = src.PrimaryPollutant;
            Type = src.Type;
            Effect = src.Effect;
            Measure = src.Measure;
        }
    }
}
