using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    public interface IAQIResult
    {
        int? AQI { get; set; }
        string PrimaryPollutant { get; set; }
        string Type { get; set; }
        string Effect { get; set; }
        string Measure { get; set; }

        void GetAQI();
    }
}
