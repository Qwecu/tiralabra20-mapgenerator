namespace Mapseesharp
{
    using System.Diagnostics;

    /// <summary>
    /// Represents an arc on the beachline.
    /// </summary>
    public class BeachArc : BeachObj
    {
        // private Site s;

        // Tarvittaessa annetaan rajat, joihin paraabeli rajautuu oikealla ja vasemmalla

        /// <summary>
        /// Initializes a new instance of the <see cref="BeachArc"/> class.
        /// </summary>
        /// <param name="s">The focus point of the parabola.</param>
        /// <param name="limLeft">Optional limit on left side.</param>
        /// <param name="limRight">Optional limit on right side.</param>
        public BeachArc(Site s, double limLeft = double.NegativeInfinity, double limRight = double.PositiveInfinity)
        {
            this.Homesite = s;
            this.LeftLimit = limLeft;
            this.RightLimit = limRight;
        }

        /// <summary>
        /// Gets the focus point of the parabola.
        /// </summary>
        public Site Homesite { get; private set; }

        /// <summary>
        /// Gets the x coordinate of the parabola's focus point.
        /// </summary>
        public double HomeX
        {
            get { return this.Homesite.X; }
        }

        /// <summary>
        /// Gets the y coordinate of the parabola's focus point.
        /// </summary>
        public double HomeY
        {
            get { return this.Homesite.Y; }
        }

        /// <summary>
        /// Gets the limit on the left side.
        /// </summary>
        public double LeftLimit { get; private set; }

        /// <summary>
        /// Gets the limit on the right side.
        /// </summary>
        public double RightLimit { get; private set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "BeachArc homesite (" + this.HomeX + ", " + this.HomeY + "), limits: Left " + this.LeftLimit + ", right " + this.RightLimit;
        }

        /// <summary>
        /// Returns the distanse from a given point on the swipeline.
        /// Source: https://jacquesheunis.com/post/fortunes-algorithm/.
        /// </summary>
        /// <param name="newsite">Site.</param>
        /// <returns>Distance.</returns>
        internal double DistFromDirectrixX(Site newsite)
        {
            double yf = this.Homesite.Y;
            double xf = this.Homesite.X;
            double yd = newsite.Y;
            double x = newsite.X;
            double yCoordinate = ((1.0 / (2.0 * (yf - yd)))
                * (x - xf) * (x - xf))
                + ((yf + yd) / 2.0);
            double distance = yCoordinate - yd;

            Debug.Assert(distance >= 0f, "Negative distance");

            return distance;
        }

        /// <summary>
        /// Returns the distanse from a given point.
        /// Source: https://jacquesheunis.com/post/fortunes-algorithm/.
        /// </summary>
        /// <param name="intersection">point.</param>
        /// <returns>Distance.</returns>
        internal double DistFromDirectrixX(Point intersection)
        {
            double yf = this.Homesite.Y;
            double xf = this.Homesite.X;
            double yd = intersection.Y;
            double x = intersection.X;
            double yCoordinate = ((1.0 / (2.0 * (yf - yd)))
                * (x - xf) * (x - xf))
                + ((yf + yd) / 2.0);
            double distance = yCoordinate - yd;
            return distance;
        }
    }
}
