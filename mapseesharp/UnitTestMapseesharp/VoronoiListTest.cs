using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mapseesharp;

namespace UnitTestMapseesharp
{
    [TestClass]
    public class VoronoiList1
    {

        [TestMethod]
        public void TestInitial()
        {
            VoronoiList<string> list = new VoronoiList<string>();
            Assert.AreEqual(0, list.Count);
            Assert.AreEqual(20, list.Capacity);
        }

        [TestMethod]
        public void TestAddOneElement()
        {
            VoronoiList<string> list = new VoronoiList<string>();
            list.Add("1");
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(20, list.Capacity);
        }

        [TestMethod]
        public void TestAdd()
        {
            VoronoiList<string> list = new VoronoiList<string>();

            list.Add("1");
            list.Add("2");
            list.Add("3");
            list.Add("4");

            Assert.AreEqual(4, list.Count);

            Assert.AreEqual("1", list[0]);
            Assert.AreEqual("2", list[1]);
            Assert.AreEqual("3", list[2]);
            Assert.AreEqual("4", list[3]);

            Assert.AreEqual(20, list.Capacity);
        }

        [TestMethod]
        public void TestIndexOf()
        {
            VoronoiList<string> list = new VoronoiList<string>();

            list.Add("1");
            list.Add("2");
            list.Add("3");
            list.Add("4");

            Assert.AreEqual(0, list.IndexOf("1"));
            Assert.AreEqual(1, list.IndexOf("2"));
            Assert.AreEqual(2, list.IndexOf("3"));
            Assert.AreEqual(3, list.IndexOf("4"));
            Assert.AreEqual(-1, list.IndexOf("5"));

            Assert.AreEqual(20, list.Capacity);
        }

        [TestMethod]
        public void TestAddRange()
        {
            VoronoiList<string> list = new VoronoiList<string>();

            list.Add("1");
            list.Add("2");
            list.Add("3");
            list.Add("4");

            list.InsertRange(2, new string[] { "5", "6" });

            Assert.AreEqual(6, list.Count);
            Assert.AreEqual(22, list.Capacity);

            Assert.AreEqual("1", list[0]);
            Assert.AreEqual("2", list[1]);
            Assert.AreEqual("5", list[2]);
            Assert.AreEqual("6", list[3]);
            Assert.AreEqual("3", list[4]);
            Assert.AreEqual("4", list[5]);
        }

        [TestMethod]
        public void TestInsert()
        {
            VoronoiList<string> list = new VoronoiList<string>();
            Assert.AreEqual(0, list.Count);

            list.Add("1");
            list.Add("2");
            list.Add("3");
            list.Add("4");

            list.Insert(2, "5");

            Assert.AreEqual("1", list[0]);
            Assert.AreEqual("2", list[1]);
            Assert.AreEqual("5", list[2]);
            Assert.AreEqual("3", list[3]);
            Assert.AreEqual("4", list[4]);

            Assert.AreEqual(20, list.Capacity);
        }
    }
}
