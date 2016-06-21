using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xin.AQS
{
    public class DataHelper
    {
        public static void UpdateRank(List<AirDayAQIRankData> list)
        {
            int currentRank = 1, currentNewRank = 1, order = 1;
            list.ForEach(o =>
            {
                if (o.Rank > currentRank)
                {
                    currentRank = o.Rank;
                    o.Rank = currentNewRank = order;
                }
                else
                {
                    o.Rank = currentNewRank;
                }
                order++;
            });
        }

        public static void UpdateRank(List<AirDayAQCIRankData> list)
        {
            int currentRank = 1, currentNewRank = 1, order = 1;
            list.ForEach(o =>
            {
                if (o.Rank > currentRank)
                {
                    currentRank = o.Rank;
                    o.Rank = currentNewRank = order;
                }
                else
                {
                    o.Rank = currentNewRank;
                }
                order++;
            });
        }

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

        public static List<AirDayAQCIRankData> GetAirDayAQCIRankData(List<AirDayData> srcList, DateTime time)
        {
            List<AirDayAQCIRankData> list = new List<AirDayAQCIRankData>();
            var groups = srcList.GroupBy(o => o.Code);
            foreach (var group in groups)
            {
                AirDayAQCIRankData data = new AirDayAQCIRankData();
                data.Code = group.Key;
                data.Name = group.First().Name;
                data.Time = time;
                data.SO2 = DataHandle.Round(group.Average(o => o.SO2));
                data.NO2 = DataHandle.Round(group.Average(o => o.NO2));
                data.PM10 = DataHandle.Round(group.Average(o => o.PM10));
                data.PM25 = DataHandle.Round(group.Average(o => o.PM25));
                IEnumerable<AirDayData> temp = group.Where(o => o.CO.HasValue).OrderBy(o => o.CO.Value);
                if (temp.Count() > 1)
                {
                    data.CO = Math.Round(AQCICalculate.CalculatePercentile(temp.Select(o => o.CO.Value).ToArray(), 0.95M), 1);
                }
                temp = group.Where(o => o.O38H.HasValue).OrderBy(o => o.O38H.Value);
                if (temp.Count() > 1)
                {
                    data.O38H = Math.Round(AQCICalculate.CalculatePercentile(temp.Select(o => o.O38H.Value).ToArray(), 0.90M));
                }
                data.GetAQCI();
                list.Add(data);
            }
            DataHelper.UpdateRankByAQCI(list);
            return list;
        }
    }
}
