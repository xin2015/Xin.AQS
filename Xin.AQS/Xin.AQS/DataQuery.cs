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
    }
}
