using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    public class DataHelper
    {
        public static void UpdateRankByAQI(List<AirDayAQIRankData> list)
        {
            int currentAQI = 0, currentRank = 1, order = 1;
            List<AirDayAQIRankData> listWithAQI = list.Where(o => o.AQI.HasValue).OrderBy(o => o.AQI.Value).ToList();
            listWithAQI.ForEach(o =>
            {
                if (o.AQI.Value > currentAQI)
                {
                    currentAQI = o.AQI.Value;
                    o.Rank = currentRank = order;
                }
                else
                {
                    o.Rank = currentRank;
                }
                order++;
            });
            list.Where(o => !o.AQI.HasValue).ToList().ForEach(o => o.Rank = order);
        }

        public static void UpdateRankByAQCI(List<AirDayAQCIRankData> list)
        {
            decimal currentAQCI = 0;
            int currentRank = 1, order = 1;
            List<AirDayAQCIRankData> listWithAQI = list.Where(o => o.AQCI.HasValue).OrderBy(o => o.AQCI.Value).ToList();
            listWithAQI.ForEach(o =>
            {
                if (o.AQCI.Value > currentAQCI)
                {
                    currentAQCI = o.AQCI.Value;
                    o.Rank = currentRank = order;
                }
                else
                {
                    o.Rank = currentRank;
                }
                order++;
            });
            list.Where(o => !o.AQCI.HasValue).ToList().ForEach(o => o.Rank = order);
        }
    }
}
