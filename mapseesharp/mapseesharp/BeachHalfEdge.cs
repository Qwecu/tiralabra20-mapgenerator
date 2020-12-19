namespace Mapseesharp
{
    using System;

    /// <summary>
    /// Represents a growing edge with a starting point and direction.
    /// </summary>
    public class BeachHalfEdge : BeachObj
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BeachHalfEdge"/> class.
        /// </summary>
        /// <param name="startingX">Starting x coordinate.</param>
        /// <param name="startingY">Starting y coordinate.</param>
        /// <param name="directionX">Direction x coordinate.</param>
        /// <param name="directionY">Direction y coordinate.</param>
        public BeachHalfEdge(double startingX, double startingY, double directionX, double directionY)
        {
            this.StartingX = startingX;
            this.StartingY = startingY;
            this.DirectionX = directionX;
            this.DirectionY = directionY;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeachHalfEdge"/> class.
        /// </summary>
        /// <param name="a">Starting point.</param>
        /// <param name="b">Direction point.</param>
        public BeachHalfEdge(Point a, Point b)
        {
            this.StartingX = a.X;
            this.StartingY = a.Y;
            this.DirectionX = b.X;
            this.DirectionY = b.Y;
        }

        /// <summary>
        /// Gets or sets the starting x coordinate.
        /// </summary>
        public double StartingX { get; set; }

        /// <summary>
        /// Gets or sets the starting y coordinate.
        /// </summary>
        public double StartingY { get; set; }

        /// <summary>
        /// Gets or sets the direction x coordinate.
        /// </summary>
        public double DirectionX { get; set; }

        /// <summary>
        /// Gets or sets the direction y coordinate.
        /// </summary>
        public double DirectionY { get; set; }

        /// <summary>
        /// Gets the starting point.
        /// </summary>
        public Point StartingPoint => new Point(this.StartingX, this.StartingY);

        /// <summary>
        /// Gets the direction point.
        /// </summary>
        public Point DirectionPoint => new Point(this.DirectionX, this.DirectionY);

        /// <summary>
        /// Gets the difference between starting and direction x coordinates.
        /// </summary>
        public double DeltaX => this.DirectionX - this.StartingX;

        /// <summary>
        /// Gets the difference between starting and direction y coordinates.
        /// </summary>
        public double DeltaY => this.DirectionY - this.StartingY;

        /// <summary>
        /// Gets a value indicating whether the edge is pointing left.
        /// </summary>
        public bool PointingLeft
        {
            get { return this.DirectionX < this.StartingX; }
        }

        /// <summary>
        /// Gets a value indicating whether the edge is pointing right.
        /// </summary>
        public bool PointingRight
        {
            get { return this.DirectionX > this.StartingX; }
        }

        /// <summary>
        /// Gets a value indicating whether the edge is pointing up.
        /// </summary>
        public bool PointingUp
        {
            get { return this.DirectionY > this.StartingY; }
        }

        /// <summary>
        /// Gets a value indicating whether the edge is pointing down.
        /// </summary>
        public bool PointingDown
        {
            get { return this.DirectionY < this.StartingY; }
        }

        /// <summary>
        /// Gets a value indicating whether the edge is vertical.
        /// </summary>
        public bool IsVertical => this.StartingX == this.DirectionX;

        /// <summary>
        /// Gets a value indicating whether the edge is horizontal.
        /// </summary>
        public bool IsHorizontal => this.StartingY == this.DirectionY;

        /// <inheritdoc/>
        public override string ToString()
        {
            return "HalfEdge (" + this.StartingX + ", " + this.StartingY + ") (" + this.DirectionX + ", " + this.DirectionY + ")";
        }

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
        /// /// <param name="keepEndingPoint">If true, ending point is kept.</param>
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

        /// <summary>
        /// Returns true if the edge is pointing to the direction of the point.
        /// </summary>
        /// <param name="a">A half edge.</param>
        /// <param name="intersection">A point that has to be on the same line as the edge.</param>
        /// <returns>A value indicating whether the edge is pointing to the point.</returns>
        internal static bool PointInFuture(BeachHalfEdge a, Point intersection)
        {
            bool xDominant = Math.Abs(a.DeltaX) > Math.Abs(a.DeltaY);

            BeachHalfEdge vectorToIntersection = new BeachHalfEdge(a.StartingX, a.StartingY, intersection.X, intersection.Y);

            if (xDominant)
            {
                return (a.DeltaX > 0) == (vectorToIntersection.DeltaX > 0);
            }
            else
            {
                return (a.DeltaY > 0) == (vectorToIntersection.DeltaY > 0);
            }
        }
    }
}
