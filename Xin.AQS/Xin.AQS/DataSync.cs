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
                                SqlHelper.Insert(dt);
                                MissingData.DeleteMissingData(liveTable, liveTable, time);
                                #region Insert HistoryData
                                try
                                {
                                    if (!HasData(historyTable, time))
                                    {
                                        try
                                        {
                                            dt.TableName = historyTable;
                                            SqlHelper.Insert(dt);
                                            MissingData.DeleteMissingData(historyTable, historyTable, time);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", historyTable), e);
                                            MissingData.InsertOrUpateMissingData(historyTable, historyTable, time, e.Message);
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
                                MissingData.InsertOrUpateMissingData(liveTable, liveTable, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpateMissingData(liveTable, liveTable, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", liveTable), e);
                        MissingData.InsertOrUpateMissingData(liveTable, liveTable, time, e.Message);
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
                                MissingData.DeleteMissingData(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpateMissingData(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpateMissingData(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpateMissingData(tableName, tableName, time, e.Message);
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
                                SqlHelper.Insert(dt);
                                MissingData.DeleteMissingData(liveTable, liveTable, time);
                                #region Insert HistoryData
                                try
                                {
                                    if (!HasData(historyTable, time))
                                    {
                                        try
                                        {
                                            dt.TableName = historyTable;
                                            SqlHelper.Insert(dt);
                                            MissingData.DeleteMissingData(historyTable, historyTable, time);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", historyTable), e);
                                            MissingData.InsertOrUpateMissingData(historyTable, historyTable, time, e.Message);
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
                                            SqlHelper.Insert(dt);
                                            MissingData.DeleteMissingData(rankTable, rankTable, time);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", rankTable), e);
                                            MissingData.InsertOrUpateMissingData(rankTable, rankTable, time, e.Message);
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
                                MissingData.InsertOrUpateMissingData(liveTable, liveTable, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpateMissingData(liveTable, liveTable, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", liveTable), e);
                        MissingData.InsertOrUpateMissingData(liveTable, liveTable, time, e.Message);
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
                                MissingData.DeleteMissingData(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpateMissingData(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpateMissingData(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpateMissingData(tableName, tableName, time, e.Message);
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
                                SqlHelper.Insert(dt);
                                MissingData.DeleteMissingData(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpateMissingData(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpateMissingData(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpateMissingData(tableName, tableName, time, e.Message);
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
                                List<AirDayAQCIRankData> rankDataList = new List<AirDayAQCIRankData>();
                                List<AirDayData> airDayAQIDataList = DataConvert.ToAirDayData(list);
                                var groups = airDayAQIDataList.GroupBy(o => o.Code);
                                foreach (var group in groups)
                                {
                                    AirDayAQCIRankData data = new AirDayAQCIRankData();
                                    data.Code = group.Key;
                                    data.Name = group.First().Name;
                                    data.Time = time;
                                    data.SO2 = DataHandle.Round(group.Average(o => o.SO2));
                                    data.NO2 = DataHandle.Round(group.Average(o => o.NO2));
                                    data.PM10 = DataHandle.Round(group.Average(o => o.PM10));
                                    data.PM25 = DataHandle.Round(group.Average(o => o.PM25));
                                    IEnumerable<AirDayData> temp = group.Where(o => o.CO.HasValue).OrderBy(o => o.CO.Value);
                                    if (temp.Count() > 1)
                                    {
                                        data.CO = Math.Round(AQCICalculate.CalculatePercentile(temp.Select(o => o.CO.Value).ToArray(), 0.95M), 1);
                                    }
                                    temp = group.Where(o => o.O38H.HasValue).OrderBy(o => o.O38H.Value);
                                    if (temp.Count() > 1)
                                    {
                                        data.O38H = Math.Round(AQCICalculate.CalculatePercentile(temp.Select(o => o.O38H.Value).ToArray(), 0.90M));
                                    }
                                    data.GetAQCI();
                                    rankDataList.Add(data);
                                }
                                DataHelper.UpdateRankByAQCI(rankDataList);
                                DataTable dt = rankDataList.GetDataTable<AirDayAQCIRankData>(tableName);
                                SqlHelper.Insert(dt);
                                MissingData.DeleteMissingData(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpateMissingData(tableName, tableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpateMissingData(tableName, tableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
                        MissingData.InsertOrUpateMissingData(tableName, tableName, time, e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Logger.Error("SyncNationalCityDayAQIPublishRankData failed.", e);
            }
        }
    }
}
