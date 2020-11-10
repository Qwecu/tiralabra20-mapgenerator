using System;

namespace mapseesharp
{
    public class EvntCircle : Evnt
    {
        public BeachHalfEdge leftEdge { get; set; }
        public BeachArc DisappearingArc { get; set; }
        public BeachHalfEdge rightEdge { get; set; }
        public Site site { get { return DisappearingArc.Homesite; } }
        public double PosEventY { get; set; }
        public Point CircleCentre { get; set; }

        public override double YToHappen
        {
            get
            {
                return PosEventY;
            }
        }

        public EvntCircle(double positionY, BeachArc newarc, BeachHalfEdge leftEdge, BeachHalfEdge rightEdge, Point circleCentre)
        {
            this.IsSiteEvent = false;
            this.PosEventY = positionY;
            this.DisappearingArc = newarc;
            this.leftEdge = leftEdge;
            this.rightEdge = rightEdge;
            CircleCentre = circleCentre;
        }
    }
}