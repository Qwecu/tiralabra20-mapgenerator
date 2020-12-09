using System.Collections.Generic;
using System.Linq;

namespace Mapseesharp
{
    public class ResultObject
    {
        public VoronoiList<BeachArc> BeachArcs { get; set; }

        public VoronoiList<BeachHalfEdge> BeachHalfEdges { get; set; }

        public VoronoiList<Edge> FinishedEdges { get; set; }

        public MaxHeap<Evnt> Events { get; set; }

        public VoronoiList<EvntCircle> OldCircleEvents { get; set; }

        public VoronoiList<BeachObj> Beachline { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public bool Ready { get; set; }

        public ResultObject(MaxHeap<Evnt> events, VoronoiList<Edge> finishedEdges, VoronoiList<BeachObj> beachline, VoronoiList<EvntCircle> oldCircleEvents, int width, int height, bool ready = false)
        {
            this.Events = events;

            this.FinishedEdges = finishedEdges;

            this.BeachArcs = beachline.GetElementsOfTypeBeachArc();
            this.BeachHalfEdges = beachline.GetElementsOfTypeBeachHalfEdge();//beachline.Where(x => x.GetType().Equals(typeof(BeachHalfEdge))).Select(x => (BeachHalfEdge)x).ToList();
            this.OldCircleEvents = oldCircleEvents;
            this.Beachline = beachline;
            this.Width = width;
            this.Height = height;
            this.Ready = ready;
        }
    }
}
