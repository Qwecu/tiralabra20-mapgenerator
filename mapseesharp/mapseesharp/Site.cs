﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapseesharp
{
    public class Site
    {
        public double x { get; set; }
        public double y { get; set; }

        public Site(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public override string ToString()
        {
            return "Site " +  x + "; " + y;
        }
    }
}
