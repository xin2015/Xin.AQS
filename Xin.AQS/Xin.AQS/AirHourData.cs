using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    /// <summary>
    /// 空气小时数据
    /// </summary>
    public class AirHourData : AirDataBase
    {
        /// <summary>
        /// 臭氧（O3）1小时平均浓度（μg/m³）
        /// </summary>
        public decimal? O3 { get; set; }
    }
}
