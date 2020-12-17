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

            Assert.AreEqual(92, result.FinishedEdges[0].StartingPoint.X);
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

            for (int j = 0; j < result.FinishedEdges.Count; j++)
            {
                Edge edge = result.FinishedEdges[j];

                Assert.IsTrue(edge.StartingPoint.X >= 0);
                Assert.IsTrue(edge.EndingPoint.X >= 0);
                Assert.IsTrue(edge.StartingPoint.X <= 400);
                Assert.IsTrue(edge.EndingPoint.X <= 400);

                Assert.IsTrue(edge.StartingPoint.Y >= 0);
                Assert.IsTrue(edge.EndingPoint.Y >= 0);
                Assert.IsTrue(edge.StartingPoint.Y <= 400);
                Assert.IsTrue(edge.EndingPoint.Y <= 400);
            }
        }

        [TestMethod]
        public void TestMethodSmallInput()
        {
            Site[] testsites = new Site[] {
                new Site(110, 162),
                new Site(331, 200),
                new Site(146, 266)};


            var pr = new Mapseesharp.Program();
            ResultObject result = pr.Calculate(testsites, 400, 400);

            while (!result.Ready)
            {
                result = pr.Calculate(result);
            }

            Assert.AreEqual(3, result.FinishedEdges.Count);

            Point middle = new Point(220.32068837897856, 182.04283863804591);
            Assert.AreEqual(middle, result.FinishedEdges[0].StartingPoint);
            Assert.AreEqual(middle, result.FinishedEdges[1].StartingPoint);
            Assert.AreEqual(middle, result.FinishedEdges[2].StartingPoint);

            Assert.AreEqual(new Point(0, 258.30769230769226), result.FinishedEdges[0].EndingPoint);
            Assert.AreEqual(new Point(298.07837837837837, 400), result.FinishedEdges[1].EndingPoint);
            Assert.AreEqual(new Point(251.62217194569681, 0), result.FinishedEdges[2].EndingPoint);
        }
    }
}
