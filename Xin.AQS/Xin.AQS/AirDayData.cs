using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 空气日均数据
    /// </summary>
    public class AirDayData : AirDataBase
    {
        /// <summary>
        /// 臭氧（O3）最大8小时滑动平均浓度（μg/m³）
        /// </summary>
        public decimal? O38H { get; set; }
    }
}
