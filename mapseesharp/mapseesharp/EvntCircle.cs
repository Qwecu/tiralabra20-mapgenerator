using System;

namespace Mapseesharp
{
    /// <summary>
    /// A circle event.
    /// </summary>
    public class EvntCircle : Evnt
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvntCircle"/> class.
        /// </summary>
        /// <param name="positionY">The y position.</param>
        /// <param name="newarc">The disappearing arc.</param>
        /// <param name="leftEdge">Edge from left.</param>
        /// <param name="rightEdge">Edge from right.</param>
        /// <param name="circleCentre">Circle centre where all the edges meet.</param>
        public EvntCircle(double positionY, BeachArc newarc, BeachHalfEdge leftEdge, BeachHalfEdge rightEdge, Point circleCentre)
        {
            this.PosEventY = positionY;
            this.DisappearingArc = newarc;
            this.LeftEdge = leftEdge;
            this.RightEdge = rightEdge;
            this.CircleCentre = circleCentre;
        }

        /// <summary>
        /// Gets edge approaching from left.
        /// </summary>
        public BeachHalfEdge LeftEdge { get; private set; }

        /// <summary>
        /// Gets the arc that is squeezed to nothingness.
        /// </summary>
        public BeachArc DisappearingArc { get; private set; }

        /// <summary>
        /// Gets edge approaching from left.
        /// </summary>
        public BeachHalfEdge RightEdge { get; private set; }

        /// <summary>
        /// Gets the homesite of the disappearing arc.
        /// </summary>
        public Site Site
        {
            get { return this.DisappearingArc.Homesite; }
        }

        /// <summary>
        /// Gets the y position of the event.
        /// </summary>
        public double PosEventY { get; private set; }

        /// <summary>
        /// Gets the centre point of the circle.
        /// </summary>
        public Point CircleCentre { get; private set; }

        /// <inheritdoc/>
        public override double YToHappen
        {
            get
            {
                return this.PosEventY;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "Circle event, happening " + this.PosEventY + ", CircleCentre " + this.CircleCentre + ", site " + this.Site;
        }
    }
}