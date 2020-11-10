﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{


    public class ReverseComparer : IComparer<double>
    {
        int IComparer<double>.Compare(double x, double y)
        {
            return -x.CompareTo(y);
        }
    }
    public class Program
    {
        private VoronoiCalculator vCalc;

        public ResultObject Calculate(Site[] sites)
        {
            vCalc = new VoronoiCalculator();
            return vCalc.Iterate(sites);
        }

        public ResultObject Calculate(ResultObject res)
        {
            vCalc = new VoronoiCalculator();
            return vCalc.Iterate(res.Events, res.FinishedEdges, res.Beachline, res.OldCircleEvents);
        }

        static void Main(string[] args)
        {
            
        }
    }
}
