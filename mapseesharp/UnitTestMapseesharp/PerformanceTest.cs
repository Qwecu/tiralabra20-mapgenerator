using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mapseesharp;
using System.Diagnostics;
using System.Collections.Generic;

namespace UnitTestMapseesharp
{
    [TestClass]
    public class PerformanceTest
    {

        [TestMethod]
        public void PerformanceTest1()
        {
            int inputSize = 2;
            double seconds = -1;

            int canvasDimension = 10000;

            while (seconds < 600) {
                var inputSites = InputRandomizer.RandomInput(inputSize, canvasDimension, canvasDimension);

                var prog = new Mapseesharp.Program();

                var t = new Stopwatch();
                t.Start();
                var result = prog.Calculate(inputSites, canvasDimension, canvasDimension);

                while(result.Ready == false)
                {
                    result = prog.Calculate(result);
                }
                t.Stop();

                seconds = t.ElapsedMilliseconds * 1.0 / 1000;

                System.Diagnostics.Debug.WriteLine("Input size "+ inputSize + " time " + seconds + " seconds");

                inputSize = (int)(1.5 * inputSize);
            }


        }
    }
}