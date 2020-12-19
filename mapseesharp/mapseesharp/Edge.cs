namespace Mapseesharp
{
    /// <summary>
    /// Rpresents an edge on the map.
    /// </summary>
    public class Edge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Edge"/> class.
        /// </summary>
        /// <param name="startingPoint">The starting point.</param>
        /// <param name="endingPoint">The ending point.</param>
        public Edge(Point startingPoint, Point endingPoint)
        {
            this.StartingPoint = startingPoint;
            this.EndingPoint = endingPoint;
        }

        /// <summary>
        /// Gets the starting point of the edge.
        /// </summary>
        public Point StartingPoint { get; private set; }

        /// <summary>
        /// Gets the ending point of the edge.
        /// </summary>
        public Point EndingPoint { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the edge is pointing left.
        /// </summary>
        public bool PointingLeft
        {
            get { return this.EndingPoint.X < this.StartingPoint.X; }
        }

        /// <summary>
        /// Gets a value indicating whether the edge is pointing right.
        /// </summary>
        public bool PointingRight
        {
            get { return this.EndingPoint.X > this.StartingPoint.X; }
        }

        /// <summary>
        /// Gets a value indicating whether the edge is pointing up.
        /// </summary>
        public bool PointingUp
        {
            get { return this.EndingPoint.Y > this.StartingPoint.Y; }
        }

        /// <summary>
        /// Gets a value indicating whether the edge is pointing down.
        /// </summary>
        public bool PointingDown
        {
            get { return this.EndingPoint.Y < this.StartingPoint.Y; }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "Edge (" + this.StartingPoint.X + "; " + this.StartingPoint.Y + ") (" + this.EndingPoint.X + "; " + this.EndingPoint.Y + ")";
        }

        /// <summary>
        /// True if both endpoint are outside the map.
        /// </summary>
        /// <param name="width">Width of map.</param>
        /// <param name="height">Height of map.</param>
        /// <returns>True if both ends outside map.</returns>
        internal bool BothEndpointsOutsideMap(int width, int height)
        {
            return !(this.StartingPoint.OnMap(width, height) || this.EndingPoint.OnMap(width, height));
        }
    }
}
