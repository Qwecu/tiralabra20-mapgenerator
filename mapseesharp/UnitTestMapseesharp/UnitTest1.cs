using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mapseesharp;

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

            var pr = new Mapseesharp.Program();
            ResultObject result = pr.Calculate(testsites, 400, 400);

            Assert.AreEqual(7, result.Events.Count);

            Assert.AreEqual(172, result.BeachArcs[0].HomeX);
            Assert.AreEqual(360, result.BeachArcs[0].HomeY);


            for (int i = 0; i < 17; i++)
            {
                result = pr.Calculate(result);
            }

            Assert.AreEqual(20, result.FinishedEdges.Count);
            Assert.AreEqual(0, result.Events.Count);
            Assert.AreEqual(10, result.OldCircleEvents.Count);
            Assert.AreEqual(false, result.Ready);

            result = pr.Calculate(result);

            Assert.AreEqual(true, result.Ready);
            Assert.AreEqual(19, result.FinishedEdges.Count);

            Assert.AreEqual(92, result.FinishedEdges[0].StartingPoint.x);
            //Assert.AreEqual(335.62962962963, result.FinishedEdges[0].StartingPoint.y);
        }

        [TestMethod]
        public void TestMethod2()
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

            var pr = new Mapseesharp.Program();
            ResultObject result = pr.Calculate(testsites, 400, 400);


            for (int i = 0; i < 18; i++)
            {
                result = pr.Calculate(result);
            }

            foreach (Edge edge in result.FinishedEdges)
            {
                Assert.IsTrue(edge.StartingPoint.x >= 0);
                Assert.IsTrue(edge.EndingPoint.x >= 0);
                Assert.IsTrue(edge.StartingPoint.x <= 400);
                Assert.IsTrue(edge.EndingPoint.x <= 400);

                Assert.IsTrue(edge.StartingPoint.y >= 0);
                Assert.IsTrue(edge.EndingPoint.y >= 0);
                Assert.IsTrue(edge.StartingPoint.y <= 400);
                Assert.IsTrue(edge.EndingPoint.y <= 400);
            }
        }
    }
}
