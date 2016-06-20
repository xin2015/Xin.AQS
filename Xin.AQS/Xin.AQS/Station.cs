using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xin.Basic;

namespace Xin.AQS
{
    public class Station
    {
        private static string selectString;
        private static string cacheKey;

        static Station()
        {
            selectString = "select * from Station where Status = 1";
            cacheKey = "Station";
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string UniqueCode { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string DistrictCode { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; }

        public static List<Station> GetList()
        {
            List<Station> list;
            if (MemoryCacheHelper.Contain(cacheKey))
            {
                try
                {
                    list = (List<Station>)MemoryCacheHelper.Get(cacheKey);
                }
                catch (Exception e)
                {
                    LogHelper.Logger.Error("从Cache中获取Station失败！", e);
                    try
                    {
                        list = SqlHelper.ExecuteDataTable(selectString).GetList<Station>();
                        MemoryCacheHelper.Set(cacheKey, list, DateTime.Now.AddHours(2));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error("从数据库中查询Station并更新Cache失败！", ex);
                        list = new List<Station>();
                    }
                }
            }
            else
            {
                try
                {
                    list = SqlHelper.ExecuteDataTable(selectString).GetList<Station>();
                    MemoryCacheHelper.Set(cacheKey, list, DateTime.Now.AddHours(2));
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error("从数据库中查询Station并更新Cache失败！", ex);
                    list = new List<Station>();
                }
            }
            return list;
        }
    }
}
