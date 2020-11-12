using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{
    class VoronoiCalculator
    {
        internal ResultObject Iterate(Site[] sites)
        {
            SortedList<double, Evnt> events = new SortedList<double, Evnt>(new ReverseComparer());
            List<BeachObj> beachline = new List<BeachObj>();
            List<Edge> FinishedEdges = new List<Edge>();
            List<EvntCircle> OldCircleEvents = new List<EvntCircle>();

            //Fill the event queue with site events for each input site.
            //	-order by y-coordinate of the site
            foreach (Site test in sites) { events.Add(test.y, new EvntSite(test)); }
            return Iterate(events, FinishedEdges, beachline, OldCircleEvents);

        }

        //internal ResultObject Calculate(Site[] sites)
        internal ResultObject Iterate(SortedList<double, Evnt> events, List<Edge> FinishedEdges, List<BeachObj> beachline, List<EvntCircle> OldCircleEvents)
        {
            ///TODO: Make sure that the sites can't be too near each other

            /*SortedList<double, Evnt> events = new SortedList<double, Evnt>(new ReverseComparer());
            List<BeachObj> beachline = new List<BeachObj>();
            List<Edge> FinishedEdges = new List<Edge>();
            List<EvntCircle> OldCircleEvents = new List<EvntCircle>();*/

            //Fill the event queue with site events for each input site.
            //	-order by y-coordinate of the site
            //foreach (Site test in sites) { events.Add(test.y, new EvntSite(test)); }

            if (beachline.Count == 0)
            {

                EvntSite first = (EvntSite)events[events.Keys[0]];

                beachline.Add(new BeachArc(first.site));

                events.Remove(events.Keys[0]);

                return new ResultObject(events, FinishedEdges, beachline, OldCircleEvents);
            }



            //While the event queue still has items in it:
            //while (events.Count > 0)
            //{
            Evnt next = events[events.Keys[0]];

            //tallennetaan oikea key jotta saadaan tämä event poistettua (TODO keksi tapa jossa ei synny clasheja)
            double nextKey = events.Keys[0];

            //    If the next event on the queue is a site event:
            if (next.IsSiteEvent)
            {
                EvntSite currentSiteEvent = (EvntSite)next;
                //        Add the new site to the beachline
                //		-search for arc above the point
                //filtteröidään pelkät kaaret listalle
                List<BeachArc> arcs = beachline.Where(x => x.GetType().Equals(typeof(BeachArc))).Select(x => (BeachArc)x).ToList();

                Tuple<BeachArc, double> aboves = GetArcAbove(arcs, currentSiteEvent.site);
                BeachArc above = aboves.Item1;
                double bestDistance = aboves.Item2;

                int indexOfBest = beachline.IndexOf(above);

                //sopivaan indeksiin:/beachline.Add(new BeachArc(se.s));
                //		-remove the arc right above it
                beachline.Remove(above);
                //		-add new arc
                BeachArc newbornArc = new BeachArc(currentSiteEvent.site);
                //		-add two copies of the removed arc
                BeachArc newLeftSideArc = new BeachArc(above.Homesite, limLeft: above.LeftLimit, limRight: currentSiteEvent.x);
                BeachArc newRightSideArc = new BeachArc(above.Homesite, limLeft: currentSiteEvent.x, limRight: above.RightLimit);
                //		-add two half-edges starting from the point in the original arc right above the new site
                //          -starting point is straight above the new site, using the shortest distance to the parable above
                //          -suuntavektori saadaan pisteiden puolivälin avulla (x- ja y-koordinaattien on oltava eri)
                double startingX = currentSiteEvent.x;
                double startingY = currentSiteEvent.y + bestDistance;

                //midway between new and old focus point
                double midwayX = (above.HomeX + currentSiteEvent.x) / 2;
                double midwayY = (above.HomeY + currentSiteEvent.y) / 2;

                //suuntavektorit uusille kaarille
                double directionLeftX = 0;
                double directionLeftY = 0;
                double directionRightX = 0;
                double directionRightY = 0;

                //keskikohta löytyi vasemmalta                   
                if (midwayX < startingX)
                {
                    directionLeftX = midwayX;
                    directionLeftY = midwayY;
                    directionRightX = startingX + (startingX - midwayX);
                    directionRightY = startingY + (startingY - midwayY);
                }
                //jos ei löydy vasemmalta, löytyy varmasti oikealta
                else
                {
                    directionRightX = midwayX;
                    directionRightY = midwayY;
                    directionLeftX = startingX + (startingX - midwayX);
                    directionLeftY = startingY + (startingY - midwayY);
                }

                BeachHalfEdge edgeToLeft = new BeachHalfEdge(startingX, startingY, directionLeftX, directionLeftY);
                BeachHalfEdge edgeToRight = new BeachHalfEdge(startingX, startingY, directionRightX, directionRightY);

                beachline.InsertRange(indexOfBest,
                    new List<BeachObj> {
                            newLeftSideArc,
                            edgeToLeft,
                            newbornArc,
                            edgeToRight,
                            newRightSideArc
                });
                //		-add possible circle events to the event queue
                //			-check if the lines next to the new arcs are going to intersect
                List<BeachArc> noobs = new List<BeachArc> { newLeftSideArc, newRightSideArc };
                foreach (BeachArc newarc in noobs)
                {
                    EvntCircle newevent = TryAddCircleEvent(newarc, beachline);
                    if (newevent != null) { events.Add(newevent.PosEventY, newevent); }
                }

            }

            //    Otherwise it must be an edge-intersection (circle) event:
            else
            {
                EvntCircle currentCircEv = (EvntCircle)next;
                //       check validity TODO tässä voi piillä bugi lähtöisin siitä kun kaaria korvataan uusilla
                BeachArc disappArc = currentCircEv.DisappearingArc;
                int? indexOnBeach = beachline.IndexOf(disappArc);
                if (indexOnBeach != -1 && beachline[(int)indexOnBeach - 1] == currentCircEv.leftEdge && beachline[(int)indexOnBeach + 1] == currentCircEv.rightEdge)
                {

                    //        Remove the squeezed cell from the beachline
                    //		-remove arc
                    beachline.Remove(disappArc);
                    //		-add new single half-edge starting from intersection point
                    BeachArc futureLeft = (BeachArc)(beachline[(int)indexOnBeach - 2]);
                    BeachArc futureRight = (BeachArc)(beachline[(int)indexOnBeach + 1]);
                    double xDirPoint = (futureLeft.HomeX + futureRight.HomeX) / 2;
                    double yDirPoint = (futureLeft.HomeY + futureRight.HomeY) / 2;

                    BeachHalfEdge singleHalfEdge = new BeachHalfEdge(currentCircEv.CircleCentre.x, currentCircEv.CircleCentre.y, xDirPoint, yDirPoint);
                    beachline.Insert((int)indexOnBeach, singleHalfEdge);

                    //		-the half-edges become finished edges, remove from beachline
                    beachline.Remove(currentCircEv.leftEdge);
                    beachline.Remove(currentCircEv.rightEdge);
                    //add finished edges
                    FinishedEdges.Add(new Edge(new Point(currentCircEv.leftEdge.startingX, currentCircEv.leftEdge.startingY), currentCircEv.CircleCentre));
                    FinishedEdges.Add(new Edge(new Point(currentCircEv.rightEdge.startingX, currentCircEv.rightEdge.startingY), currentCircEv.CircleCentre));
                    //-check both arcs for new future intersections

                    List<BeachArc> noobs = new List<BeachArc> { futureLeft, futureRight };
                    foreach (BeachArc newarc in noobs)
                    {
                        EvntCircle newevent = TryAddCircleEvent(newarc, beachline);
                        if (newevent != null) { events.Add(newevent.PosEventY, newevent); }
                    }
                }
                OldCircleEvents.Add(currentCircEv);
            }
            events.Remove(nextKey);
            //Cleanup any remaining intermediate state
            //	-remaining collisions must only have one arc in between
            //too many edges are not needed


            
            return new ResultObject(events, FinishedEdges, beachline, OldCircleEvents);
        }

        private Tuple<BeachArc, double> GetArcAbove(List<BeachArc> arcs, Site site)
        {
            double bestDistance = -1;
            BeachArc above = null;

            if (double.IsNaN(bestDistance)) { throw new Exception("Etäisyyden laskemisessa virhe"); }

            foreach (BeachArc arc in arcs)
            {
                //erotetaan identtiset kaaret toisistaan
                if (arc.LeftLimit > site.x || arc.RightLimit < site.x) continue;
                else
                {
                    double distance = arc.DistFromDirectrixX(site);
                    if (bestDistance == -1 || distance < bestDistance) { bestDistance = distance; above = arc; }
                }
            }
            return new Tuple<BeachArc, double>(above, bestDistance);
        }
        

        /*
        private EvntCircle TryAddCircleEvent(BeachArc newarc, List<BeachObj> beachline)
        {
            EvntCircle res = null;
            int noobindex = beachline.IndexOf(newarc);

            if (noobindex - 1 >= 0 && noobindex + 1 <= beachline.Count - 1
                && beachline[noobindex - 1].GetType().Equals(typeof(BeachHalfEdge))
                && beachline[noobindex + 1].GetType().Equals(typeof(BeachHalfEdge))
               && !(  ((BeachHalfEdge)beachline[noobindex - 1]).PointingLeft && ((BeachHalfEdge)beachline[noobindex + 1]).PointingRight)
                )
            {
                BeachHalfEdge leftEdge = (BeachHalfEdge)beachline[noobindex - 1];
                BeachHalfEdge rightEdge = (BeachHalfEdge)beachline[noobindex + 1];
                Point intersection = new Point(leftEdge, rightEdge);

                List<BeachArc> arcs = beachline.Where(x => x.GetType().Equals(typeof(BeachArc))).Select(x => (BeachArc)x).ToList();
                BeachArc above = arcs[0];
                double bestDistance = above.DistFromDirectrixX(intersection);

                if (double.IsNaN(bestDistance)) { throw new Exception("Etäisyyden laskemisessa virhe"); }
                foreach (BeachArc arc in arcs)
                {
                    //erotetaan identtiset kaaret toisistaan
                    if (arc.LeftLimit > intersection.x || arc.RightLimit < intersection.x) continue;
                    else
                    {
                        double distance = arc.DistFromDirectrixX(intersection);
                        if (distance < bestDistance) { bestDistance = distance; above = arc; }
                    }
                }
                //			-if yes, add circle event to queue
                //			-y-coordinate of event (sweepline location) is point of intersection minus distance to endpoint
                //tsekataan että löytyy "tulevaisuudesta" (tämä lienee turha, tarkistaa siis että viivat kohtaavat paraabelin polttopisteen alapuolella)
                if (intersection.y < newarc.HomeY)
                {
                //pisteen etäisyys focus pointista on sama kuin pisteen etäisyys swipelinesta eventin aikana
                double distFromFocus = Math.Sqrt(Math.Pow((newarc.HomeX - intersection.x), 2) + Math.Pow(newarc.HomeY - intersection.y, 2));
                var circleEvent = new EvntCircle(intersection.y - distFromFocus, newarc, leftEdge, rightEdge, intersection);

                res = circleEvent;
                }
            }
            return res;
        }*/


        private EvntCircle TryAddCircleEvent(BeachArc newarc, List<BeachObj> beachline)
        {
            EvntCircle res = null;
            int noobindex = beachline.IndexOf(newarc);

            if (noobindex - 1 >= 0 && noobindex + 1 <= beachline.Count - 1
                && beachline[noobindex - 1].GetType().Equals(typeof(BeachHalfEdge))
                && beachline[noobindex + 1].GetType().Equals(typeof(BeachHalfEdge))
                && ((BeachHalfEdge)beachline[noobindex - 1]).PointingRight
                && ((BeachHalfEdge)beachline[noobindex + 1]).PointingLeft //VÄÄRÄ OLETUS
                )
            {
                BeachHalfEdge leftEdge = (BeachHalfEdge)beachline[noobindex - 1];
                BeachHalfEdge rightEdge = (BeachHalfEdge)beachline[noobindex + 1];
                Point intersection = new Point(leftEdge, rightEdge);
                //			-if yes, add circle event to queue
                //			-y-coordinate of event (sweepline location) is point of intersection minus distance to endpoint
                //tsekataan että löytyy "tulevaisuudesta" (tämä lienee turha, tarkistaa siis että viivat kohtaavat paraabelin polttopisteen alapuolella)
                //if (intersection.y < newarc.HomeY)
                //{
                //pisteen etäisyys focus pointista on sama kuin pisteen etäisyys swipelinesta eventin aikana
                double distFromFocus = Math.Sqrt(Math.Pow((newarc.HomeX - intersection.x), 2) + Math.Pow(newarc.HomeY - intersection.y, 2));
                var circleEvent = new EvntCircle(intersection.y - distFromFocus, newarc, leftEdge, rightEdge, intersection);

                res = circleEvent;
                //}
            }
            return res;
        }
    }
}
