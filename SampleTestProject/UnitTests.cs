using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleTestProject
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void TestMethodSuccess()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestMethodFailure()
        {
            Assert.IsTrue(false);
        }
    }
}
