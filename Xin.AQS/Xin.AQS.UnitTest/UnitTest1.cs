using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xin.Basic;

namespace Xin.AQS.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DataSync.SyncNationalCityDayAQIPublishData(new DateTime(2018, 2, 5));
            DataSync.SyncNationalCityDayAQIPublishHistoryData(new DateTime(2018, 2, 5));
            DataSync.SyncNationalCityDayAQIPublishRankData(new DateTime(2018, 2, 5));
            DataSync.SyncNationalCityDayAQCIPublishRankData(new DateTime(2018, 2, 5));
        }

        [TestMethod]
        public void TestLog()
        {
            LogHelper.Logger.Info(string.Format("测试日志，{0}", AppDomain.CurrentDomain.BaseDirectory));
        }
    }
}
