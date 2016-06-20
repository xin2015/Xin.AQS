using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xin.AQS.UnitTest
{
    [TestClass]
    public class DataSyncUnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            DateTime date = DateTime.Today;
            DateTime time = date.AddHours(DateTime.Now.Hour);
            DataSync.SyncNationalCityAQIPublishData(time);
            DataSync.SyncNationalCityDayAQIPublishData(date.AddDays(-1));
        }

        [TestMethod]
        public void TestMethodSyncNationalCityDayAQIPublishRankData()
        {
            for (DateTime time = new DateTime(2016, 6, 1); time < DateTime.Today; time = time.AddDays(1))
            {
                DataSync.SyncNationalCityDayAQIPublishHistoryData(time);
            }
        }

        [TestMethod]
        public void TestMethodSyncNationalCityDayAQCIPublishRankData()
        {
            DataSync.SyncNationalCityDayAQCIPublishRankData(DateTime.Today);
        }
    }
}
