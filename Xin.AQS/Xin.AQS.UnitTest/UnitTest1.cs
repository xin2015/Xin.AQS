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
    }
}
