using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// AQI计算方法类
    /// </summary>
    public static class AQICalculate
    {
        #region 字段参数
        /// <summary>
        /// 首要污染物限值
        /// </summary>
        private static int primaryPollutantLimit = 50;
        /// <summary>
        /// 超标污染物限值
        /// </summary>
        private static int nonAttainmentPollutantLimit = 100;
        /// <summary>
        /// AQI等级计算单元
        /// </summary>
        private static int AQILevelUnit = 50;
        /// <summary>
        /// 对健康影响情况数组
        /// </summary>
        private static string[] effects;
        /// <summary>
        /// 建议采取的措施数组
        /// </summary>
        private static string[] measures;
        /// <summary>
        /// 污染物监测项CodeName字典
        /// </summary>
        private static Dictionary<string, string> pollutantCodeNameDic;
        /// <summary>
        /// 污染物监测项日均浓度限值
        /// </summary>
        private static Dictionary<string, int[]> dayPollutantLimitsDic;
        /// <summary>
        /// 污染物监测项小时浓度限值
        /// </summary>
        private static Dictionary<string, int[]> hourPollutantLimitsDic;
        /// <summary>
        /// 空气质量分指数限值数组
        /// </summary>
        private static int[] IAQILimits = { 0, 50, 100, 150, 200, 300, 400, 500 };
        /// <summary>
        /// 空气质量指数日报属性
        /// </summary>
        private static PropertyInfo[] dayAQIReusltProperties;
        /// <summary>
        /// 空气质量指数实时报属性
        /// </summary>
        private static PropertyInfo[] hourAQIReusltProperties;
        #endregion

        /// <summary>
        /// 静态构造函数：字段参数初始化
        /// </summary>
        static AQICalculate()
        {
            effects = new string[] { "空气质量令人满意，基本无空气污染", "空气质量可接受，但某些污染物可能对极少数异常敏感人群健康有较弱影响", "易感人群症状有轻度加剧，健康人群出现刺激症状", "进一步加剧易感人群症状，可能对健康人群心脏、呼吸系统有影响", "心脏病和肺病患者症状显著加剧，运动耐受力降低，健康人群普遍出现症状", "健康人群运动耐受力降低，有明显强烈症状，提前出现某些疾病" };
            measures = new string[] { "各类人群可正常活动", "极少数异常敏感人群应减少户外活动", "儿童、老年人及心脏病、呼吸系统疾病患者应减少长时间、高强度的户外锻炼", "儿童、老年人及心脏病、呼吸系统疾病患者避免长时间、高强度的户外锻炼，一般人群适量减少户外运动", "儿童、老年人和心脏病、肺病患者应停留在室内，停止户外运动，一般人群减少户外运动", "儿童、老年人和病人应当留在室内，避免体力消耗，一般人群应避免户外活动" };
            pollutantCodeNameDic = new Dictionary<string, string>(){
                {"SO2","二氧化硫"},
                {"NO2","二氧化氮"},
                {"PM10","颗粒物"},
                {"CO","一氧化碳"},
                {"O3","臭氧1小时"},
                {"O38H","臭氧8小时"},
                {"PM25","细颗粒物"}
            };
            dayPollutantLimitsDic = new Dictionary<string, int[]>(){
                {"SO2",new int[]{ 0, 50, 150, 475, 800, 1600, 2100, 2620 }},// 二氧化硫（SO2）24小时平均浓度限值
                {"NO2",new int[]{ 0, 40, 80, 180, 280, 565, 750, 940 }},// 二氧化氮（NO2）24小时平均浓度限值
                {"PM10",new int[]{ 0, 50, 150, 250, 350, 420, 500, 600 }},// 可吸入颗粒物（PM10）24小时平均浓度限值
                {"CO",new int[]{ 0, 2, 4, 14, 24, 36, 48, 60 }},// 一氧化碳（CO）24小时平均浓度限值
                {"O38H",new int[]{ 0, 100, 160, 215, 265, 800 }},// 臭氧（O3）8小时滑动平均浓度限值
                {"PM25",new int[]{ 0, 35, 75, 115, 150, 250, 350, 500 }}// 细颗粒物（PM2.5）24小时平均浓度限值
            };
            hourPollutantLimitsDic = new Dictionary<string, int[]>(){
                {"SO2",new int[]{ 0, 150, 500, 650, 800 }},// 二氧化硫（SO2）1小时平均浓度限值
                {"NO2",new int[]{ 0, 100, 200, 700, 1200, 2340, 3090, 3840 }},// 二氧化氮（NO2）1小时平均浓度限值
                {"PM10",new int[]{ 0, 50, 150, 250, 350, 420, 500, 600 }},// 可吸入颗粒物（PM10）24小时平均浓度限值
                {"CO",new int[]{ 0, 5, 10, 35, 60, 90, 120, 150 }},// 一氧化碳（CO）1小时平均浓度限值
                {"O3",new int[]{ 0, 160, 200, 300, 400, 800, 1000, 1200 }},// 臭氧（O3）1小时平均浓度限值
                {"PM25",new int[]{ 0, 35, 75, 115, 150, 250, 350, 500 }}// 细颗粒物（PM2.5）24小时平均浓度限值
            };
            dayAQIReusltProperties = typeof(DayAQIResult).GetProperties();
            hourAQIReusltProperties = typeof(HourAQIResult).GetProperties();
        }

        /// <summary>
        /// 计算空气质量指数日报
        /// </summary>
        /// <param name="data">空气质量指数日报</param>
        public static void CalculateDayAQI(DayAQIResult data)
        {
            #region 计算IAQI
            Dictionary<string, int> IAQIDic = new Dictionary<string, int>();
            foreach (string pollutant in dayPollutantLimitsDic.Keys)
            {
                PropertyInfo property = dayAQIReusltProperties.First(o => o.Name == pollutant);
                if (property.GetValue(data, null) != null)
                {
                    int? iaqi = CalculateIAQI(dayPollutantLimitsDic[pollutant], (decimal)property.GetValue(data, null));
                    if (iaqi.HasValue)
                    {
                        dayAQIReusltProperties.First(o => o.Name == string.Format("I{0}", pollutant)).SetValue(data, iaqi, null);
                        IAQIDic.Add(pollutant, iaqi.Value);
                    }
                }
            }
            #endregion
            CalculateAQIResult(IAQIDic, data);
        }

        /// <summary>
        /// 计算空气质量指数实时报
        /// </summary>
        /// <param name="data">空气质量指数实时报</param>
        public static void CalculateHourAQI(HourAQIResult data)
        {
            #region 计算IAQI
            Dictionary<string, int> IAQIDic = new Dictionary<string, int>();
            foreach (string pollutant in hourPollutantLimitsDic.Keys)
            {
                PropertyInfo property = hourAQIReusltProperties.First(o => o.Name == pollutant);
                if (property.GetValue(data, null) != null)
                {
                    int? iaqi = CalculateIAQI(hourPollutantLimitsDic[pollutant], (decimal)property.GetValue(data, null));
                    if (iaqi.HasValue)
                    {
                        hourAQIReusltProperties.First(o => o.Name == string.Format("I{0}", pollutant)).SetValue(data, iaqi, null);
                        IAQIDic.Add(pollutant, iaqi.Value);
                    }
                }
            }
            #endregion
            CalculateAQIResult(IAQIDic, data);
        }

        /// <summary>
        /// 根据IAQI计算AQI、PrimaryPollutantList和NonAttainmentPollutantList
        /// </summary>
        /// <param name="IAQIDic">IAQIDic</param>
        /// <param name="data">AQIResult</param>
        public static void CalculateAQIResult(Dictionary<string, int> IAQIDic, AQIResult data)
        {
            if (IAQIDic.Any())
            {
                data.AQI = IAQIDic.Max(t => t.Value);
                data.PrimaryPollutant = string.Join(",", pollutantCodeNameDic.Where(o => IAQIDic.Where(t => t.Value == data.AQI && t.Value > primaryPollutantLimit).Select(t => t.Key).Contains(o.Key)).Select(o => o.Value));
                if (string.IsNullOrEmpty(data.PrimaryPollutant))
                {
                    data.PrimaryPollutant = ConfigHelper.EmptyValueString;
                    data.NonAttainmentPollutant = ConfigHelper.EmptyValueString;
                }
                else
                {
                    data.NonAttainmentPollutant = string.Join(",", pollutantCodeNameDic.Where(o => IAQIDic.Where(t => t.Value > nonAttainmentPollutantLimit).Select(t => t.Key).Contains(o.Key)).Select(o => o.Value));
                    if (string.IsNullOrEmpty(data.NonAttainmentPollutant))
                    {
                        data.NonAttainmentPollutant = ConfigHelper.EmptyValueString;
                    }
                }
            }
            CalculateAQIAbout(data);
        }

        /// <summary>
        /// 计算空气质量分指数IAQI
        /// </summary>
        /// <param name="concentrationLimits">浓度值限值</param>
        /// <param name="value">浓度值</param>
        /// <returns></returns>
        private static int? CalculateIAQI(int[] concentrationLimits, decimal value)
        {
            int? result = null;
            if (value >= 0 && value <= concentrationLimits.Last())
            {
                for (int i = 1; i < concentrationLimits.Length; i++)
                {
                    if (value <= concentrationLimits[i])
                    {
                        result = (int)Math.Ceiling((IAQILimits[i] - IAQILimits[i - 1]) * (value - concentrationLimits[i - 1]) / (concentrationLimits[i] - concentrationLimits[i - 1])) + IAQILimits[i - 1];
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 计算污染物监测项的实时IAQI
        /// </summary>
        /// <param name="pollutant">监测项名称，参照HourAQIResult</param>
        /// <param name="value">浓度值</param>
        /// <returns></returns>
        public static int? CalculateHourIAQI(string pollutant, decimal? value)
        {
            int? result;
            if (value.HasValue)
            {
                result = CalculateIAQI(hourPollutantLimitsDic[pollutant], value.Value);
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// 计算污染物监测项的日均IAQI
        /// </summary>
        /// <param name="pollutant">监测项名称，参照DayAQIResult</param>
        /// <param name="value">浓度值</param>
        /// <returns></returns>
        public static int? CalculateDayIAQI(string pollutant, decimal? value)
        {
            int? result;
            if (value.HasValue)
            {
                result = CalculateIAQI(dayPollutantLimitsDic[pollutant], value.Value);
            }
            else
            {
                result = null;
            }
            return result;
        }

        /// <summary>
        /// 根据AQI获取AQI等级、颜色、类别等信息
        /// </summary>
        /// <param name="data">AQIResult</param>
        public static void CalculateAQIAbout(AQIResult data)
        {
            if (data.AQI.HasValue && data.AQI.Value >= 0)
            {
                int level;
                switch ((data.AQI.Value - 1) / AQILevelUnit)
                {
                    case 0: level = 0; break;
                    case 1: level = 1; break;
                    case 2: level = 2; break;
                    case 3: level = 3; break;
                    case 4:
                    case 5: level = 4; break;
                    default: level = 5; break;
                }
                data.Color = Enum.GetName(typeof(Color), level);
                data.Type = Enum.GetName(typeof(Type), level);
                data.Level = Enum.GetName(typeof(Level), level);
                data.Effect = effects[level];
                data.Measure = measures[level];
            }
        }
    }
}
