using System.Collections.Generic;
using System.Linq;

namespace Mapseesharp
{
    public class ResultObject
    {
        public List<BeachArc> BeachArcs { get; set; }

        public List<BeachHalfEdge> BeachHalfEdges { get; set; }

        public List<Edge> FinishedEdges { get; set; }

        public SortedList<double, Evnt> Events { get; set; }

        public List<EvntCircle> OldCircleEvents { get; set; }

        public List<BeachObj> Beachline { get; set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public bool Ready { get; set; }

        public ResultObject(SortedList<double, Evnt> events, List<Edge> finishedEdges, List<BeachObj> beachline, List<EvntCircle> oldCircleEvents, int width, int height, bool ready = false)
        {
            this.Events = events;

            this.FinishedEdges = finishedEdges;

            this.BeachArcs = beachline.Where(x => x.GetType().Equals(typeof(BeachArc))).Select(x => (BeachArc)x).ToList();
            this.BeachHalfEdges = beachline.Where(x => x.GetType().Equals(typeof(BeachHalfEdge))).Select(x => (BeachHalfEdge)x).ToList();
            this.OldCircleEvents = oldCircleEvents;
            this.Beachline = beachline;
            this.Width = width;
            this.Height = height;
            this.Ready = ready;
        }
    }
}
