﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{
    class BeachHalfEdge : BeachObj
    {
        public double startingX { get; set; }
        public double startingY { get; set; }
        public double directionX { get; set; }
        public double directionY { get; set; }

        public BeachHalfEdge(double startingX, double startingY, double directionX, double directionY)
        {
            this.startingX = startingX;
            this.startingY = startingY;
            this.directionX = directionX;
            this.directionY = directionY;
        }

        public bool PointingLeft { get { return (directionX < startingX); } }
        public bool PointingRight { get { return (directionX > startingX); } }
    }
}