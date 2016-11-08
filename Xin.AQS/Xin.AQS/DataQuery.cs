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
    public class DataQuery
    {
        private static string selectLiveText = "select * from {0}";

        private static string selectRankText = "select * from {0} order by Rank";

        private static string selectRankForProvinceText = "Select * from {0} where Code like @CodeFormat order by Rank";

        private static string selectHistoryText = "select * from {0} where TimePoint between @BeginTime and @EndTime";
        public static List<T> GetLive<T>(string tableName) where T : class, new()
        {
            string cmdText = string.Format(selectLiveText, tableName);
            List<T> list;
            try
            {
                list = SqlHelper.ExecuteDataTable(cmdText).GetList<T>();
            }
            catch (Exception e)
            {
                list = new List<T>();
                LogHelper.Logger.Error(tableName, e);
            }
            return list;
        }

        public static List<T> GetHistory<T>(string tableName, DateTime beginTime, DateTime endTime) where T : class, new()
        {
            string cmdText = string.Format(selectHistoryText, tableName);
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@BeginTime",beginTime),
                new SqlParameter("@EndTime",endTime)
            };
            List<T> list;
            try
            {
                list = SqlHelper.ExecuteDataTable(cmdText, parameters).GetList<T>();
            }
            catch (Exception e)
            {
                list = new List<T>();
                LogHelper.Logger.Error(tableName, e);
            }
            return list;
        }

        public static List<AirDayAQIRankData> GetNationalCityDayAQIPublishRankData()
        {
            return GetRankData<AirDayAQIRankData>(ConfigHelper.NationalCityDayAQIPublishRankData);
        }

        public static List<AirDayAQIRankData> GetNationalCityDayAQIPublishRankDataForProvince(string provinceCodePre)
        {
            List<AirDayAQIRankData> list;
            try
            {
                string cmdText = string.Format(selectRankForProvinceText, ConfigHelper.NationalCityDayAQIPublishRankData);
                SqlParameter parameter = new SqlParameter("@CodeFormat", string.Format("{0}%", provinceCodePre));
                list = SqlHelper.ExecuteDataTable(cmdText, parameter).GetList<AirDayAQIRankData>();
                DataHelper.UpdateRank(list);
            }
            catch (Exception e)
            {
                list = new List<AirDayAQIRankData>();
                LogHelper.Logger.Error("GetNationalCityDayAQIPublishRankDataForProvince failed.", e);
            }
            return list;
        }

        public static List<AirDayAQIRankData> GetNationalCityDayAQIPublishRankDataForConcerned(string[] cityNames)
        {
            List<AirDayAQIRankData> list = GetRankData<AirDayAQIRankData>(ConfigHelper.NationalCityDayAQIPublishRankData);
            list = list.Where(o => cityNames.Contains(o.Name)).ToList();
            DataHelper.UpdateRank(list);
            return list;
        }

        public static List<AirDayAQCIRankData> GetNationalCityDayAQCIPublishRankData()
        {
            return GetRankData<AirDayAQCIRankData>(ConfigHelper.NationalCityDayAQCIPublishRankData);
        }

        public static List<AirDayAQCIRankData> GetNationalCityDayAQCIPublishRankDataForProvince(string provinceCodePre)
        {
            List<AirDayAQCIRankData> list;
            try
            {
                string cmdText = string.Format(selectRankForProvinceText, ConfigHelper.NationalCityDayAQCIPublishRankData);
                SqlParameter parameter = new SqlParameter("@CodeFormat", string.Format("{0}%", provinceCodePre));
                list = SqlHelper.ExecuteDataTable(cmdText, parameter).GetList<AirDayAQCIRankData>();
                DataHelper.UpdateRank(list);
            }
            catch (Exception e)
            {
                list = new List<AirDayAQCIRankData>();
                LogHelper.Logger.Error("GetNationalCityDayAQCIPublishRankDataForProvince failed.", e);
            }
            return list;
        }

        public static List<AirDayAQCIRankData> GetNationalCityDayAQCIPublishRankDataForConcerned(string[] cityNames)
        {
            List<AirDayAQCIRankData> list = GetRankData<AirDayAQCIRankData>(ConfigHelper.NationalCityDayAQCIPublishRankData);
            list = list.Where(o => cityNames.Contains(o.Name)).ToList();
            DataHelper.UpdateRank(list);
            return list;
        }

        public static List<T> GetDistrictDayAQIPublishHistoryData<T>(DateTime beginTime, DateTime endTime) where T : class, new()
        {
            DataTable dt = GetDistrictDayAQIPublishHistoryDataDT(beginTime, endTime);
            return dt.GetList<T>();
        }
        public static DataTable GetDistrictDayAQIPublishHistoryDataDT(DateTime beginTime, DateTime endTime)
        {
            string tableName = ConfigHelper.DistrictDayAQIPublishHistoryData;
            string cmdText = string.Format(selectHistoryText.Replace("TimePoint", "Time"), tableName);
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@BeginTime",beginTime),
                new SqlParameter("@EndTime",endTime)
            };
            DataTable dt;
            try
            {
                dt = SqlHelper.ExecuteDataTable(cmdText, parameters);
            }
            catch (Exception e)
            {
                dt = new DataTable();
                LogHelper.Logger.Error("GetDistrictDayAQIPublishHistoryData failed.", e);
            }
            dt.TableName = tableName;
            return dt;
        }

        public static List<AirDayAQIRankData> GetDistrictDayAQIPublishRankData()
        {
            return GetRankData<AirDayAQIRankData>(ConfigHelper.DistrictDayAQIPublishRankData);
        }

        public static List<AirDayAQCIRankData> GetDistrictDayAQCIPublishRankData()
        {
            return GetRankData<AirDayAQCIRankData>(ConfigHelper.DistrictDayAQCIPublishRankData);
        }

        public static List<AirHourAQIData> GetDistrictHourAQIPublishData(string districtCode, DateTime beginTime, DateTime endTime)
        {
            List<AirHourAQIData> list;
            try
            {
                string cmdText = string.Format("select * from {0} where Code = @Code, Time >= @BeginTime and Time <= @EndTime", ConfigHelper.DistrictHourAQIPublishData);
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@Code",districtCode),
                    new SqlParameter("@BeginTime",beginTime),
                    new SqlParameter("@EndTime",endTime)
                };
                list = SqlHelper.ExecuteList<AirHourAQIData>(cmdText, parameters);
            }
            catch (Exception e)
            {
                list = new List<AirHourAQIData>();
                LogHelper.Logger.Error("GetDistrictHourAQIPublishData failed.", e);
            }
            return list;
        }

        public static List<AirDayAQIData> GetDistrictDayAQIPublishData(string districtCode, DateTime beginTime, DateTime endTime)
        {
            List<AirDayAQIData> list;
            try
            {
                string cmdText = string.Format("select * from {0} where Code = @Code, Time >= @BeginTime and Time <= @EndTime", ConfigHelper.DistrictDayAQIPublishHistoryData);
                SqlParameter[] parameters = new SqlParameter[]{
                    new SqlParameter("@Code",districtCode),
                    new SqlParameter("@BeginTime",beginTime),
                    new SqlParameter("@EndTime",endTime)
                };
                list = SqlHelper.ExecuteList<AirDayAQIData>(cmdText, parameters);
            }
            catch (Exception e)
            {
                list = new List<AirDayAQIData>();
                LogHelper.Logger.Error("GetDistrictDayAQIPublishData failed.", e);
            }
            return list;
        }

        public static List<AirHourAQIData> GetDistrictHourAQIPublishData(DateTime time)
        {
            List<AirHourAQIData> list;
            try
            {
                string cmdText = string.Format("select * from {0} where Time = @Time", ConfigHelper.DistrictHourAQIPublishData);
                SqlParameter param = new SqlParameter("@Time", time);
                list = SqlHelper.ExecuteList<AirHourAQIData>(cmdText, param);
            }
            catch (Exception e)
            {
                list = new List<AirHourAQIData>();
                LogHelper.Logger.Error("GetDistrictHourAQIPublishData failed.", e);
            }
            return list;
        }

        private static List<T> GetRankData<T>(string tableName) where T : class,new()
        {
            List<T> list;
            try
            {
                string cmdText = string.Format(selectRankText, tableName);
                list = SqlHelper.ExecuteDataTable(cmdText).GetList<T>();
            }
            catch (Exception e)
            {
                list = new List<T>();
                LogHelper.Logger.Error(string.Format("Get {0} failed.", tableName), e);
            }
            return list;
        }
    }
}
