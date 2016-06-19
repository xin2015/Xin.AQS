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
            string tableName = "NationalCityAQIPublishLive";
            try
            {
                if (!HasData(tableName, time))
                {
                    List<CityAQIPublishLive> list;
                    try
                    {
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
                            DataTable dt = list.GetDataTable<CityAQIPublishLive>(tableName);
                            try
                            {
                                SqlHelper.Insert(dt);
                                dt.TableName = tableName.Replace("Live", "History");
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
                LogHelper.Logger.Error("SyncNationalCityAQIPublishData failed.", e);
            }
        }

        public static void SyncNationalCityDayAQIPublishData(DateTime time, bool live = true)
        {
            string tableName = "NationalCityDayAQIPublishLive";
            try
            {
                if (!HasData(tableName, time))
                {
                    List<CityDayAQIPublishLive> list;
                    try
                    {
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
                            DataTable dt = list.GetDataTable<CityDayAQIPublishLive>(tableName);
                            try
                            {
                                SqlHelper.Insert(dt);
                                dt.TableName = tableName.Replace("Live", "History");
                                SqlHelper.Insert(dt);
                                MissingData.DeleteMissingData(tableName, tableName, time);
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Error(string.Format("Insert {0} failed.", tableName), e);
                                MissingData.InsertOrUpateMissingData(tableName, tableName, time, e.Message);
                            }
                            tableName = "NationalCityDayAQIPublishRankData";
                            List<AirDayAQIRankData> rankDataList=new List<AirDayAQIRankData>();
                            list.ForEach(o=>{
                                
                            })
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
                LogHelper.Logger.Error("SyncNationalCityDayAQIPublishData failed.", e);
            }
        }
    }
}
