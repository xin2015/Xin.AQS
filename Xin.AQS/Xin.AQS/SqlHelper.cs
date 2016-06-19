using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 数据库工具类
    /// </summary>
    public static class SqlHelper
    {
        /// <summary>
        /// 默认数据库连接
        /// </summary>
        public static readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        #region SqlCommand
        public static SqlCommand GetInitSqlCommand(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = cmdType;
            if (cmdParameters.Any())
            {
                cmd.Parameters.AddRange(cmdParameters);
            }
            return cmd;
        }

        public static SqlCommand GetInitSqlCommand(SqlConnection conn, string cmdText, params SqlParameter[] cmdParameters)
        {
            SqlCommand cmd = new SqlCommand(cmdText, conn);
            if (cmdParameters.Any())
            {
                cmd.Parameters.AddRange(cmdParameters);
            }
            return cmd;
        }
        #endregion
        #region ExecuteNonQuery
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            int result;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = GetInitSqlCommand(conn, cmdType, cmdText, cmdParameters);
                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        public static int ExecuteNonQuery(string connectionString, string cmdText, params SqlParameter[] cmdParameters)
        {
            int result;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = GetInitSqlCommand(conn, cmdText, cmdParameters);
                result = cmd.ExecuteNonQuery();
            }
            return result;
        }

        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            return ExecuteNonQuery(connectionString, cmdType, cmdText, cmdParameters);
        }

        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] cmdParameters)
        {
            return ExecuteNonQuery(connectionString, cmdText, cmdParameters);
        }
        #endregion
        #region ExecuteScalar
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            object result;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = GetInitSqlCommand(conn, cmdType, cmdText, cmdParameters);
                result = cmd.ExecuteScalar();
            }
            return result;
        }

        public static object ExecuteScalar(string connectionString, string cmdText, params SqlParameter[] cmdParameters)
        {
            object result;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = GetInitSqlCommand(conn, cmdText, cmdParameters);
                result = cmd.ExecuteScalar();
            }
            return result;
        }

        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            return ExecuteScalar(connectionString, cmdType, cmdText, cmdParameters);
        }

        public static object ExecuteScalar(string cmdText, params SqlParameter[] cmdParameters)
        {
            return ExecuteScalar(connectionString, cmdText, cmdParameters);
        }
        #endregion
        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            DataTable result = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(GetInitSqlCommand(conn, cmdType, cmdText, cmdParameters));
                adapter.Fill(result);
            }
            return result;
        }

        public static DataTable ExecuteDataTable(string connectionString, string cmdText, params SqlParameter[] cmdParameters)
        {
            DataTable result = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(GetInitSqlCommand(conn, cmdText, cmdParameters));
                adapter.Fill(result);
            }
            return result;
        }

        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            return ExecuteDataTable(connectionString, cmdType, cmdText, cmdParameters);
        }

        public static DataTable ExecuteDataTable(string cmdText, params SqlParameter[] cmdParameters)
        {
            return ExecuteDataTable(connectionString, cmdText, cmdParameters);
        }
        #endregion
        #region ExecuteDataSet
        public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            DataSet result = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(GetInitSqlCommand(conn, cmdType, cmdText, cmdParameters));
                adapter.Fill(result);
            }
            return result;
        }

        public static DataSet ExecuteDataSet(string connectionString, string cmdText, params SqlParameter[] cmdParameters)
        {
            DataSet result = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(GetInitSqlCommand(conn, cmdText, cmdParameters));
                adapter.Fill(result);
            }
            return result;
        }

        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] cmdParameters)
        {
            return ExecuteDataSet(connectionString, cmdType, cmdText, cmdParameters);
        }

        public static DataSet ExecuteDataSet(string cmdText, params SqlParameter[] cmdParameters)
        {
            return ExecuteDataSet(connectionString, cmdText, cmdParameters);
        }
        #endregion
        #region Insert by SqlBulkCopy
        public static void Insert(string connetionString, DataTable dt)
        {
            using (SqlBulkCopy sbc = new SqlBulkCopy(connetionString))
            {
                sbc.DestinationTableName = dt.TableName;
                sbc.BatchSize = 50000;
                foreach (DataColumn item in dt.Columns)
                {
                    sbc.ColumnMappings.Add(item.ColumnName, item.ColumnName);
                }
                sbc.WriteToServer(dt);
            }
        }

        public static void Insert(DataTable dt)
        {
            Insert(connectionString, dt);
        }
        #endregion
    }
}
