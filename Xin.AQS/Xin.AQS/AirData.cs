using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    public class AirData
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public decimal? SO2 { get; set; }
        public decimal? NO2 { get; set; }
        public decimal? PM10 { get; set; }
        public decimal? CO { get; set; }
        public decimal? O3 { get; set; }
        public decimal? O38H { get; set; }
        public decimal? PM25 { get; set; }
    }
}
