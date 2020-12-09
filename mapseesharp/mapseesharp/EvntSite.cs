using System;

namespace Mapseesharp
{
    /// <summary>
    /// A site event.
    /// </summary>
    public class EvntSite : Evnt
    {
        public Site Site { get; private set; }

        public double X { get { return this.Site.X; } }

        public double Y { get { return this.Site.Y; } }

        public override double YToHappen
        {
            get
            {
                return this.Y;
            }
        }

        public EvntSite(Site s)
        {
            this.Site = s;
        }

        public override string ToString()
        {
            return "Site event at (" + X + "; " + Y + ")";
        }
    }
}