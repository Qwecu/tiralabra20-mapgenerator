using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mapseesharp;

namespace UnitTestMapseesharp
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            Site[] testsites = new Site[] {
                new Site(12,36),
                new Site(172,360),
                new Site(302,196),
                new Site(212,100),
                new Site(92,252),
                new Site(152,76),
                new Site(124,4),
                new Site(248,284)
            };

            var pr = new mapseesharp.Program();
            var result = pr.Calculate(testsites, 400, 400);

            Assert.AreEqual(172, result.BeachArcs[0].HomeX);


            for(int i = 0; i < 17; i++)
            {
                result = pr.Calculate(result);
            }

            Assert.AreEqual(1, 1);
        }
    }
}
