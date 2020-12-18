using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapseesharp
{
    public class BeachHalfEdge : BeachObj
    {
        public double StartingX { get; set; }

        public double StartingY { get; set; }

        public double DirectionX { get; set; }

        public double DirectionY { get; set; }

        public Point StartingPoint => new Point(StartingX, StartingY);

        public Point DirectionPoint => new Point(DirectionX, DirectionY);

        public double deltaX => DirectionX - StartingX;

        public double deltaY => DirectionY - StartingY;

        public string Name { get; set; }

        public BeachHalfEdge(double startingX, double startingY, double directionX, double directionY)
        {
            this.StartingX = startingX;
            this.StartingY = startingY;
            this.DirectionX = directionX;
            this.DirectionY = directionY;
        }

        public override string ToString()
        {
            return Name + " HalfEdge (" + this.StartingX + ", " + this.StartingY + ") (" + this.DirectionX + ", " + this.DirectionY + ")";
        }

        public bool PointingLeft { get { return this.DirectionX < this.StartingX; } }

        public bool PointingRight { get { return this.DirectionX > this.StartingX; } }

        public bool PointingUp { get { return this.DirectionY > this.StartingY; } }

        public bool PointingDown { get { return this.DirectionY < this.StartingY; } }

        /// <summary>
        /// Returns a new half edge with the same length and starting point as input, pointing to opposite direction.
        /// </summary>
        /// <param name="singleHalfEdge">Input.</param>
        /// <returns>Mirrored half edge.</returns>
        internal static BeachHalfEdge MirrorKeepStartingPoint(BeachHalfEdge singleHalfEdge)
        {
            return new BeachHalfEdge(
                            singleHalfEdge.StartingX,
                            singleHalfEdge.StartingY,
                            singleHalfEdge.StartingX - (singleHalfEdge.DirectionX - singleHalfEdge.StartingX),
                            singleHalfEdge.StartingY - (singleHalfEdge.DirectionY - singleHalfEdge.StartingY));
        }

        /// <summary>
        /// Returns a new vector with the same ending point and direction but length of 1.
        /// </summary>
        /// <param name="he">Input half edge.</param>
        /// <returns>The unit vector.</returns>
        internal static BeachHalfEdge MakeUnitVectorKeepEndingPoint(BeachHalfEdge he, bool keepEndingPoint = true)
        {
            if (he.StartingX == he.DirectionX)
            {
                return new BeachHalfEdge(he.StartingX, he.DirectionY - 1, he.DirectionX, he.DirectionY);
            }
            else if (he.StartingY == he.DirectionY)
            {
                return new BeachHalfEdge(he.DirectionX - 1, he.StartingY, he.DirectionX, he.DirectionY);
            }
            else
            {
                double lengthOld = Math.Sqrt(Math.Pow(he.DirectionX - he.StartingX, 2) + Math.Pow(he.DirectionY - he.StartingY, 2));
                double xLengthNew = (he.DirectionX - he.StartingX) / lengthOld;
                double yLengthNew = (he.DirectionY - he.StartingY) / lengthOld;

                if (keepEndingPoint)
                {
                    return new BeachHalfEdge(he.DirectionX - xLengthNew, he.DirectionY - yLengthNew, he.DirectionX, he.DirectionY);
                }
                else
                {
                    return new BeachHalfEdge(he.StartingX, he.StartingY, he.StartingX + xLengthNew, he.StartingY + yLengthNew);
                }
            }
        }


        /// <summary>
        /// Returns true if the two halfedges are going to intersect at some point.
        /// </summary>
        /// <param name="a">Edge starting from the left.</param>
        /// <param name="b">Esge starting from the right.</param>
        /// <returns>True if intersection is possible.</returns>
        internal static bool FutureIntersectionPossible(BeachHalfEdge a, BeachHalfEdge b)
        {

            Point intersection = new Point(a, b);

            return BeachHalfEdge.PointInFuture(a, intersection) && BeachHalfEdge.PointInFuture(b, intersection);
        }

        private static bool PointInFuture(BeachHalfEdge a, Point intersection)
        {
            bool xDominant = Math.Abs(a.deltaX) > Math.Abs(a.deltaY);

            BeachHalfEdge vectorToIntersection = new BeachHalfEdge(a.StartingX, a.StartingY, intersection.X, intersection.Y);

            if (xDominant)
            {
                return (a.deltaX > 0) == (vectorToIntersection.deltaX > 0);

            }
            else
            {
                return (a.deltaY > 0) == (vectorToIntersection.deltaY > 0);
            }
        }
    }
}
