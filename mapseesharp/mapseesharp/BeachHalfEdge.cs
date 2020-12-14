using System;
using System.Collections.Generic;
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

        public BeachHalfEdge(double startingX, double startingY, double directionX, double directionY)
        {
            this.StartingX = startingX;
            this.StartingY = startingY;
            this.DirectionX = directionX;
            this.DirectionY = directionY;
        }

        public override string ToString()
        {
            return "HalfEdge (" + this.StartingX + "; " + this.StartingY + ") (" + this.DirectionX + "; " + this.DirectionY + ")";
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
        internal static BeachHalfEdge MakeUnitVectorKeepEndingPoint(BeachHalfEdge he)
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

                return new BeachHalfEdge(he.DirectionX - xLengthNew, he.DirectionY - yLengthNew, he.DirectionX, he.DirectionY);
            }
        }
    }
}
