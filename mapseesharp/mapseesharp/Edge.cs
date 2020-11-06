using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{
    class Edge
    {
        public Point StartingPoing { get; set; }
        public Point EndingPoint { get; set; }

        public Edge(Point startingPoing, Point endingPoint)
        {
            this.StartingPoing = startingPoing;
            this.EndingPoint = endingPoint;
        }
    }
}
