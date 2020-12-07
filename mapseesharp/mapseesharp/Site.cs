namespace Mapseesharp
{
    /// <summary>
    /// A site.
    /// </summary>
    public class Site
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Site"/> class.
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        public Site(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets or sets x position.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets y position.
        /// </summary>
        public double Y { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "Site " + this.X + "; " + this.Y;
        }
    }
}
