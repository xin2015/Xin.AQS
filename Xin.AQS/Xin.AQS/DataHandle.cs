using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 数据处理工具类
    /// </summary>
    public static class DataHandle
    {
        /// <summary>
        /// 将小数值舍入到最接近的整数值
        /// </summary>
        /// <param name="d">要舍入的小数</param>
        /// <returns></returns>
        public static decimal? Round(decimal? d)
        {
            decimal? result;
            if (d.HasValue)
            {
                result = Math.Round(d.Value);
            }
            else
            {
                result = d;
            }
            return result;
        }

        /// <summary>
        /// 将小数值按指定的小数位数舍入
        /// </summary>
        /// <param name="d">要舍入的小数</param>
        /// <param name="decimals">返回值中的小数位数</param>
        /// <returns></returns>
        public static decimal? Round(decimal? d, int decimals)
        {
            decimal? result;
            if (d.HasValue)
            {
                result = Math.Round(d.Value, decimals);
            }
            else
            {
                result = d;
            }
            return result;
        }
    }
}
