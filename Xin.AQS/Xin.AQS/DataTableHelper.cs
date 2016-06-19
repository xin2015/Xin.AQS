using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 数据库表与实体类转换类
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// 将DataTable转换为实体类集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns>实体类集合</returns>
        public static List<T> GetList<T>(this DataTable dt) where T : class, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties().Where(t => dt.Columns.Contains(t.Name)).ToArray();
            int count = dt.Rows.Count;
            T[] array = new T[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = new T();
            }
            foreach (PropertyInfo property in properties)
            {
                System.Type propertyType = property.PropertyType;
                DataColumn dc = dt.Columns[dt.Columns.IndexOf(property.Name)];
                int i = 0;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    System.Type underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
                    if (dc.DataType.Equals(underlyingType))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!Convert.IsDBNull(dr[property.Name]))
                            {
                                property.SetValue(array[i], dr[property.Name], null);
                            }
                            i++;
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!Convert.IsDBNull(dr[property.Name]))
                            {
                                try
                                {
                                    property.SetValue(array[i], Convert.ChangeType(dr[property.Name], underlyingType), null);
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Debug("GetList Convert UnderlyingType Error!", e);
                                }
                            }
                            i++;
                        }
                    }
                }
                else
                {
                    if (dc.DataType.Equals(property.PropertyType))
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!Convert.IsDBNull(dr[property.Name]))
                            {
                                property.SetValue(array[i], dr[property.Name], null);
                            }
                            i++;
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!Convert.IsDBNull(dr[property.Name]))
                            {
                                try
                                {
                                    property.SetValue(array[i], Convert.ChangeType(dr[property.Name], property.PropertyType), null);
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Debug("GetList Convert Error!", e);
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            return array.ToList();
        }

        /// <summary>
        /// 将DataTable转换为实体类集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns>实体类集合</returns>
        public static List<T> GetListO<T>(this DataTable dt) where T : class, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties().Where(t => dt.Columns.Contains(t.Name)).ToArray();
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T data = new T();
                foreach (PropertyInfo property in properties)
                {
                    if (!Convert.IsDBNull(dr[property.Name]))
                    {
                        try
                        {
                            property.SetValue(data, dr[property.Name], null);
                        }
                        catch (ArgumentException)
                        {
                            try
                            {
                                property.SetValue(data, Convert.ChangeType(dr[property.Name], property.PropertyType), null);
                            }
                            catch (InvalidCastException)
                            {
                                try
                                {
                                    property.SetValue(data, Convert.ChangeType(dr[property.Name], Nullable.GetUnderlyingType(property.PropertyType)), null);
                                }
                                catch (Exception e)
                                {
                                    LogHelper.Logger.Debug("GetListO Convert UnderlyingType Error!", e);
                                }
                            }
                            catch (Exception e)
                            {
                                LogHelper.Logger.Debug("GetListO Convert Error!", e);
                            }
                        }
                        catch (Exception e)
                        {
                            LogHelper.Logger.Debug("GetListO SetValue Error!", e);
                        }
                    }
                }
                list.Add(data);
            }
            return list;
        }

        /// <summary>
        /// 将实体类集合转换为DataTable
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="collection">实体类集合</param>
        /// <param name="tableName">表名</param>
        /// <param name="preclusiveColumnNames">排除的列名</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDataTable<T>(this IEnumerable<T> collection, string tableName, params string[] preclusiveColumnNames)
        {
            DataTable dt = new DataTable();
            dt.TableName = tableName;
            PropertyInfo[] properties = typeof(T).GetProperties();
            if (preclusiveColumnNames.Any())
            {
                properties = properties.Where(o => !preclusiveColumnNames.Contains(o.Name)).ToArray();
            }
            foreach (PropertyInfo property in properties)
            {
                System.Type propertyType = property.PropertyType;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    dt.Columns.Add(property.Name, Nullable.GetUnderlyingType(propertyType));
                }
                else
                {
                    dt.Columns.Add(property.Name, propertyType);
                }
            }
            foreach (T data in collection)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyInfo property in properties)
                {
                    dr[property.Name] = property.GetValue(data, null) ?? DBNull.Value;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
