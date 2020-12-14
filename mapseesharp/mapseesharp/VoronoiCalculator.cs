namespace Mapseesharp
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class includes the main algorithm.
    /// </summary>
    internal class VoronoiCalculator
    {
        /// <summary>
        /// Returns the next iteration of the algorithm.
        /// </summary>
        /// <param name="sites">Sites that define the division.</param>
        /// <param name="width">Width of canvas.</param>
        /// <param name="height">Height of canvas.</param>
        /// <returns>The next iteration of the algorithm.</returns>
        internal ResultObject Iterate(Site[] sites, int width, int height)
        {
            var events = new MaxHeap<Evnt>(); // ongelma jos tulee kaksi eventtiä samalla y-koordinaatilla
            VoronoiList<BeachObj> beachline = new VoronoiList<BeachObj>();

            VoronoiList<Edge> finishedEdges = new VoronoiList<Edge>();

            VoronoiList<EvntCircle> oldCircleEvents = new VoronoiList<EvntCircle>();

            // Fill the event queue with site events for each input site.
            // -order by y-coordinate of the site
            foreach (Site test in sites)
            {
                events.Add(new EvntSite(test));
            }

            return this.Iterate(events, finishedEdges, beachline, oldCircleEvents, width, height);
        }

        /// <summary>
        /// Returns the next iteration of the algorithm.
        /// </summary>
        /// <param name="events">Upcoming site and circle events.</param>
        /// <param name="finishedEdges">Finished edges.</param>
        /// <param name="beachline">The beachline.</param>
        /// <param name="oldCircleEvents">Old circle events that have been either discarded or processed.</param>
        /// <param name="width">Width of canvas.</param>
        /// <param name="height">Height of canvas.</param>
        /// <returns>The next iteration of the algorithm.</returns>
        internal ResultObject Iterate(MaxHeap<Evnt> events, VoronoiList<Edge> finishedEdges, VoronoiList<BeachObj> beachline, VoronoiList<EvntCircle> oldCircleEvents, int width, int height)
        {
            // Fill the event queue with site events for each input site.
            // -order by y-coordinate of the site
            if (beachline.Count == 0)
            {
                EvntSite first = (EvntSite)events.PeakMax();

                beachline.Add(new BeachArc(first.Site));

                events.PopMax();

                return new ResultObject(events, finishedEdges, beachline, oldCircleEvents, width, height);
            }

            // if all the events are done, there is just the last trimming left to do
            else if (events.Count == 0)
            {
                return this.TrimLastHalfEdges(events, finishedEdges, beachline, oldCircleEvents, width, height);
            }

            // While the event queue still has items in it:
            Evnt next = events.PeakMax();
            double currentEventPosY = next.YToHappen;

            // tallennetaan oikea key jotta saadaan tämä event poistettua (TODO keksi tapa jossa ei synny clasheja)
            // double nextKey = events.Keys[0];
            double nextKey = next.YToHappen;

            // If the next event on the queue is a site event:
            if (next.IsSiteEvent)
            {
                EvntSite currentSiteEvent = (EvntSite)next;

                // Add the new site to the beachline
                // -search for arc above the point
                // filtteröidään pelkät kaaret listalle
                VoronoiList<BeachArc> arcs = beachline.GetElementsOfTypeBeachArc();

                Tuple<BeachArc, double> aboves = this.GetArcAbove(arcs, currentSiteEvent.Site);
                BeachArc above = aboves.Item1;
                double bestDistance = aboves.Item2;

                int indexOfBest = beachline.IndexOf(above);

                // sopivaan indeksiin:/beachline.Add(new BeachArc(se.s));
                // -remove the arc right above it
                beachline.Remove(above);

                // -add new arc
                BeachArc newbornArc = new BeachArc(currentSiteEvent.Site);

                // -add two copies of the removed arc
                BeachArc newLeftSideArc = new BeachArc(above.Homesite, limLeft: above.LeftLimit, limRight: currentSiteEvent.X);
                BeachArc newRightSideArc = new BeachArc(above.Homesite, limLeft: currentSiteEvent.X, limRight: above.RightLimit);

                // -add two half-edges starting from the point in the original arc right above the new site
                // -starting point is straight above the new site, using the shortest distance to the parable above
                // -suuntavektori saadaan pisteiden puolivälin avulla (x- ja y-koordinaattien on oltava eri)
                double startingX = currentSiteEvent.X;
                double startingY = currentSiteEvent.Y + bestDistance;

                // midway between new and old focus point
                double midwayX = (above.HomeX + currentSiteEvent.X) / 2;
                double midwayY = (above.HomeY + currentSiteEvent.Y) / 2;

                // suuntavektorit uusille kaarille
                double directionLeftX = 0;
                double directionLeftY = 0;
                double directionRightX = 0;
                double directionRightY = 0;

                // keskikohta löytyi vasemmalta
                if (midwayX < startingX)
                {
                    directionLeftX = midwayX;
                    directionLeftY = midwayY;
                    directionRightX = startingX + (startingX - midwayX);
                    directionRightY = startingY + (startingY - midwayY);
                }

                // jos ei löydy vasemmalta, löytyy varmasti oikealta
                else
                {
                    directionRightX = midwayX;
                    directionRightY = midwayY;
                    directionLeftX = startingX + (startingX - midwayX);
                    directionLeftY = startingY + (startingY - midwayY);
                }

                BeachHalfEdge edgeToLeft = new BeachHalfEdge(startingX, startingY, directionLeftX, directionLeftY);
                BeachHalfEdge edgeToRight = new BeachHalfEdge(startingX, startingY, directionRightX, directionRightY);

                beachline.InsertRange(
                    indexOfBest,
                    new BeachObj[] { newLeftSideArc, edgeToLeft, newbornArc, edgeToRight, newRightSideArc });

                // -add possible circle events to the event queue
                // -check if the lines next to the new arcs are going to intersect
                var noobs = new BeachArc[] { newLeftSideArc, newRightSideArc };
                foreach (BeachArc newarc in noobs)
                {
                    EvntCircle newevent = this.TryAddCircleEvent(newarc, beachline, currentEventPosY);
                    if (newevent != null)
                    {
                        events.Add(newevent);
                    }
                }
            }

            // Otherwise it must be an edge-intersection (circle) event:
            else
            {
                EvntCircle currentCircEv = (EvntCircle)next;

                // check validity TODO tässä voi piillä bugi lähtöisin siitä kun kaaria korvataan uusilla
                BeachArc disappArc = currentCircEv.DisappearingArc;
                int? indexOnBeach = beachline.IndexOf(disappArc);
                if (indexOnBeach != -1 && indexOnBeach > 0 && indexOnBeach < beachline.Count - 1 && beachline[(int)indexOnBeach - 1] == currentCircEv.LeftEdge && beachline[(int)indexOnBeach + 1] == currentCircEv.RightEdge)
                {
                    // Remove the squeezed cell from the beachline
                    // -remove arc
                    beachline.Remove(disappArc);

                    // -add new single half-edge starting from intersection point
                    BeachArc futureLeft = (BeachArc)beachline[(int)indexOnBeach - 2];
                    BeachArc futureRight = (BeachArc)beachline[(int)indexOnBeach + 1];

                    BeachHalfEdge leftEdge = (BeachHalfEdge)beachline[(int)indexOnBeach - 1];
                    BeachHalfEdge rightEdge = (BeachHalfEdge)beachline[(int)indexOnBeach];
                    Point midpoint = Point.GetPointAtMidway(leftEdge.StartingPoint, rightEdge.StartingPoint);

                    double xDirPoint = (futureLeft.HomeX + futureRight.HomeX) / 2;
                    double yDirPoint = (futureLeft.HomeY + futureRight.HomeY) / 2;

                    BeachHalfEdge singleHalfEdge = new BeachHalfEdge(currentCircEv.CircleCentre.x, currentCircEv.CircleCentre.y, xDirPoint, yDirPoint);
                    BeachHalfEdge mirroredEdge = BeachHalfEdge.MirrorKeepStartingPoint(singleHalfEdge);

                    if(Point.DistanceBetweenPoints(midpoint, singleHalfEdge.DirectionPoint) < Point.DistanceBetweenPoints(midpoint, mirroredEdge.DirectionPoint))
                    {
                        singleHalfEdge = mirroredEdge;
                    }

                    // Mirrored to the opposite direction if pointing upwards WRONG!!
                    /*if (singleHalfEdge.startingY < singleHalfEdge.directionY)
                    {
                        singleHalfEdge = BeachHalfEdge.MirrorKeepStartingPoint(singleHalfEdge);
                    }*/

                    beachline.Insert((int)indexOnBeach, singleHalfEdge);

                    // -the half-edges become finished edges, remove from beachline
                    beachline.Remove(currentCircEv.LeftEdge);
                    beachline.Remove(currentCircEv.RightEdge);

                    // add finished edges
                    finishedEdges.Add(new Edge(new Point(currentCircEv.LeftEdge.StartingX, currentCircEv.LeftEdge.StartingY), currentCircEv.CircleCentre));
                    finishedEdges.Add(new Edge(new Point(currentCircEv.RightEdge.StartingX, currentCircEv.RightEdge.StartingY), currentCircEv.CircleCentre));

                    // -check both arcs for new future intersections
                    BeachArc[] noobs = new BeachArc[] { futureLeft, futureRight };
                    foreach (BeachArc newarc in noobs)
                    {
                        EvntCircle newevent = this.TryAddCircleEvent(newarc, beachline, currentEventPosY);
                        if (newevent != null)
                        {
                            events.Add(newevent);
                        }
                    }
                }

                oldCircleEvents.Add(currentCircEv);
            }

            var gone = events.PopMax();
            if (gone.YToHappen != nextKey)
            {
                throw new Exception("key mismatch");
            }

            // events.Remove(nextKey);
            // Cleanup any remaining intermediate state
            // -remaining collisions must only have one arc in between

            // Remove the arcs and edges that are left out of scope (outside canvas)
            var arcAtLefttEdge = this.GetArcAbove(beachline.GetElementsOfTypeBeachArc(), new Site(0, currentEventPosY));
            int indexAtBeachL = beachline.IndexOf(arcAtLefttEdge.Item1);
            var arcAtRightEdge = this.GetArcAbove(beachline.GetElementsOfTypeBeachArc(), new Site(width, currentEventPosY));
            int indexAtBeachR = beachline.IndexOf(arcAtRightEdge.Item1);

            VoronoiList<BeachObj> cleanedbeach = new VoronoiList<BeachObj>();

            for (int i = 0; i < beachline.Count; i++)
            {
                if (i >= indexAtBeachL && i <= indexAtBeachR)
                {
                    cleanedbeach.Add(beachline[i]);
                }
                else
                {
                    var thing = beachline[i];
                    if (thing is BeachHalfEdge)
                    {
                        BeachHalfEdge he = thing as BeachHalfEdge;
                        if (he.PointingLeft)
                        {
                            BeachHalfEdge leftWall = new BeachHalfEdge(0, 0, 0, height);
                            Point isct = new Point(he, leftWall);
                            if (isct.y <= height && isct.y >= 0)
                            {
                                finishedEdges.Add(new Edge(new Point(he.StartingX, he.StartingY), isct));
                            }
                        }

                        if (he.PointingRight)
                        {
                            BeachHalfEdge rightWall = new BeachHalfEdge(width, 0, width, height);
                            Point isct = new Point(he, rightWall);
                            if (isct.y <= height && isct.y >= 0)
                            {
                                finishedEdges.Add(new Edge(new Point(he.StartingX, he.StartingY), isct));
                            }
                        }

                        if (he.PointingUp)
                        {
                            BeachHalfEdge ceiling = new BeachHalfEdge(0, height, width, height);
                            Point isct = new Point(he, ceiling);
                            if (isct.x <= width && isct.x >= 0)
                            {
                                finishedEdges.Add(new Edge(new Point(he.StartingX, he.StartingY), isct));
                            }
                        }

                        if (he.PointingDown)
                        {
                            BeachHalfEdge floor = new BeachHalfEdge(0, 0, width, 0);
                            Point isct = new Point(he, floor);
                            if (isct.x <= width && isct.x >= 0)
                            {
                                finishedEdges.Add(new Edge(new Point(he.StartingX, he.StartingY), isct));
                            }
                        }
                    }
                }
            }

            beachline = cleanedbeach;

            return new ResultObject(events, finishedEdges, beachline, oldCircleEvents, width, height);
        }

        private ResultObject TrimLastHalfEdges(MaxHeap<Evnt> events, VoronoiList<Edge> finishedEdges, VoronoiList<BeachObj> beachline, VoronoiList<EvntCircle> oldCircleEvents, int width, int height)
        {
            // Halfedges are added to the finished pool for trimming
            var halfEdges = beachline.GetElementsOfTypeBeachHalfEdge();
            for (int i = 0; i < halfEdges.Count; i++)
            {
                var he = halfEdges[i];
                finishedEdges.Add(new Edge(new Point(he.StartingX, he.StartingY), new Point(he.DirectionX, he.DirectionY)));
                beachline.Remove(he);
            }

            Dictionary<Point, int> count = new Dictionary<Point, int>();

            for (int j = 0; j < finishedEdges.Count; j++)
            {
                Edge edge = finishedEdges[j];

                if (count.ContainsKey(edge.StartingPoint))
                {
                    count[edge.StartingPoint] = count[edge.StartingPoint] + 1;
                }
                else
                {
                    count.Add(edge.StartingPoint, 1);
                }

                if (count.ContainsKey(edge.EndingPoint))
                {
                    count[edge.EndingPoint] = count[edge.EndingPoint] + 1;
                }
                else
                {
                    count.Add(edge.EndingPoint, 1);
                }
            }

            // edges with one endpoint that is not shared with others (not created by a circle event)
            VoronoiList<Edge> loners = new VoronoiList<Edge>();
            VoronoiList<Edge> toBeDeleted = new VoronoiList<Edge>();
            for (int j = 0; j < finishedEdges.Count; j++)
            {
                Edge edge = finishedEdges[j];
                if (edge.BothEndpointsOutsideMap(width, height))
                {
                    // edge is totally out of scope, it will be removed from the graph
                    toBeDeleted.Add(edge);
                    continue;
                }

                if (count[edge.EndingPoint] == 1)
                {
                    loners.Add(edge);

                    // let's remove the original anyway because a new edge will be added in its place
                    toBeDeleted.Add(edge);
                }
                else if (count[edge.StartingPoint] == 1)
                {
                    loners.Add(new Edge(edge.EndingPoint, edge.StartingPoint));

                    // let's remove the original anyway because a new edge will be added in its place
                    toBeDeleted.Add(edge);
                }

                // if both ends are connected but one falls outside map, the edge needs trimming
                else if (!edge.StartingPoint.OnMap(width, height))
                {
                    loners.Add(new Edge(edge.EndingPoint, edge.StartingPoint));
                    toBeDeleted.Add(edge);
                }
                else if (!edge.EndingPoint.OnMap(width, height))
                {
                    loners.Add(edge);
                    toBeDeleted.Add(edge);
                }
            }

            for (int j = 0; j < loners.Count; j++)
            {
                Edge he = loners[j];

                if (he.PointingLeft)
                {
                    BeachHalfEdge leftWall = new BeachHalfEdge(0, 0, 0, height);
                    Point isct = new Point(he, leftWall);
                    if (isct.y <= height && isct.y >= 0)
                    {
                        finishedEdges.Add(new Edge(new Point(he.StartingPoint.x, he.StartingPoint.y), isct));
                        continue;
                    }
                }

                if (he.PointingRight)
                {
                    BeachHalfEdge rightWall = new BeachHalfEdge(width, 0, width, height);
                    Point isct = new Point(he, rightWall);
                    if (isct.y <= height && isct.y >= 0)
                    {
                        finishedEdges.Add(new Edge(new Point(he.StartingPoint.x, he.StartingPoint.y), isct));
                        continue;
                    }
                }

                if (he.PointingUp)
                {
                    BeachHalfEdge ceiling = new BeachHalfEdge(0, height, width, height);
                    Point isct = new Point(he, ceiling);
                    if (isct.x <= width && isct.x >= 0)
                    {
                        finishedEdges.Add(new Edge(new Point(he.StartingPoint.x, he.StartingPoint.y), isct));
                        continue;
                    }
                }

                if (he.PointingDown)
                {
                    BeachHalfEdge floor = new BeachHalfEdge(0, 0, width, 0);
                    Point isct = new Point(he, floor);
                    if (isct.x <= width && isct.x >= 0)
                    {
                        finishedEdges.Add(new Edge(new Point(he.StartingPoint.x, he.StartingPoint.y), isct));
                        continue;
                    }
                }
            }

            for (int j = 0; j < toBeDeleted.Count; j++)
            {
                Edge os = toBeDeleted[j];

                finishedEdges.Remove(os);
            }

            return new ResultObject(events, finishedEdges, beachline, oldCircleEvents, width, height, ready: true);
        }

        private Tuple<BeachArc, double> GetArcAbove(VoronoiList<BeachArc> arcs, Site site)
        {
            double bestDistance = -1;
            BeachArc above = null;

            if (double.IsNaN(bestDistance))
            {
                throw new Exception("Etäisyyden laskemisessa virhe");
            }

            for (int j = 0; j < arcs.Count; j++)
            {
                BeachArc arc = arcs[j];

                // erotetaan identtiset kaaret toisistaan
                if (arc.LeftLimit > site.X || arc.RightLimit < site.X)
                {
                    continue;
                }
                else
                {
                    double distance = arc.DistFromDirectrixX(site);
                    if (bestDistance == -1 || distance < bestDistance)
                    {
                        bestDistance = distance;
                        above = arc;
                    }
                }
            }

            return new Tuple<BeachArc, double>(above, bestDistance);
        }

        private EvntCircle TryAddCircleEvent(BeachArc newarc, VoronoiList<BeachObj> beachline, double directrixY)
        {
            EvntCircle res = null;
            int noobindex = beachline.IndexOf(newarc);

            if (noobindex - 1 >= 0 && noobindex + 1 <= beachline.Count - 1
                && beachline[noobindex - 1].GetType().Equals(typeof(BeachHalfEdge))
                && beachline[noobindex + 1].GetType().Equals(typeof(BeachHalfEdge))
               && !(((BeachHalfEdge)beachline[noobindex - 1]).PointingLeft && ((BeachHalfEdge)beachline[noobindex + 1]).PointingRight))
            {
                BeachHalfEdge leftEdge = (BeachHalfEdge)beachline[noobindex - 1];
                BeachHalfEdge rightEdge = (BeachHalfEdge)beachline[noobindex + 1];
                Point intersection = new Point(leftEdge, rightEdge);

                VoronoiList<BeachArc> arcs = beachline.GetElementsOfTypeBeachArc();

                var aboves = this.GetArcAbove(arcs, new Site(intersection.x, directrixY));

                // -if yes, add circle event to queue
                // -y-coordinate of event (sweepline location) is point of intersection minus distance to endpoint
                // tsekataan että löytyy "tulevaisuudesta" (tämä lienee turha, tarkistaa siis että viivat kohtaavat paraabelin polttopisteen alapuolella)
                if (intersection.y < directrixY + aboves.Item2)
                {
                    // pisteen etäisyys focus pointista on sama kuin pisteen etäisyys swipelinesta eventin aikana
                    double distFromFocus = Math.Sqrt(Math.Pow(newarc.HomeX - intersection.x, 2) + Math.Pow(newarc.HomeY - intersection.y, 2));
                    var circleEvent = new EvntCircle(intersection.y - distFromFocus, newarc, leftEdge, rightEdge, intersection);

                    res = circleEvent;
                }
            }

            return res;
        }
    }
}
