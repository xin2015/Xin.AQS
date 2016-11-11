using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xin.AQS.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DataSync.SyncDistrictDayAQIPublishData(new DateTime(2015, 5, 10));
        }
        
        [TestMethod]
        public void TestMethod2()
        {
            DataSync.SyncDistrictHourAQIPublishData(DateTime.Today.AddHours(DateTime.Now.Hour-1));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var result = DataQuery.GetDistrictHourAQIPublishData("441402", DateTime.Now.AddDays(-1), DateTime.Now);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var result = DataQuery.GetDistrictDayAQIPublishData("441402", DateTime.Today.AddDays(-30), DateTime.Today);
        }
    }
}
