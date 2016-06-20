using System;
using System.Collections.Generic;
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
            List<AirDayAQIRankData> list;
            try
            {
                string cmdText = string.Format(selectRankText, ConfigHelper.NationalCityDayAQIPublishRankData);
                list = SqlHelper.ExecuteDataTable(cmdText).GetList<AirDayAQIRankData>();
            }
            catch (Exception e)
            {
                list = new List<AirDayAQIRankData>();
                LogHelper.Logger.Error("GetNationalCityDayAQIPublishRankData failed.", e);
            }
            return list;
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
            List<AirDayAQIRankData> list;
            try
            {
                string cmdText = string.Format(selectRankText, ConfigHelper.NationalCityDayAQIPublishRankData);
                list = SqlHelper.ExecuteDataTable(cmdText).GetList<AirDayAQIRankData>().Where(o => cityNames.Contains(o.Name)).ToList();
                DataHelper.UpdateRank(list);
            }
            catch (Exception e)
            {
                list = new List<AirDayAQIRankData>();
                LogHelper.Logger.Error("GetNationalCityDayAQIPublishRankDataForProvince failed.", e);
            }
            return list;
        }

        public static List<AirDayAQCIRankData> GetNationalCityDayAQCIPublishRankData()
        {
            List<AirDayAQCIRankData> list;
            try
            {
                string cmdText = string.Format(selectRankText, ConfigHelper.NationalCityDayAQCIPublishRankData);
                list = SqlHelper.ExecuteDataTable(cmdText).GetList<AirDayAQCIRankData>();
            }
            catch (Exception e)
            {
                list = new List<AirDayAQCIRankData>();
                LogHelper.Logger.Error("GetNationalCityDayAQCIPublishRankData failed.", e);
            }
            return list;
        }
    }
}
