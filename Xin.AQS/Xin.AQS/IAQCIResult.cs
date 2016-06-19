using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    public interface IAQCIResult
    {
        decimal? AQCI { get; set; }
        string PrimaryPollutant { get; set; }

        void GetAQCI();
    }
}
