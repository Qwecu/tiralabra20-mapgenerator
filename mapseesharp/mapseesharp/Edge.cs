using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapseesharp
{
    public class Edge
    {
        public Point StartingPoint { get; set; }
        public Point EndingPoint { get; set; }

        public bool PointingLeft { get { return (EndingPoint.X < StartingPoint.X); } }
        public bool PointingRight { get { return (EndingPoint.X > StartingPoint.X); } }
        public bool PointingUp { get { return (EndingPoint.Y > StartingPoint.Y); } }
        public bool PointingDown { get { return (EndingPoint.Y < StartingPoint.Y); } }

        public Edge(Point startingPoint, Point endingPoint)
        {
            this.StartingPoint = startingPoint;
            this.EndingPoint = endingPoint;
        }

        public override string ToString()
        {
            return "Edge (" + StartingPoint.X + "; " + StartingPoint.Y + ") (" + EndingPoint.X + "; " + EndingPoint.Y + ")";
        }

        internal bool BothEndpointsOutsideMap(int width, int height)
        {
            return !(StartingPoint.OnMap(width, height) || EndingPoint.OnMap(width, height));
        }
    }
}
