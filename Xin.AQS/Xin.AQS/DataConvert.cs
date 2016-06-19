using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 类型转换工具类
    /// </summary>
    public static class DataConvert
    {
        public static DayAQIResult ToDayAQIResult(AirDayAQIData src)
        {
            DayAQIResult dar = new DayAQIResult();
            dar.SO2 = src.SO2;
            dar.NO2 = src.NO2;
            dar.PM10 = src.PM10;
            dar.CO = src.CO;
            dar.O38H = src.O38H;
            dar.PM25 = src.PM25;
            return dar;
        }

        public static HourAQIResult ToHourAQIResult(AirHourAQIData src)
        {
            HourAQIResult har = new HourAQIResult();
            har.SO2 = src.SO2;
            har.NO2 = src.NO2;
            har.PM10 = src.PM10;
            har.CO = src.CO;
            har.O3 = src.O3;
            har.PM25 = src.PM25;
            return har;
        }

        public static AQCIResult ToAQCIResult(AirDayAQCIData src)
        {
            AQCIResult aqci = new AQCIResult();
            aqci.SO2 = src.SO2;
            aqci.NO2 = src.NO2;
            aqci.PM10 = src.PM10;
            aqci.CO = src.CO;
            aqci.O38H = src.O38H;
            aqci.PM25 = src.PM25;
            return aqci;
        }
    }
}
