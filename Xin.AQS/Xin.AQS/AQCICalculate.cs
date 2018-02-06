using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 空气质量综合指数计算方法类
    /// </summary>
    public static class AQCICalculate
    {
        /// <summary>
        /// 污染物监测项字典（用于首要污染物）
        /// </summary>
        private static Dictionary<string, string> pollutantDic;
        /// <summary>
        /// AQCIResult中用于计算的监测项属性
        /// </summary>
        private static PropertyInfo[] propertiesForAQCICalculate;
        /// <summary>
        /// AQCIResult类的全部属性
        /// </summary>
        private static PropertyInfo[] propertiesOfAQCIResult;
        /// <summary>
        /// 污染物监测项浓度二级标准字典
        /// </summary>
        private static Dictionary<string, int> limitDic;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static AQCICalculate()
        {
            pollutantDic = new Dictionary<string, string>(){
                {"SO2","二氧化硫"},
                {"NO2","二氧化氮"},
                {"PM10","颗粒物(PM10)"},
                {"CO","一氧化碳"},
                {"O38H","臭氧8小时"},
                {"PM25","细颗粒物(PM2.5)"}
            };
            propertiesOfAQCIResult = typeof(AQCIResult).GetProperties();
            propertiesForAQCICalculate = propertiesOfAQCIResult.Where(o => pollutantDic.ContainsKey(o.Name)).ToArray();
            limitDic = new Dictionary<string, int>(){
                {"SO2",60},
                {"NO2",40},
                {"PM10",70},
                {"CO",4},
                {"O38H",160},
                {"PM25",35}
            };
        }

        /// <summary>
        /// 计算百分位数
        /// </summary>
        /// <param name="values">按从小到大排序的数据，且数组长度需大于1</param>
        /// <param name="p">百分位</param>
        /// <returns>百分位数</returns>
        public static decimal CalculatePercentile(decimal[] values, decimal p)
        {
            decimal k = (values.Length - 1) * p;
            int s = (int)Math.Floor(k);
            decimal percentile = values[s] + (k - s) * (values[s + 1] - values[s]);
            return percentile;
        }

        /// <summary>
        /// 计算空气质量综合指数
        /// </summary>
        /// <param name="data">空气质量综合指数计算结果</param>
        public static void CalculateAQCI(AQCIResult data)
        {
            Dictionary<string, decimal> IAQCIDic = new Dictionary<string, decimal>();
            bool calculate = true;
            foreach (PropertyInfo property in propertiesForAQCICalculate)
            {
                if (property.GetValue(data, null) == null)
                {
                    calculate = false;
                    break;
                }
                else
                {
                    decimal iaqci = Math.Round(Convert.ToDecimal(property.GetValue(data, null)) / limitDic[property.Name], 2);
                    propertiesOfAQCIResult.First(o => o.Name == string.Format("I{0}", property.Name)).SetValue(data, iaqci, null);
                    IAQCIDic.Add(property.Name, iaqci);
                }
            }
            if (calculate)
            {
                data.AQCI = IAQCIDic.Sum(o => o.Value);
                decimal maxIAQCI = IAQCIDic.Max(o => o.Value);
                data.PrimaryPollutant = string.Join(",", pollutantDic.Where(o => IAQCIDic.Where(t => t.Value == maxIAQCI).Select(t => t.Key).Contains(o.Key)).Select(o => o.Value));
            }
        }
    }
}
