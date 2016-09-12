using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xin.AQS.SyncNationalAQIPublishDataService;
using Xin.Basic;

namespace Xin.AQS
{
    /// <summary>
    /// 数据同步工具类
    /// </summary>
    public class DataSync
    {
        private static string selectCountSqlString;
        static DataSync()
        {
            selectCountSqlString = "select count(*) [Count] from {0} where {1} = @Time";
        }
        public static bool HasData(string tableName, DateTime time, string timeName = "TimePoint")
        {
            string cmdText = string.Format(selectCountSqlString, tableName, timeName);
            SqlParameter parameter = new SqlParameter("@Time", time);
            int count = (int)SqlHelper.ExecuteScalar(cmdText, parameter);
            return count > 0;
        }

        public static void SyncNationalCityAQIPublishData(DateTime time, bool live = true)
        {
            string liveTable = ConfigHelper.NationalCityAQIPublishLive;
            string historyTable = ConfigHelper.NationalCityAQIPublishHistory;
            try
            {
                if (!HasData(liveTable, time))
                {
                    try
                    {
                        List<CityAQIPublishLive> list;
                        using (SyncNationalAQIPublishDataServiceClient client = new SyncNationalAQIPublishDataServiceClient())
                        {
                            if (live)
                            {
                                list = client.GetCityAQIPublishLive().Where(o => o.TimePoint == time).ToList();
                            }
                            else
                            {
                                list = client.GetCityAQIPublishHistory(time, time).ToList();
                            }
                        }
                        if (list.Any())
                        {
                            try
                            {
                                DataTable dt = list.GetDataTable<CityAQIPublishLive>(liveTable, "ExtensionData");
                                SqlHelper.ExecuteNonQuery(string.Format("delete {0}", liveTable));
                                SqlHelper.Insert(dt);
                                MissingData.Delete(liveTable, liveTable, time);
                                #region Insert HistoryData
                                try
                                {
                                    if (!HasData(historyTable, time))
                                    {
                                        try
                                        {
                                            dt.TableName = historyTable;
                                            SqlHelper.Insert(dt);
                                            MissingData.Delete(historyTable, historyTable, time);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", historyTable), e);
                                            MissingData.InsertOrUpate(historyTable, historyTable, time, e.Message);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Error(string.Format("Query {0} failed.", historyTable), e);
                                }
                                #endregion
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", liveTable), e);
                                MissingData.InsertOrUpate(liveTable, liveTable, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(liveTable, liveTable, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", liveTable), e);
                        MissingData.InsertOrUpate(liveTable, liveTable, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncNationalCityAQIPublishData failed.", e);
            }
        }

        public static void SyncNationalCityAQIPublishHistoryData(DateTime time)
        {
            string tableName = ConfigHelper.NationalCityAQIPublishHistory;
            try
            {
                if (!HasData(tableName, time))
                {
                    try
                    {
                        List<CityAQIPublishLive> list;
                        using (SyncNationalAQIPublishDataServiceClient client = new SyncNationalAQIPublishDataServiceClient())
                        {
                            list = client.GetCityAQIPublishHistory(time, time).ToList();
                        }
                        if (list.Any())
                        {
                            try
                            {
                                DataTable dt = list.GetDataTable<CityAQIPublishLive>(tableName, "ExtensionData");
                                SqlHelper.Insert(dt);
                                MissingData.Delete(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncNationalCityAQIPublishHistoryData failed.", e);
            }
        }

        public static void SyncNationalCityDayAQIPublishData(DateTime time, bool live = true)
        {
            string liveTable = ConfigHelper.NationalCityDayAQIPublishLive;
            string historyTable = ConfigHelper.NationalCityDayAQIPublishHistory;
            string rankTable = ConfigHelper.NationalCityDayAQIPublishRankData;
            try
            {
                if (!HasData(liveTable, time))
                {
                    try
                    {
                        List<CityDayAQIPublishLive> list;
                        using (SyncNationalAQIPublishDataServiceClient client = new SyncNationalAQIPublishDataServiceClient())
                        {
                            if (live)
                            {
                                list = client.GetCityDayAQIPublishLive().Where(o => o.TimePoint == time).ToList();
                            }
                            else
                            {
                                list = client.GetCityDayAQIPublishHistory(time, time).ToList();
                            }
                        }
                        if (list.Any())
                        {
                            try
                            {
                                DataTable dt = list.GetDataTable<CityDayAQIPublishLive>(liveTable, "ExtensionData");
                                SqlHelper.ExecuteNonQuery(string.Format("delete {0}", liveTable));
                                SqlHelper.Insert(dt);
                                MissingData.Delete(liveTable, liveTable, time);
                                #region Insert HistoryData
                                try
                                {
                                    if (!HasData(historyTable, time))
                                    {
                                        try
                                        {
                                            dt.TableName = historyTable;
                                            SqlHelper.Insert(dt);
                                            MissingData.Delete(historyTable, historyTable, time);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", historyTable), e);
                                            MissingData.InsertOrUpate(historyTable, historyTable, time, e.Message);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Error(string.Format("Query {0} failed.", historyTable), e);
                                }
                                #endregion
                                #region Insert RankData
                                try
                                {
                                    if (!HasData(rankTable, time, "Time"))
                                    {
                                        try
                                        {
                                            List<AirDayAQIRankData> rankDataList = DataConvert.ToAirDayAQIRankData(list);
                                            dt = rankDataList.GetDataTable<AirDayAQIRankData>(rankTable, "Effect", "Measure");
                                            SqlHelper.ExecuteNonQuery(string.Format("delete {0}", rankTable));
                                            SqlHelper.Insert(dt);
                                            MissingData.Delete(rankTable, rankTable, time);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", rankTable), e);
                                            MissingData.InsertOrUpate(rankTable, rankTable, time, e.Message);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Error(string.Format("Query {0} failed.", rankTable), e);
                                }
                                #endregion
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", liveTable), e);
                                MissingData.InsertOrUpate(liveTable, liveTable, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(liveTable, liveTable, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", liveTable), e);
                        MissingData.InsertOrUpate(liveTable, liveTable, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncNationalCityDayAQIPublishData failed.", e);
            }
        }

        public static void SyncNationalCityDayAQIPublishHistoryData(DateTime time)
        {
            string tableName = ConfigHelper.NationalCityDayAQIPublishHistory;
            try
            {
                if (!HasData(tableName, time))
                {
                    try
                    {
                        List<CityDayAQIPublishLive> list;
                        using (SyncNationalAQIPublishDataServiceClient client = new SyncNationalAQIPublishDataServiceClient())
                        {
                            list = client.GetCityDayAQIPublishHistory(time, time).ToList();
                        }
                        if (list.Any())
                        {
                            try
                            {
                                DataTable dt = list.GetDataTable<CityDayAQIPublishLive>(tableName, "ExtensionData");
                                SqlHelper.Insert(dt);
                                MissingData.Delete(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncNationalCityDayAQIPublishHistoryData failed.", e);
            }
        }

        public static void SyncNationalCityDayAQIPublishRankData(DateTime time)
        {
            string tableName = ConfigHelper.NationalCityDayAQIPublishRankData;
            try
            {
                if (!HasData(tableName, time, "Time"))
                {
                    try
                    {
                        List<CityDayAQIPublishLive> list = DataQuery.GetHistory<CityDayAQIPublishLive>(ConfigHelper.NationalCityDayAQIPublishHistory, time, time);
                        if (list.Any())
                        {
                            try
                            {
                                List<AirDayAQIRankData> rankDataList = DataConvert.ToAirDayAQIRankData(list);
                                DataTable dt = rankDataList.GetDataTable<AirDayAQIRankData>(tableName, "Effect", "Measure");
                                SqlHelper.ExecuteNonQuery(string.Format("delete {0}", tableName));
                                SqlHelper.Insert(dt);
                                MissingData.Delete(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncNationalCityDayAQIPublishRankData failed.", e);
            }
        }

        public static void SyncNationalCityDayAQCIPublishRankData(DateTime time)
        {
            string tableName = ConfigHelper.NationalCityDayAQCIPublishRankData;
            DateTime beginTime = time.AddDays(1 - time.Day);
            try
            {
                if (!HasData(tableName, time, "Time"))
                {
                    try
                    {
                        List<CityDayAQIPublishLive> list = DataQuery.GetHistory<CityDayAQIPublishLive>(ConfigHelper.NationalCityDayAQIPublishHistory, beginTime, time);
                        if (list.Any())
                        {
                            try
                            {
                                List<AirDayData> airDayDataList = DataConvert.ToAirDayData(list);
                                List<AirDayAQCIRankData> rankDataList = DataHelper.GetAirDayAQCIRankData(airDayDataList, time);
                                DataTable dt = rankDataList.GetDataTable<AirDayAQCIRankData>(tableName);
                                SqlHelper.ExecuteNonQuery(string.Format("delete {0}", tableName));
                                SqlHelper.Insert(dt);
                                MissingData.Delete(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncNationalCityDayAQIPublishRankData failed.", e);
            }
        }

        public static void SyncDistrictHourAQIPublishData(DateTime time)
        {
            string tableName = ConfigHelper.DistrictHourAQIPublishData;
            try
            {
                if (!HasData(tableName, time, "Time"))
                {
                    try
                    {
                        DateTime timePoint = time.AddHours(1);
                        List<AQIDataPublishLive> stationDataList = DataQuery.GetHistory<AQIDataPublishLive>(ConfigHelper.AQIDataPublishHistory, timePoint, timePoint);
                        if (stationDataList.Any())
                        {
                            try
                            {
                                List<AirHourData> stationHourDataList = DataConvert.ToAirHourData(stationDataList);
                                List<District> districtList = District.GetList();
                                List<Station> stationList = Station.GetList();
                                List<AirHourAQIData> list = new List<AirHourAQIData>();
                                districtList.ForEach(o =>
                                {
                                    string[] stationCodes = stationList.Where(p => p.DistrictCode == o.Code).Select(p => p.Code).ToArray();
                                    IEnumerable<AirHourData> tempList = stationHourDataList.Where(p => stationCodes.Contains(p.Code));
                                    if (tempList.Any())
                                    {
                                        AirHourAQIData data = new AirHourAQIData();
                                        data.Code = o.Code;
                                        data.Name = o.Name;
                                        data.Time = time;
                                        data.SO2 = DataHandle.Round(tempList.Average(p => p.SO2));
                                        data.NO2 = DataHandle.Round(tempList.Average(p => p.NO2));
                                        data.PM10 = DataHandle.Round(tempList.Average(p => p.PM10));
                                        data.CO = DataHandle.Round(tempList.Average(p => p.CO), 3);
                                        data.O3 = DataHandle.Round(tempList.Average(p => p.O3));
                                        data.PM25 = DataHandle.Round(tempList.Average(p => p.PM25));
                                        data.GetAQI();
                                        list.Add(data);
                                    }
                                });
                                DataTable dt = list.GetDataTable<AirHourAQIData>(tableName, "Effect", "Measure");
                                SqlHelper.Insert(dt);
                                MissingData.Delete(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncNationalCityHourAQIPublishRankData failed.", e);
            }
        }

        public static void SyncDistrictDayAQIPublishData(DateTime time)
        {
            string tableName = ConfigHelper.DistrictDayAQIPublishHistoryData;
            string rankTable = ConfigHelper.DistrictDayAQIPublishRankData;
            try
            {
                if (!HasData(tableName, time, "Time"))
                {
                    try
                    {
                        DateTime timePoint = time.AddDays(1);
                        List<AQIDataPublishLive> stationDataList = DataQuery.GetHistory<AQIDataPublishLive>(ConfigHelper.AQIDataPublishHistory, timePoint, timePoint);
                        if (stationDataList.Any())
                        {
                            try
                            {
                                List<AirDayData> stationDayDataList = DataConvert.ToAirDayData(stationDataList);
                                List<District> districtList = District.GetList();
                                List<Station> stationList = Station.GetList();
                                List<AirDayAQIRankData> list = new List<AirDayAQIRankData>();
                                districtList.ForEach(o =>
                                {
                                    string[] stationCodes = stationList.Where(p => p.DistrictCode == o.Code).Select(p => p.Code).ToArray();
                                    IEnumerable<AirDayData> tempList = stationDayDataList.Where(p => stationCodes.Contains(p.Code));
                                    if (tempList.Any())
                                    {
                                        AirDayAQIRankData data = new AirDayAQIRankData();
                                        data.Code = o.Code;
                                        data.Name = o.Name;
                                        data.Time = time;
                                        data.SO2 = DataHandle.Round(tempList.Average(p => p.SO2));
                                        data.NO2 = DataHandle.Round(tempList.Average(p => p.NO2));
                                        data.PM10 = DataHandle.Round(tempList.Average(p => p.PM10));
                                        data.CO = DataHandle.Round(tempList.Average(p => p.CO), 3);
                                        data.O38H = DataHandle.Round(tempList.Average(p => p.O38H));
                                        data.PM25 = DataHandle.Round(tempList.Average(p => p.PM25));
                                        data.GetAQI();
                                        list.Add(data);
                                    }
                                });
                                DataTable dt = list.GetDataTable<AirDayAQIRankData>(tableName, "Effect", "Measure", "Rank");
                                SqlHelper.Insert(dt);
                                MissingData.Delete(tableName, tableName, time);
                                try
                                {
                                    if (!HasData(rankTable, time, "Time"))
                                    {
                                        try
                                        {
                                            DataHelper.UpdateRankByAQI(list);
                                            dt = list.GetDataTable<AirDayAQIRankData>(rankTable, "Effect", "Measure");
                                            SqlHelper.ExecuteNonQuery(string.Format("delete {0}", rankTable));
                                            SqlHelper.Insert(dt);
                                            MissingData.Delete(rankTable, rankTable, time);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", rankTable), e);
                                            MissingData.InsertOrUpate(rankTable, rankTable, time, e.Message);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Error("SyncDistrictDayAQIPublishRankData failed.", e);
                                }
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncNationalCityDayAQIPublishRankData failed.", e);
            }
        }

        public static void SyncDistrictDayAQIPublishRankData(DateTime time)
        {
            string tableName = ConfigHelper.DistrictDayAQIPublishRankData;
            try
            {
                if (!HasData(tableName, time, "Time"))
                {
                    try
                    {
                        List<AirDayAQIRankData> list = DataQuery.GetDistrictDayAQIPublishHistoryData<AirDayAQIRankData>(time, time);
                        if (list.Any())
                        {
                            try
                            {
                                DataHelper.UpdateRankByAQI(list);
                                DataTable dt = list.GetDataTable<AirDayAQIRankData>(tableName, "Effect", "Measure");
                                SqlHelper.ExecuteNonQuery(string.Format("delete {0}", tableName));
                                SqlHelper.Insert(dt);
                                MissingData.Delete(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncDistrictDayAQIPublishRankData failed.", e);
            }
        }

        public static void SyncDistrictDayAQCIPublishRankData(DateTime time)
        {
            string tableName = ConfigHelper.DistrictDayAQCIPublishRankData;
            DateTime beginTime = time.AddDays(1 - time.Day);
            try
            {
                if (!HasData(tableName, time, "Time"))
                {
                    try
                    {
                        List<AirDayData> dataList = DataQuery.GetDistrictDayAQIPublishHistoryData<AirDayData>(beginTime, time);
                        if (dataList.Any())
                        {
                            try
                            {
                                List<AirDayAQCIRankData> rankDataList = DataHelper.GetAirDayAQCIRankData(dataList, time);
                                DataTable dt = rankDataList.GetDataTable<AirDayAQCIRankData>(tableName);
                                SqlHelper.ExecuteNonQuery(string.Format("delete {0}", tableName));
                                SqlHelper.Insert(dt);
                                MissingData.Delete(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpate(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpate(tableName, tableName, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncDistrictDayAQCIPublishRankData failed.", e);
            }
        }

        public static void AutoCoverData()
        {
            List<MissingData> list = MissingData.GetList();
            list.ForEach(o =>
            {
                switch (o.TableName)
                {
                    case "NationalCityAQIPublishLive": SyncNationalCityAQIPublishData(o.Time, false); break;
                    case "NationalCityAQIPublishHistory": SyncNationalCityAQIPublishHistoryData(o.Time); break;
                    case "NationalCityDayAQIPublishLive": SyncNationalCityDayAQIPublishData(o.Time, false); break;
                    case "NationalCityDayAQIPublishHistory": SyncNationalCityDayAQIPublishHistoryData(o.Time); break;
                    case "NationalCityDayAQIPublishRankData": SyncNationalCityDayAQIPublishRankData(o.Time); break;
                    case "NationalCityDayAQCIPublishRankData": SyncNationalCityDayAQCIPublishRankData(o.Time); break;
                    case "DistrictDayAQIPublishHistoryData": SyncDistrictDayAQIPublishData(o.Time); break;
                    case "DistrictDayAQIPublishRankData": SyncDistrictDayAQIPublishRankData(o.Time); break;
                    case "DistrictDayAQCIPublishRankData": SyncDistrictDayAQCIPublishRankData(o.Time); break;
                    case "DistrictHourAQIPublishData": SyncDistrictHourAQIPublishData(o.Time); break;
                    default: break;
                }
            });
        }
    }
}
