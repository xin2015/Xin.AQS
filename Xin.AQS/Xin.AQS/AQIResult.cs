using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// AQI计算结果
    /// </summary>
    public abstract class AQIResult: IAQIResult
    {
        /// <summary>
        /// 空气质量指数
        /// </summary>
        public int? AQI { get; set; }
        /// <summary>
        /// 首要污染物
        /// </summary>
        public string PrimaryPollutant { get; set; }
        /// <summary>
        /// 空气质量指数级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 空气质量指数类别
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 空气质量指数类别颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 对健康影响情况
        /// </summary>
        public string Effect { get; set; }
        /// <summary>
        /// 建议采取的措施
        /// </summary>
        public string Measure { get; set; }
        /// <summary>
        /// 超标污染物
        /// </summary>
        public string NonAttainmentPollutant { get; set; }

        /// <summary>
        /// 构造函数，赋默认值
        /// </summary>
        public AQIResult()
        {
            PrimaryPollutant = ConfigHelper.EmptyValueString;
            Level = ConfigHelper.EmptyValueString;
            Type = ConfigHelper.EmptyValueString;
            Color = ConfigHelper.EmptyValueString;
            Effect = ConfigHelper.EmptyValueString;
            Measure = ConfigHelper.EmptyValueString;
            NonAttainmentPollutant = ConfigHelper.EmptyValueString;
        }

        /// <summary>
        /// 计算AQI
        /// </summary>
        public abstract void GetAQI();
    }

    /// <summary>
    /// 空气质量指数级别
    /// </summary>
    public enum Level
    {
        /// <summary>
        /// 优
        /// </summary>
        一级,
        /// <summary>
        /// 良
        /// </summary>
        二级,
        /// <summary>
        /// 轻度污染
        /// </summary>
        三级,
        /// <summary>
        /// 中度污染
        /// </summary>
        四级,
        /// <summary>
        /// 重度污染
        /// </summary>
        五级,
        /// <summary>
        /// 严重污染
        /// </summary>
        六级
    }

    /// <summary>
    /// 空气质量指数类别
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// 一级
        /// </summary>
        优,
        /// <summary>
        /// 二级
        /// </summary>
        良,
        /// <summary>
        /// 三级
        /// </summary>
        轻度污染,
        /// <summary>
        /// 四级
        /// </summary>
        中度污染,
        /// <summary>
        /// 五级
        /// </summary>
        重度污染,
        /// <summary>
        /// 六级
        /// </summary>
        严重污染
    }

    /// <summary>
    /// 空气质量指数类别颜色
    /// </summary>
    public enum Color
    {
        /// <summary>
        /// 0,228,0   #00E400
        /// </summary>
        绿色,
        /// <summary>
        /// 255,255,0   #FFFF00
        /// </summary>
        黄色,
        /// <summary>
        /// 255,126,0   #FF7E00
        /// </summary>
        橙色,
        /// <summary>
        /// 255,0,0   #FF0000
        /// </summary>
        红色,
        /// <summary>
        /// 153,0,76   #99004C
        /// </summary>
        紫色,
        /// <summary>
        /// 126,0,35   #7E0023
        /// </summary>
        褐红色
    }
}
