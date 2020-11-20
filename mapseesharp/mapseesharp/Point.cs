using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{
    public struct Point
    {

        public double x { get; set; }
        public double y { get; set; }

        //Does not work with vertical lines
        public Point(BeachHalfEdge intersectingLineFromLeft, BeachHalfEdge intersectingLineFromRight)
        {
            //kulmakerroin
            double k1 = (intersectingLineFromLeft.directionY - intersectingLineFromLeft.startingY) / (intersectingLineFromLeft.directionX - intersectingLineFromLeft.startingX);
            double k2 = (intersectingLineFromRight.startingY - intersectingLineFromRight.directionY) / (intersectingLineFromRight.startingX - intersectingLineFromRight.directionX);
            //vakiotermi
            double b1 = ((intersectingLineFromLeft.startingY * intersectingLineFromLeft.directionX) - (intersectingLineFromLeft.startingX * intersectingLineFromLeft.directionY))
                / (intersectingLineFromLeft.directionX - intersectingLineFromLeft.startingX);
            double b2 = ((intersectingLineFromRight.startingY * intersectingLineFromRight.directionX) - (intersectingLineFromRight.startingX * intersectingLineFromRight.directionY))
    / (intersectingLineFromRight.directionX - intersectingLineFromRight.startingX);

            //double b1 = (intersectingLineFromLeft.startingY - k1 * intersectingLineFromLeft.startingX);
            //double b2 = (intersectingLineFromRight.startingY - k1 * intersectingLineFromRight.startingX);

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

        //Does not work if edge is vertical (halfedge can be)
        public Point(Edge edge, BeachHalfEdge halfedge) : this()
        {
            if (halfedge.startingX == halfedge.directionX)
            {
                //kulmakerroin
                double k1 = (edge.EndingPoint.y - edge.StartingPoint.y) / (edge.EndingPoint.x - edge.StartingPoint.x);
                //vakiotermi
                double b1 = ((edge.StartingPoint.y * edge.EndingPoint.x) - (edge.StartingPoint.x * edge.EndingPoint.y))
                    / (edge.EndingPoint.x - edge.StartingPoint.x);

                //vakio y = n
                double n = halfedge.startingX;

                // x @ leikkaus
                x = n;
                //y @ leikkaus
                y = k1 * x + b1;
            }
            else
            {
                //kulmakerroin
                double k1 = (edge.EndingPoint.y - edge.StartingPoint.y) / (edge.EndingPoint.x - edge.StartingPoint.x);
                double k2 = (halfedge.startingY - halfedge.directionY) / (halfedge.startingX - halfedge.directionX);
                //vakiotermi
                double b1 = ((edge.StartingPoint.y * edge.EndingPoint.x) - (edge.StartingPoint.x * edge.EndingPoint.y))
                    / (edge.EndingPoint.x - edge.StartingPoint.x);
                double b2 = ((halfedge.startingY * halfedge.directionX) - (halfedge.startingX * halfedge.directionY))
        / (halfedge.directionX - halfedge.startingX);


                // x @ leikkaus
                x = (b2 - b1) / (k1 - k2);
                //y @ leikkaus
                y = k1 * x + b1;
            }
        }
    }
}
