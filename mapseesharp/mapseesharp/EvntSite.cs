using System;

namespace Mapseesharp
{
    public class EvntSite : Evnt
    {
        public Site site { get; private set; }

        public double x { get { return site.X; } }
        public double y { get { return site.Y; } }

        public override double YToHappen
        {
            get
            {
                return y;
            }
        }

        public EvntSite(Site s)
        {
            this.site = s;
            this.IsSiteEvent = true;
        }
    }
}