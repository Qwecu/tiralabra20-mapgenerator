using System;

namespace Mapseesharp
{
    /// <summary>
    /// A circle event.
    /// </summary>
    public class EvntCircle : Evnt
    {
        public BeachHalfEdge LeftEdge { get; set; }

        public BeachArc DisappearingArc { get; set; }

        public BeachHalfEdge RightEdge { get; set; }

        public Site site { get { return this.DisappearingArc.Homesite; } }

        public double PosEventY { get; set; }

        public Point CircleCentre { get; set; }

        /// <inheritdoc/>
        public override double YToHappen
        {
            get
            {
                return this.PosEventY;
            }
        }

        public EvntCircle(double positionY, BeachArc newarc, BeachHalfEdge leftEdge, BeachHalfEdge rightEdge, Point circleCentre)
        {
            this.PosEventY = positionY;
            this.DisappearingArc = newarc;
            this.LeftEdge = leftEdge;
            this.RightEdge = rightEdge;
            this.CircleCentre = circleCentre;
        }

        public override string ToString()
        {
            return "Circle event, happening " + PosEventY + ", CircleCentre " + CircleCentre + ", site " + site;
        }
    }
}