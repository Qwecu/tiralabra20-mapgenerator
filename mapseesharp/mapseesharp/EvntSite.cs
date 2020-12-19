using System;

namespace Mapseesharp
{
    /// <summary>
    /// A site event.
    /// </summary>
    public class EvntSite : Evnt
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvntSite"/> class.
        /// </summary>
        /// <param name="s">Site.</param>
        public EvntSite(Site s)
        {
            this.Site = s;
        }

        /// <summary>
        /// Gets the new site.
        /// </summary>
        public Site Site { get; private set; }

        /// <summary>
        /// Gets x coordinate of the site.
        /// </summary>
        public double X
        {
            get { return this.Site.X; }
        }

        /// <summary>
        /// Gets y coordinate of the site.
        /// </summary>
        public double Y
        {
            get { return this.Site.Y; }
        }

        /// <summary>
        /// Gets the y coordinate of the site.
        /// </summary>
        public override double YToHappen
        {
            get
            {
                return this.Y;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "Site event at (" + this.X + "; " + this.Y + ")";
        }
    }
}