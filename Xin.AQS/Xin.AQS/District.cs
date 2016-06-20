using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xin.Basic;

namespace Xin.AQS
{
    public class District
    {
        private static string selectString;
        private static string cacheKey;

        static District()
        {
            selectString = "select * from District where Status = 1";
            cacheKey = "District";
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool Status { get; set; }

        public static List<District> GetList()
        {
            List<District> list;
            if (MemoryCacheHelper.Contain(cacheKey))
            {
                try
                {
                    list = (List<District>)MemoryCacheHelper.Get(cacheKey);
                }
                catch (Exception e)
                {
                    LogHelper.Logger.Error("从Cache中获取District失败！", e);
                    try
                    {
                        list = SqlHelper.ExecuteDataTable(selectString).GetList<District>();
                        MemoryCacheHelper.Set(cacheKey, list, DateTime.Now.AddHours(2));
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Logger.Error("从数据库中查询District并更新Cache失败！", ex);
                        list = new List<District>();
                    }
                }
            }
            else
            {
                try
                {
                    list = SqlHelper.ExecuteDataTable(selectString).GetList<District>();
                    MemoryCacheHelper.Set(cacheKey, list, DateTime.Now.AddHours(2));
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error("从数据库中查询District并更新Cache失败！", ex);
                    list = new List<District>();
                }
            }
            return list;
        }
    }
}
