namespace Mapseesharp
{
    /// <summary>
    /// Represents result of an iteration.
    /// </summary>
    public class ResultObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultObject"/> class.
        /// </summary>
        /// <param name="events">The upcoming events.</param>
        /// <param name="finishedEdges">The finished edges.</param>
        /// <param name="beachline">The beachline.</param>
        /// <param name="oldCircleEvents">The old circle events, processed or discarded.</param>
        /// <param name="width">Width of canvas.</param>
        /// <param name="height">Height of canvas.</param>
        /// <param name="ready">True if ready.</param>
        public ResultObject(MaxHeap<Evnt> events, VoronoiList<Edge> finishedEdges, VoronoiList<BeachObj> beachline, VoronoiList<EvntCircle> oldCircleEvents, int width, int height, bool ready = false)
        {
            this.Events = events;

            this.FinishedEdges = finishedEdges;

            this.BeachArcs = beachline.GetAllElementsOfTypeBeachArc();
            this.BeachHalfEdges = beachline.GetAllElementsOfTypeBeachHalfEdge();
            this.OldCircleEvents = oldCircleEvents;
            this.Beachline = beachline;
            this.Width = width;
            this.Height = height;
            this.Ready = ready;
        }

        /// <summary>
        /// Gets the beach arcs.
        /// </summary>
        public VoronoiList<BeachArc> BeachArcs { get; private set; }

        /// <summary>
        /// Gets the half edges.
        /// </summary>
        public VoronoiList<BeachHalfEdge> BeachHalfEdges { get; private set; }

        /// <summary>
        /// Gets the finished edges.
        /// </summary>
        public VoronoiList<Edge> FinishedEdges { get; private set; }

        /// <summary>
        /// Gets the events.
        /// </summary>
        public MaxHeap<Evnt> Events { get; private set; }

        /// <summary>
        /// Gets the old circle events.
        /// </summary>
        public VoronoiList<EvntCircle> OldCircleEvents { get; private set; }

        /// <summary>
        /// Gets the beachline.
        /// </summary>
        public VoronoiList<BeachObj> Beachline { get; private set; }

        /// <summary>
        /// Gets width of canvas.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// Gets height of canvas.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the algorithm has finished.
        /// </summary>
        public bool Ready { get; private set; }
    }
}
