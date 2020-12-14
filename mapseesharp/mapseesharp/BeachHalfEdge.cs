using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapseesharp
{
    public class BeachHalfEdge : BeachObj
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

        public override string ToString()
        {
            return "HalfEdge (" + startingX + "; " + startingY + ") (" + directionX + "; " + directionY + ")";
        }

        public bool PointingLeft { get { return (directionX < this.startingX); } }

        public bool PointingRight { get { return (directionX > startingX); } }

        public bool PointingUp { get { return (directionY > startingY); } }

        public bool PointingDown { get { return (directionY < startingY); } }

        /// <summary>
        /// Returns a new half edge with the same length and starting point as input, pointing to opposite direction.
        /// </summary>
        /// <param name="singleHalfEdge">Input.</param>
        /// <returns>Mirrored half edge.</returns>
        internal static BeachHalfEdge MirrorKeepStartingPoint(BeachHalfEdge singleHalfEdge)
        {
            return new BeachHalfEdge(
                            singleHalfEdge.startingX,
                            singleHalfEdge.startingY,
                            singleHalfEdge.startingX - (singleHalfEdge.directionX - singleHalfEdge.startingX),
                            singleHalfEdge.startingY - (singleHalfEdge.directionY - singleHalfEdge.startingY));
        }
    }
}
