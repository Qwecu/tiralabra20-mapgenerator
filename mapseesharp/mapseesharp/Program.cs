using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{


    public class ReverseComparer : IComparer<double>
    {
        int IComparer<double>.Compare(double x, double y)
        {
            return -x.CompareTo(y);
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            //testataan ensin homma toimivaksi tällä setillä
            var testsites = new Site[] {
                new Site(3,9),
                new Site(43,90),
                new Site(73,49),
                new Site(53,25),
                new Site(23,63),
                new Site(38,19),
                new Site(31,1),
                new Site(62,71)
            };

            var r = new Site(5, 1);
            ///TODO: Make sure that the sites can't be too near each other


            SortedList<double, Evnt> events = new SortedList<double, Evnt>(new ReverseComparer());


            List<BeachObj> beachline = new List<BeachObj>();
            List<Edge> FinishedEdges = new List<Edge>();

            //Fill the event queue with site events for each input site.
            //	-order by y-coordinate of the site
            foreach (Site test in testsites) { events.Add(test.y, new EvntSite(test)); }

            EvntSite first = (EvntSite)events[events.Keys[0]];

            beachline.Add(new BeachArc(first.site));

            events.Remove(events.Keys[0]);



            //While the event queue still has items in it:
            while (events.Count > 0)
            {
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
                    BeachArc above = arcs[0];
                    double bestDistance = above.DistFromDirectrixX(currentSiteEvent.site);

                    if (double.IsNaN(bestDistance)) { throw new Exception("Etäisyyden laskemisessa virhe"); }

                    foreach (BeachArc arc in arcs)
                    {
                        //erotetaan identtiset kaaret toisistaan
                        if (arc.LeftLimit > currentSiteEvent.x || arc.RightLimit < currentSiteEvent.x) continue;
                        else
                        {
                            double distance = arc.DistFromDirectrixX(currentSiteEvent.site);
                            if (distance < bestDistance) { bestDistance = distance; above = arc; }
                        }
                    }

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
                        int noobindex = beachline.IndexOf(newarc);

                        if (noobindex - 1 >= 0 && noobindex + 1 <= beachline.Count - 1
                            && beachline[noobindex - 1].GetType().Equals(typeof(BeachHalfEdge))
                            && beachline[noobindex + 1].GetType().Equals(typeof(BeachHalfEdge))
                            && ((BeachHalfEdge)beachline[noobindex - 1]).PointingRight
                            && ((BeachHalfEdge)beachline[noobindex + 1]).PointingLeft
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
                                var circleEvent = new EvntCircle(newarc.HomeY - distFromFocus, newarc, leftEdge, rightEdge, intersection);
                                events.Add(circleEvent.PosEventY, circleEvent);
                            //}
                        }

                    }

                }

                //    Otherwise it must be an edge-intersection (circle) event:
                else
                {
                    EvntCircle currentCircEv = (EvntCircle)next;
                    //       check validity TODO tässä voi piillä bugi lähtöisin siitä kun kaaria korvataan uusilla
                    BeachArc disappArc = currentCircEv.DisappearingArc;
                    int? indexOnBeach = beachline.IndexOf(disappArc);
                    if (indexOnBeach != null && beachline[(int)indexOnBeach - 1] == currentCircEv.leftEdge && beachline[(int)indexOnBeach + 1] == currentCircEv.rightEdge)
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
                            int noobindex = beachline.IndexOf(newarc);

                            if (noobindex - 1 >= 0 && noobindex + 1 <= beachline.Count - 1
                                && beachline[noobindex - 1].GetType().Equals(typeof(BeachHalfEdge))
                                && beachline[noobindex + 1].GetType().Equals(typeof(BeachHalfEdge))
                                && ((BeachHalfEdge)beachline[noobindex - 1]).PointingRight
                                && ((BeachHalfEdge)beachline[noobindex + 1]).PointingLeft
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
                                var circleEvent = new EvntCircle(newarc.HomeY - distFromFocus, newarc, leftEdge, rightEdge, intersection);
                                events.Add(circleEvent.PosEventY, circleEvent);
                                //}
                            }

                        }
                    }
                }
                events.Remove(nextKey);
                //Cleanup any remaining intermediate state
                //	-remaining collisions must only have one arc in between
            }
            //print
            Console.Out.WriteLine("Valmis");
            foreach (Edge edge in FinishedEdges)
            {
                Console.Out.WriteLine(
                    edge.StartingPoing.x + " " +
                    edge.StartingPoing.y + " " +
                    edge.EndingPoint.x + " " +
                    edge.EndingPoint.y + " "
                    );
            }
            Console.ReadKey();
        }
    }
}
