using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xin.Basic;

namespace Xin.AQS
{
    /// <summary>
    /// 缺失数据记录
    /// </summary>
    public class MissingData
    {
        private static string selectByPrimaryKeyText = "select * from MissingData where TableName=@TableName and Code=@Code and Time=@Time";
        private static string deleteByPrimaryKeyText = "delete MissingData where TableName=@TableName and Code=@Code and Time=@Time";
        private static string selectByTableNameText = "select * from MissingData where TableName=@TableName and MissTimes < 100";
        private static string selectText = "select * from MissingData where MissTimes<100";
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 缺失次数
        /// </summary>
        public int MissTimes { get; set; }
        /// <summary>
        /// 详情
        /// </summary>
        public string Exception { get; set; }

        public MissingData() { }

        public MissingData(string tableName, string code, DateTime time, string exception = "")
        {
            TableName = tableName;
            Code = code;
            Time = time;
            MissTimes = 1;
            Exception = exception;
        }

        public static void InsertOrUpate(string tableName, string code, DateTime time, string exception = "")
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@TableName", tableName),
                new SqlParameter("@Code", code),
                new SqlParameter("@Time", time)
            };
            DataTable dt = SqlHelper.ExecuteDataTable(selectByPrimaryKeyText, parameters);
            dt.TableName = "MissingData";
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["MissTimes"] = (int)dt.Rows[0]["MissTimes"] + 1;
                dt.Rows[0]["Exception"] = exception;
                Delete(tableName, code, time);
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["TableName"] = tableName;
                dr["Code"] = code;
                dr["Time"] = time;
                dr["MissTimes"] = 1;
                dr["Exception"] = exception;
                dt.Rows.Add(dr);
            }
            SqlHelper.Insert(dt);
        }

        public static void Delete(string tableName, string code, DateTime time)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@TableName", tableName),
                new SqlParameter("@Code", code),
                new SqlParameter("@Time", time)
            };
            SqlHelper.ExecuteNonQuery(deleteByPrimaryKeyText, parameters);
        }

        public static List<MissingData> GetList(string tableName)
        {
            SqlParameter parameter = new SqlParameter("@TableName", tableName);
            List<MissingData> list = SqlHelper.ExecuteDataTable(selectByTableNameText, parameter).GetList<MissingData>();
            return list;
        }

        public static List<MissingData> GetList()
        {
            List<MissingData> list = SqlHelper.ExecuteDataTable(selectText).GetList<MissingData>();
            return list;
        }
    }
}
