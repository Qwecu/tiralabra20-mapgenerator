namespace mapseesharp
{
    internal class EvntSite : Evnt
    {
        public Site site { get; private set; }

        public double x { get { return site.x; } }
        public double y { get { return site.y; } }

        public EvntSite(Site s)
        {
            this.site = s;
            this.IsSiteEvent = true;
        }
    }
}