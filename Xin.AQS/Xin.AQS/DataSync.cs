using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xin.AQS.NationalAQIPublishService;
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

        public static void SyncNationalCityAQIPublishData(DateTime time)
        {
            string tableName = "NationalCityAQIPublishLive";
            if (!HasData(tableName, time))
            {
                using (SyncServiceClient client = new SyncServiceClient())
                {
                    CityAQIPublishLive 
                }
            }
        }
    }
}
