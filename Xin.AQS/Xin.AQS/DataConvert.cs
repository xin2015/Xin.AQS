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

        public static void ToAirDayData(CityDayAQIPublishLive source, AirDayData target)
        {
            target.Code = source.CityCode;
            target.Name = source.Area;
            target.Time = source.TimePoint;
            try
            {
                if (source.SO2_24h != ConfigHelper.EmptyValueString)
                {
                    target.SO2 = decimal.Parse(source.SO2_24h);
                }
                if (source.NO2_24h != ConfigHelper.EmptyValueString)
                {
                    target.NO2 = decimal.Parse(source.NO2_24h);
                }
                if (source.PM10_24h != ConfigHelper.EmptyValueString)
                {
                    target.PM10 = decimal.Parse(source.PM10_24h);
                }
                if (source.CO_24h != ConfigHelper.EmptyValueString)
                {
                    target.CO = decimal.Parse(source.CO_24h);
                }
                if (source.O3_8h_24h != ConfigHelper.EmptyValueString)
                {
                    target.O38H = decimal.Parse(source.O3_8h_24h);
                }
                if (source.PM2_5_24h != ConfigHelper.EmptyValueString)
                {
                    target.PM25 = decimal.Parse(source.PM2_5_24h);
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("CityDayAQIPublishLive To AirDayData", e);
            }
        }

        public static void ToAirDayAQIData(CityDayAQIPublishLive source, AirDayAQIData target)
        {
            ToAirDayData(source, target);
            try
            {
                if (source.AQI != ConfigHelper.EmptyValueString)
                {
                    target.AQI = int.Parse(source.AQI);
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("CityDayAQIPublishLive To AirDayAQIData", e);
            }
            target.PrimaryPollutant = source.PrimaryPollutant;
            target.Type = source.Quality;
            target.Effect = source.Unheathful;
            target.Measure = source.Measure;
        }

        public static AirDayAQIData ToAirDayAQIData(CityDayAQIPublishLive src)
        {
            AirDayAQIData data = new AirDayAQIData();
            ToAirDayAQIData(src, data);
            return data;
        }

        public static List<AirDayAQIData> ToAirDayAQIData(List<CityDayAQIPublishLive> src)
        {
            List<AirDayAQIData> list = new List<AirDayAQIData>();
            src.ForEach(o => list.Add(ToAirDayAQIData(o)));
            return list;
        }

        public static AirDayAQIRankData ToAirDayAQIRankData(CityDayAQIPublishLive src)
        {
            AirDayAQIRankData data = new AirDayAQIRankData();
            ToAirDayAQIData(src, data);
            return data;
        }

        public static List<AirDayAQIRankData> ToAirDayAQIRankData(List<CityDayAQIPublishLive> src)
        {
            List<AirDayAQIRankData> list = new List<AirDayAQIRankData>();
            src.ForEach(o => list.Add(ToAirDayAQIRankData(o)));
            DataHelper.UpdateRankByAQI(list);
            return list;
        }
    }
}
