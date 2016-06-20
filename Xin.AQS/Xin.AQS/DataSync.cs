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
            selectCountSqlString = "select count(*) [Count] from {0} where Time = @Time";

        }
        public static bool HasData(string tableName, DateTime time)
        {
            string cmdText = string.Format(selectCountSqlString, tableName);
            SqlParameter parameter = new SqlParameter("@Time", time);
            int count = (int)SqlHelper.ExecuteScalar(cmdText, parameter);
            return count > 0;
        }

        public static void SyncNationalCityAQIPublishData(DateTime time, bool live = true)
        {
            string liveTableName = "NationalCityAQIPublishLive";
            string historyTableName = "NationalCityAQIPublishHistory";
            try
            {
                if (!HasData(liveTableName, time))
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
                                DataTable dt = list.GetDataTable<CityAQIPublishLive>(liveTableName);
                                SqlHelper.Insert(dt);
                                MissingData.DeleteMissingData(liveTableName, liveTableName, time);
                                #region Insert HistoryData
                                try
                                {
                                    if (!HasData(historyTableName, time))
                                    {
                                        try
                                        {
                                            dt.TableName = historyTableName;
                                            SqlHelper.Insert(dt);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", historyTableName), e);
                                            MissingData.InsertOrUpateMissingData(historyTableName, historyTableName, time, e.Message);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Error(string.Format("Query {0} failed.", historyTableName), e);
                                }
                                #endregion
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", liveTableName), e);
                                MissingData.InsertOrUpateMissingData(liveTableName, liveTableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpateMissingData(liveTableName, liveTableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", liveTableName), e);
                        MissingData.InsertOrUpateMissingData(liveTableName, liveTableName, time, e.Message);
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
            string tableName = "NationalCityAQIPublishHistory";
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
                                DataTable dt = list.GetDataTable<CityAQIPublishLive>(tableName);
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
            string liveTableName = "NationalCityDayAQIPublishLive";
            string historyTableName = "NationalCityDayAQIPublishHistory";
            string rankTableName = "NationalCityDayAQIPublishRankData";
            try
            {
                if (!HasData(liveTableName, time))
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
                                DataTable dt = list.GetDataTable<CityDayAQIPublishLive>(liveTableName);
                                SqlHelper.Insert(dt);
                                MissingData.DeleteMissingData(liveTableName, liveTableName, time);
                                #region Insert HistoryData
                                try
                                {
                                    if (!HasData(historyTableName, time))
                                    {
                                        try
                                        {
                                            dt.TableName = historyTableName;
                                            SqlHelper.Insert(dt);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", historyTableName), e);
                                            MissingData.InsertOrUpateMissingData(historyTableName, historyTableName, time, e.Message);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Error(string.Format("Query {0} failed.", historyTableName), e);
                                }
                                #endregion
                                #region Insert RankData
                                try
                                {
                                    if (!HasData(rankTableName, time))
                                    {
                                        try
                                        {
                                            List<AirDayAQIRankData> rankDataList = DataConvert.ToAirDayAQIRankData(list);
                                            dt = rankDataList.GetDataTable<AirDayAQIRankData>(rankTableName);
                                            SqlHelper.Insert(dt);
                                        }
                                        catch (Exception e)
                                        {
                                            LogHelper.Logger.Error(string.Format("Insert {0} failed.", rankTableName), e);
                                            MissingData.InsertOrUpateMissingData(rankTableName, rankTableName, time, e.Message);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Error(string.Format("Query {0} failed.", historyTableName), e);
                                }
                                #endregion
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", liveTableName), e);
                                MissingData.InsertOrUpateMissingData(liveTableName, liveTableName, time, e.Message);
                            }
                        }
                        else
                        {
                            MissingData.InsertOrUpateMissingData(liveTableName, liveTableName, time);
                        }
                    }
                    catch (Exception e)
                    {
                        LogHelper.Logger.Error(string.Format("Get {0} failed.", liveTableName), e);
                        MissingData.InsertOrUpateMissingData(liveTableName, liveTableName, time, e.Message);
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
            string tableName = "NationalCityDayAQIPublishHistory";
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
                                DataTable dt = list.GetDataTable<CityDayAQIPublishLive>(tableName);
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
            string tableName = "NationalCityDayAQIPublishRankData";
            try
            {
                if (!HasData(tableName, time))
                {
                    try
                    {
                        List<CityDayAQIPublishLive> list = DataQuery.GetCityDayAQIPublishLive(time);
                        if (list.Any())
                        {
                            try
                            {
                                List<AirDayAQIRankData> rankDataList = DataConvert.ToAirDayAQIRankData(list);
                                DataTable dt = rankDataList.GetDataTable<AirDayAQIRankData>(tableName);
                                SqlHelper.Insert(dt);
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
