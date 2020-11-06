using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{
    class Point
    {

        public double x { get; set; }
        public double y { get; set; }

        public Point(BeachHalfEdge intersectingLineFromLeft, BeachHalfEdge intersectingLineFromRight) {
            //kulmakerroin
            double k1 = (intersectingLineFromLeft.directionY - intersectingLineFromLeft.startingY) / (intersectingLineFromLeft.directionX - intersectingLineFromLeft.startingX);
            double k2 = (intersectingLineFromRight.startingY - intersectingLineFromRight.directionY) / (intersectingLineFromRight.startingX - intersectingLineFromRight.directionX);
            //vakiotermi
            double b1 = (intersectingLineFromLeft.startingY - k1 * intersectingLineFromLeft.startingX);
            double b2 = (intersectingLineFromRight.startingY - k1 * intersectingLineFromRight.startingX);

            // x @ leikkaus
            x = (b2 - b1) / (k1 - k2);
            //y @ leikkaus
            y = k1 * x + b1;
        }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
