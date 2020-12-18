using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapseesharp
{
    public struct Point
    {

        /// <summary>
        /// Gets or sets the x coordinate.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        public double Y { get; set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// Returns the intersection point between two lines.
        /// </summary>
        /// <param name="intersectingLineFromLeft">Line 1.</param>
        /// <param name="intersectingLineFromRight">Line 2.</param>
        public Point(BeachHalfEdge intersectingLineFromLeft, BeachHalfEdge intersectingLineFromRight)
        {
            if (intersectingLineFromLeft.IsVertical && intersectingLineFromRight.IsVertical)
            {
                throw new Exception("Two horizontal lines don't have an intersection point");
            }
            else if (intersectingLineFromLeft.IsVertical || intersectingLineFromRight.IsVertical)
            {
                BeachHalfEdge vertical = intersectingLineFromLeft.IsVertical ? intersectingLineFromLeft : intersectingLineFromRight;
                BeachHalfEdge other = intersectingLineFromLeft.IsVertical ? intersectingLineFromRight : intersectingLineFromLeft;

                this.X = vertical.StartingX;

                double k = (other.DirectionY - other.StartingY) / (other.DirectionX - other.StartingX);
                double b = ((other.StartingY * other.DirectionX) - (other.StartingX * other.DirectionY)) / (other.DirectionX - other.StartingX);

                this.Y = (k * this.X) + b;
            }
            else
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
                this.X = (b2 - b1) / (k1 - k2);
                // y @ leikkaus
                this.Y = (k1 * this.X) + b1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        // Does not work if edge is vertical (halfedge can be)
        public Point(Edge edge, BeachHalfEdge halfedge)
            : this()
        {
            if (halfedge.StartingX == halfedge.DirectionX)
            {
                // kulmakerroin
                double k1 = (edge.EndingPoint.Y - edge.StartingPoint.Y) / (edge.EndingPoint.X - edge.StartingPoint.X);

                // vakiotermi
                double b1 = ((edge.StartingPoint.Y * edge.EndingPoint.X) - (edge.StartingPoint.X * edge.EndingPoint.Y))
                    / (edge.EndingPoint.X - edge.StartingPoint.X);

                // vakio y = n
                double n = halfedge.StartingX;

                // x @ leikkaus
                this.X = n;

                // y @ leikkaus
                this.Y = (k1 * this.X) + b1;
            }
            else
            {
                // kulmakerroin
                double k1 = (edge.EndingPoint.Y - edge.StartingPoint.Y) / (edge.EndingPoint.X - edge.StartingPoint.X);
                double k2 = (halfedge.StartingY - halfedge.DirectionY) / (halfedge.StartingX - halfedge.DirectionX);

                // vakiotermi
                double b1 = ((edge.StartingPoint.Y * edge.EndingPoint.X) - (edge.StartingPoint.X * edge.EndingPoint.Y))
                    / (edge.EndingPoint.X - edge.StartingPoint.X);
                double b2 = ((halfedge.StartingY * halfedge.DirectionX) - (halfedge.StartingX * halfedge.DirectionY))
        / (halfedge.DirectionX - halfedge.StartingX);

                // x @ leikkaus
                this.X = (b2 - b1) / (k1 - k2);

                // y @ leikkaus
                this.Y = (k1 * this.X) + b1;
            }
        }

        /// <summary>
        /// Returns true if the point is inside the given coordinates.
        /// </summary>
        /// <param name="width">Width of canvas.</param>
        /// <param name="height">Height of canvas.</param>
        /// <returns>True if the point is on the canvas.</returns>
        internal bool OnMap(int width, int height)
        {
            return this.X > 0 && this.X < width && this.Y > 0 && this.Y < height;
        }

        /// <summary>
        /// Returns a new point equidistant to the two given points.
        /// </summary>
        /// <param name="first">The first point.</param>
        /// <param name="second">The secont point.</param>
        /// <returns>The midway point.</returns>
        internal static Point GetPointAtMidway(Point first, Point second)
        {
            return new Point((first.X + second.X) / 2, (first.Y + second.Y) / 2);
        }

        /// <summary>
        /// Returns the distance between two points.
        /// </summary>
        /// <param name="midpoint">Point 1.</param>
        /// <param name="directionPoint">Point 2.</param>
        /// <returns>Distance between the points.</returns>
        internal static double DistanceBetweenPoints(Point midpoint, Point directionPoint)
        {
            return Math.Sqrt(Math.Pow(midpoint.X - directionPoint.X, 2) + Math.Pow(midpoint.Y - directionPoint.Y, 2));
        }
    }
}
