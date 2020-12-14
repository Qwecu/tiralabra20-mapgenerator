using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapseesharp
{
    public struct Point
    {

        public double x { get; set; }
        public double y { get; set; }

        //D oes not work with vertical lines
        public Point(BeachHalfEdge intersectingLineFromLeft, BeachHalfEdge intersectingLineFromRight)
        {
            // kulmakerroin
            double k1 = (intersectingLineFromLeft.DirectionY - intersectingLineFromLeft.StartingY) / (intersectingLineFromLeft.DirectionX - intersectingLineFromLeft.StartingX);
            double k2 = (intersectingLineFromRight.StartingY - intersectingLineFromRight.DirectionY) / (intersectingLineFromRight.StartingX - intersectingLineFromRight.DirectionX);
            // vakiotermi
            double b1 = ((intersectingLineFromLeft.StartingY * intersectingLineFromLeft.DirectionX) - (intersectingLineFromLeft.StartingX * intersectingLineFromLeft.DirectionY))
                / (intersectingLineFromLeft.DirectionX - intersectingLineFromLeft.StartingX);
            double b2 = ((intersectingLineFromRight.StartingY * intersectingLineFromRight.DirectionX) - (intersectingLineFromRight.StartingX * intersectingLineFromRight.DirectionY))
    / (intersectingLineFromRight.DirectionX - intersectingLineFromRight.StartingX);


            // x @ leikkaus
            x = (b2 - b1) / (k1 - k2);
            // y @ leikkaus
            y = (k1 * x) + b1;
        }

        internal bool OnMap(int width, int height)
        {
            return x > 0 && x < width && y > 0 && y < height;
        }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // Does not work if edge is vertical (halfedge can be)
        public Point(Edge edge, BeachHalfEdge halfedge)
            : this()
        {
            if (halfedge.StartingX == halfedge.DirectionX)
            {
                // kulmakerroin
                double k1 = (edge.EndingPoint.y - edge.StartingPoint.y) / (edge.EndingPoint.x - edge.StartingPoint.x);

                // vakiotermi
                double b1 = ((edge.StartingPoint.y * edge.EndingPoint.x) - (edge.StartingPoint.x * edge.EndingPoint.y))
                    / (edge.EndingPoint.x - edge.StartingPoint.x);

                // vakio y = n
                double n = halfedge.StartingX;

                // x @ leikkaus
                x = n;

                // y @ leikkaus
                y = (k1 * x) + b1;
            }
            else
            {
                // kulmakerroin
                double k1 = (edge.EndingPoint.y - edge.StartingPoint.y) / (edge.EndingPoint.x - edge.StartingPoint.x);
                double k2 = (halfedge.StartingY - halfedge.DirectionY) / (halfedge.StartingX - halfedge.DirectionX);

                // vakiotermi
                double b1 = ((edge.StartingPoint.y * edge.EndingPoint.x) - (edge.StartingPoint.x * edge.EndingPoint.y))
                    / (edge.EndingPoint.x - edge.StartingPoint.x);
                double b2 = ((halfedge.StartingY * halfedge.DirectionX) - (halfedge.StartingX * halfedge.DirectionY))
        / (halfedge.DirectionX - halfedge.StartingX);

                // x @ leikkaus
                x = (b2 - b1) / (k1 - k2);

                // y @ leikkaus
                y = (k1 * x) + b1;
            }
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            /*if(obj is Point)
            {
                Point p = (Point)obj;
                bool samex = this.x == p.x;
                bool samey = this.y == p.y;
                return samex && samey;
            }*/

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a new point equidistant to the two given points.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The secont point.</param>
        /// <returns>The midway point.</returns>
        internal static Point GetPointAtMidway(Point first, Point second)
        {
            return new Point((first.x + second.x) / 2, (first.y + second.y) / 2);
        }

        internal static double DistanceBetweenPoints(Point midpoint, Point directionPoint)
        {
            return Math.Sqrt(Math.Pow(midpoint.x - directionPoint.x, 2) + Math.Pow(midpoint.y - directionPoint.y, 2));
        }
    }
}
