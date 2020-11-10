using mapseesharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int xSiirto = 200;
        int ySiirto = 200;

        int canvasWidth = 200;
        int canvasHeight = 200;

        int windowWidth = 700;
        int windowHeight = 700;

        double currentY = 0;

        ResultObject result;

        Site[] testsites4 = new Site[] {
                new Site(3,9),
                new Site(43,90),
                new Site(73,49),
                new Site(53,25),
                new Site(23,63),
                new Site(38,19),
                new Site(31,1),
                new Site(62,71)
            };

        Site[] testsites = new Site[] {
                new Site(6,18),
                new Site(86,180),
                new Site(146,98),
                new Site(106,50),
                new Site(46,126),
                new Site(76,38),
                new Site(62,2),
                new Site(124,142)
            };


        //TODO käännetään koordinaatisto oikein päin ihmiselle...
        /*
        private double getY(double y)
        {
            return 0.0;
        }

        private double getX(double x)
        {
            return 0.0;
        }*/

        private void DrawEverything()
        {
            //myGrid.Children.Clear();

            List<UIElement> badChildren = new List<UIElement>();

            foreach(UIElement b in myGrid.Children)
            {
                if(!b.GetType().Equals(typeof(Button)))
                {
                    badChildren.Add(b);                    
                }
            }

            foreach(UIElement b in badChildren)
            {
                myGrid.Children.Remove(b);
            }

            foreach(var evnt in result.Events.Values)
            {
                if (evnt.IsSiteEvent)
                {
                    var myLine = new Line();
                    myLine.Stroke = Brushes.LightGray;
                    var e = (EvntSite)evnt;
                    myLine.X1 = 0 + xSiirto;
                    myLine.Y1 = e.y + ySiirto;
                    myLine.X2 = canvasWidth + xSiirto;
                    myLine.Y2 = e.y + ySiirto;
                    myLine.StrokeThickness = 1;
                    myGrid.Children.Add(myLine);
                }
                else
                {
                    var myLine = new Line();
                    myLine.Stroke = Brushes.LightSalmon;
                    var e = (EvntCircle)evnt;
                    myLine.X1 = 0 + xSiirto;
                    myLine.Y1 = e.PosEventY + ySiirto;
                    myLine.X2 = canvasWidth + xSiirto;
                    myLine.Y2 = e.PosEventY + ySiirto;
                    myLine.StrokeThickness = 1;
                    myGrid.Children.Add(myLine);
                }
            }

            var myLinew = new Line();
            myLinew.Stroke = Brushes.LightGray;
            myLinew.X1 = 0 + xSiirto;
            myLinew.Y1 = currentY + ySiirto;
            myLinew.X2 = canvasWidth + xSiirto;
            myLinew.Y2 = currentY + ySiirto;
            myLinew.StrokeThickness = 3;
            myGrid.Children.Add(myLinew);

            foreach (Site site in testsites)
            {
                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.Black;
                myLine.X1 = site.x + xSiirto;
                myLine.Y1 = site.y + ySiirto;
                myLine.X2 = site.x + xSiirto + 1;
                myLine.Y2 = site.y + ySiirto + 1;
                //myLine.HorizontalAlignment = HorizontalAlignment.Left;
                //myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 2;
                myGrid.Children.Add(myLine);
            }

            foreach(var ba in result.BeachArcs)
            {
                for (int i = 0; i < canvasWidth; i++)
                    //for (int i = Math.Max(0, (int)ba.LeftLimit); i < Math.Min(canvasWidth, (int)ba.RightLimit); i++)
                {
                    var myLine = new Line();
                    myLine.Stroke = System.Windows.Media.Brushes.Silver;

                    double yf = ba.Homesite.y;
                    double xf = ba.Homesite.x;
                    double yd = currentY;
                    double x = i;
                    double yCoordinate = (1.0 / (2.0 * (yf - yd)))
                        * (x - xf) * (x - xf)
                        + ((yf + yd) / 2.0);
                    double distance = yCoordinate - yd;

                    if (double.IsInfinity(yCoordinate) || yf == yd) { continue; }

                    myLine.X1 = x + xSiirto;
                    myLine.Y1 = yCoordinate + ySiirto;
                    myLine.X2 = x + xSiirto + 1;
                    myLine.Y2 = yCoordinate + ySiirto + 1;
                    myLine.StrokeThickness = 1;
                    myGrid.Children.Add(myLine);
                }
            }

            List<Edge> canvasedges = new List<Edge>();

            canvasedges.Add(new Edge(new mapseesharp.Point(0, 0), new mapseesharp.Point(canvasWidth, 0)));
            canvasedges.Add(new Edge(new mapseesharp.Point(0, 0), new mapseesharp.Point(0, canvasHeight)));
            canvasedges.Add(new Edge(new mapseesharp.Point(canvasWidth, 0), new mapseesharp.Point(canvasWidth, canvasHeight)));
            canvasedges.Add(new Edge(new mapseesharp.Point(0, canvasHeight), new mapseesharp.Point(canvasWidth, canvasHeight)));


            foreach (Edge p in result.FinishedEdges)
            {
                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.LightBlue;
                myLine.X1 = p.StartingPoing.x + xSiirto;
                myLine.Y1 = p.StartingPoing.y + ySiirto;
                myLine.X2 = p.EndingPoint.x + xSiirto;
                myLine.Y2 = p.EndingPoint.y + ySiirto;
                myLine.StrokeThickness = 2;
                myGrid.Children.Add(myLine);
            }

            foreach (Edge p in canvasedges)
            {
                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.LightSeaGreen;
                myLine.X1 = p.StartingPoing.x + xSiirto;
                myLine.Y1 = p.StartingPoing.y + ySiirto;
                myLine.X2 = p.EndingPoint.x + xSiirto;
                myLine.Y2 = p.EndingPoint.y + ySiirto;
                myLine.StrokeThickness = 2;
                myGrid.Children.Add(myLine);
            }

            foreach (BeachHalfEdge p in result.BeachHalfEdges)
            {
                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.Red;
                myLine.X1 = p.startingX + xSiirto;
                myLine.Y1 = p.startingY + ySiirto;
                myLine.X2 = p.directionX + xSiirto;
                myLine.Y2 = p.directionY + ySiirto;
                myLine.StrokeThickness = 1;
                myGrid.Children.Add(myLine);
            }


            /*double[] testResult = new double[] {
                23, 83.9074074074074, 41.9042553191489, 69.9042553191489,
                62, 90, 41.9042553191489, 69.9042553191489,
                41.9042553191489, 69.9042553191489, 46.1279069767442, 49.3139534883721,
                73, 62.75, 46.1279069767442, 49.3139534883721,
                46.1279069767442, 49.3139534883721, 46.5243243243243, 50.7297297297297,
                53, 45.3333333333333, 46.5243243243243, 50.7297297297297,
                3, 39.7037037037037, 23.1193277310924, 32.2521008403361,
                46.5243243243243, 50.7297297297297, 23.1193277310924, 32.2521008403361,
                23.1193277310924, 32.2521008403361, -49.9999999999999, 260.75,   //näissä kolmessa jotain häikkää
                38, 40.75, -49.9999999999999, 260.75,
                -49.9999999999999, 260.75, 20.0357142857143, 15.625,
                31, 11.3611111111111, 20.0357142857143, 15.625,

                0,0,canvasWidth,0,
                0,0,0,canvasHeight,
                canvasWidth,0,canvasWidth,canvasHeight,
               0,canvasHeight,canvasWidth,canvasHeight

            };

            for (int i = 0; i < testResult.Length; i += 4) {
                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.MediumSlateBlue;
                myLine.X1 = testResult[i] + xSiirto;
                myLine.Y1 = testResult[i + 1] + ySiirto;
                myLine.X2 = testResult[i+2] + xSiirto;
                myLine.Y2 = testResult[i + 3] + ySiirto;
                //myLine.HorizontalAlignment = HorizontalAlignment.Left;
                //myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 1;
                myGrid.Children.Add(myLine);
            } */

            foreach (EvntCircle ce in result.OldCircleEvents)
            {
                Canvas canvas = new Canvas();
                myGrid.Children.Add(canvas);

                var circle = new System.Windows.Shapes.Ellipse();
                circle.Stroke = System.Windows.Media.Brushes.LightGreen;
                circle.Width = circle.Height = 2 * (ce.CircleCentre.y - ce.PosEventY);

                /*myLine.ali=
                myLine.Y1 = p.StartingPoing.y + ySiirto;
                myLine.X2 = p.EndingPoint.x + xSiirto;
                myLine.Y2 = p.EndingPoint.y + ySiirto;*/

                circle.StrokeThickness = 1;

                //myGrid.Children.Add(myLine);
                Canvas.SetLeft(circle, ce.CircleCentre.x - circle.Width / 2 + xSiirto);
                Canvas.SetTop(circle, ce.CircleCentre.y - circle.Height / 2 + ySiirto);

                canvas.Children.Add(circle);


                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.HotPink;
                myLine.X1 = ce.CircleCentre.x + xSiirto - 1;
                myLine.Y1 = ce.CircleCentre.y + ySiirto - 1;
                myLine.X2 = ce.CircleCentre.x + xSiirto + 1;
                myLine.Y2 = ce.CircleCentre.y + ySiirto + 1;
                //myLine.HorizontalAlignment = HorizontalAlignment.Left;
                //myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 3;
                myGrid.Children.Add(myLine);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            var prog = new mapseesharp.Program();
            currentY = testsites.OrderByDescending(x => x.y).Select(x => x.y).First();
            result = prog.Calculate(testsites);
            DrawEverything();

        }

        private void iterate_Click(object sender, RoutedEventArgs e)
        {
            if (result.Events.Count == 0) return;
            currentY = (result.Events[result.Events.Keys[0]]).YToHappen;
            result = new mapseesharp.Program().Calculate(result);
            DrawEverything();
        }
    }
}
