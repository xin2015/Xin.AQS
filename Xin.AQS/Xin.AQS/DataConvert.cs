using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xin.AQS.SyncNationalAQIPublishDataService;
using Xin.Basic;

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

        public static AirDayAQIRankData ToAirDayAQIRankData(CityDayAQIPublishLive src)
        {
            AirDayAQIRankData data = new AirDayAQIRankData();
            data.Code = src.CityCode;
            data.Name = src.Area;
            data.Time = src.TimePoint;
            try
            {
                if (src.SO2_24h != ConfigHelper.EmptyValueString)
                {
                    data.SO2 = decimal.Parse(src.SO2_24h);
                }
                if (src.NO2_24h != ConfigHelper.EmptyValueString)
                {
                    data.NO2 = decimal.Parse(src.NO2_24h);
                }
                if (src.PM10_24h != ConfigHelper.EmptyValueString)
                {
                    data.PM10 = decimal.Parse(src.PM10_24h);
                }
                if (src.CO_24h != ConfigHelper.EmptyValueString)
                {
                    data.CO = decimal.Parse(src.CO_24h);
                }
                if (src.O3_8h_24h != ConfigHelper.EmptyValueString)
                {
                    data.O38H = decimal.Parse(src.O3_8h_24h);
                }
                if (src.PM2_5_24h != ConfigHelper.EmptyValueString)
                {
                    data.PM25 = decimal.Parse(src.PM2_5_24h);
                }
                if (src.AQI != ConfigHelper.EmptyValueString)
                {
                    data.AQI = int.Parse(src.AQI);
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("", e);
            }
            data.PrimaryPollutant = src.PrimaryPollutant;
            data.Type = src.Quality;
            return data;
        }
    }
}
