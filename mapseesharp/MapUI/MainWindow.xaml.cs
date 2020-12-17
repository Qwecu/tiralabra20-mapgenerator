using Mapseesharp;
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

        int canvasWidth = 400;
        int canvasHeight = 400;

        int windowWidth = 700;
        int windowHeight = 700;

        int siteAmount = 3;

        bool showCircles = true;

        bool drawParabolas = true;

        private bool randomStart = false;

        double currentY = 0;

        ResultObject result;

        Stack<ResultObject> history = new Stack<ResultObject>();



        Site[] inputSites5 = new Site[] {
        new Site(110, 162),
        new Site(331, 200),
        new Site(146, 266)};

        Site[] inputSitesq = new Site[] {
        new Site(328, 96),
        new Site(319, 258),
        new Site(395, 33)};

        Site[] inputSites9 = new Site[] { //slant upwards
        new Site(28, 96),
        new Site(19, 258),
        new Site(95, 33)};

        Site[] inputSites = new Site[] {
        new Site(393, 59),
        new Site(318, 378),
        new Site(234, 81) };

        Site[] inputSites85 = new Site[] {
                new Site(12,36),
                new Site(172,360),
                new Site(302,196),
                new Site(212,100),
                new Site(92,252),
                new Site(152,76),
                new Site(124,4),
                new Site(248,284)
            };

        Site[] inputSites80 = new Site[] {
        new Site( 132.703772807822, 216.510730337589),
        new Site( 117.885699364303, 165.98259106557),
        new Site( 144.231118515241, 33.8614152902092),
        new Site( 171.910081883851, 344.362142283638),
        new Site( 346.308117334874, 368.572620474069),
        new Site( 7.80726503944363, 289.410772868158),
        new Site( 372.202118286957, 168.460731473035),
        new Site( 253.085058859123, 339.887540200673),
        new Site( 323.285551705996, 333.510649732086),
        new Site( 227.683028498517, 43.5105510258631),
        new Site( 38.1046384750421, 202.494957578599),
        new Site( 14.2840713329073, 56.7734806131448),
        new Site( 235.257502196011, 132.350159684359),
        new Site( 207.98918819427, 312.921185005885),
        new Site( 378.95092255387, 62.4665446870339),
        new Site( 44.5098502768715, 330.032376353644),
        new Site( 282.089904827108, 147.24831774237),
        new Site( 391.87865051994, 144.603627987487),
        new Site( 268.787009580427, 319.066796972913),
        new Site( 49.0667248373231, 333.350667512673),
        new Site( 204.137528410245, 30.5357001864052),
        new Site( 107.098480363888, 339.326953114628),
        new Site( 25.1015488175217, 157.497397138503),
        new Site( 247.966045442953, 212.784526409947),
        new Site( 389.480560454298, 32.163867183106),
        new Site( 262.648179131862, 280.421244483637),
        new Site( 29.5274658266117, 315.386054997978),
        new Site( 297.2386816038, 330.208815229223),
        new Site( 202.226659004682, 61.1122187511587),
        new Site( 330.725088869559, 11.8809588308823) };

        private void DrawEverything()
        {
            //myGrid.Children.Clear();

            var eventsToDraw = result.Events.GetAllAsArray();

            List<UIElement> badChildren = new List<UIElement>();

            foreach (UIElement b in myGrid.Children)
            {
                if (!b.GetType().Equals(typeof(Button)))
                {
                    badChildren.Add(b);
                }
            }

            foreach (UIElement b in badChildren)
            {
                myGrid.Children.Remove(b);
            }

            foreach (var evnt in eventsToDraw)
            {
                if (evnt.IsSiteEvent)
                {
                    var myLine = new Line();
                    myLine.Stroke = Brushes.LightGray;
                    var e = (EvntSite)evnt;
                    myLine.X1 = 0 + xSiirto;
                    myLine.Y1 = e.Y + ySiirto;
                    myLine.X2 = canvasWidth + xSiirto;
                    myLine.Y2 = e.Y + ySiirto;
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



            if (drawParabolas)
            {

                for (int j = 0; j < result.BeachArcs.Count; j++)
                {
                    BeachArc ba = result.BeachArcs[j];


                    for (int i = 0; i < canvasWidth; i++)
                    //  for (int i = Math.Max(0, (int)ba.LeftLimit); i < Math.Min(canvasWidth, (int)ba.RightLimit); i++)
                    {
                        var myLine = new Line();
                        myLine.StrokeThickness = 1;
                        myLine.Stroke = System.Windows.Media.Brushes.Beige;
                        //myLine.Stroke = System.Windows.Media.Brushes.Silver;
                        if (i > ba.LeftLimit && i < ba.RightLimit) { myLine.Stroke = Brushes.Violet; myLine.StrokeThickness = 3; }

                        double yf = ba.Homesite.Y;
                        double xf = ba.Homesite.X;
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

                        myGrid.Children.Add(myLine);
                    }
                }
            }

            List<Edge> canvasedges = new List<Edge>();

            canvasedges.Add(new Edge(new Mapseesharp.Point(0, 0), new Mapseesharp.Point(canvasWidth, 0)));
            canvasedges.Add(new Edge(new Mapseesharp.Point(0, 0), new Mapseesharp.Point(0, canvasHeight)));
            canvasedges.Add(new Edge(new Mapseesharp.Point(canvasWidth, 0), new Mapseesharp.Point(canvasWidth, canvasHeight)));
            canvasedges.Add(new Edge(new Mapseesharp.Point(0, canvasHeight), new Mapseesharp.Point(canvasWidth, canvasHeight)));

            for (int i = 0; i < result.FinishedEdges.Count; i++)
            {
                Edge p = result.FinishedEdges[i];

                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.LightBlue;
                myLine.X1 = p.StartingPoint.X + xSiirto;
                myLine.Y1 = p.StartingPoint.Y + ySiirto;
                myLine.X2 = p.EndingPoint.X + xSiirto;
                myLine.Y2 = p.EndingPoint.Y + ySiirto;
                myLine.StrokeThickness = 2;
                myGrid.Children.Add(myLine);
            }

            foreach (Edge p in canvasedges)
            {
                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.LightSeaGreen;
                myLine.X1 = p.StartingPoint.X + xSiirto;
                myLine.Y1 = p.StartingPoint.Y + ySiirto;
                myLine.X2 = p.EndingPoint.X + xSiirto;
                myLine.Y2 = p.EndingPoint.Y + ySiirto;
                myLine.StrokeThickness = 2;
                myGrid.Children.Add(myLine);
            }


            for (int i = 0; i < result.BeachHalfEdges.Count; i++)
            {
                BeachHalfEdge p = result.BeachHalfEdges[i];

                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.Red;
                myLine.X1 = p.StartingX + xSiirto;
                myLine.Y1 = p.StartingY + ySiirto;
                myLine.X2 = p.DirectionX + xSiirto;
                myLine.Y2 = p.DirectionY + ySiirto;
                myLine.StrokeThickness = 1;
                myGrid.Children.Add(myLine);
            }

            if (showCircles)
            {
                DrawCircles(eventsToDraw);
            }

            foreach (Site site in inputSites)
            {
                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.Black;
                myLine.X1 = site.X + xSiirto;
                myLine.Y1 = site.Y + ySiirto;
                myLine.X2 = site.X + xSiirto + 1;
                myLine.Y2 = site.Y + ySiirto + 1;
                //myLine.HorizontalAlignment = HorizontalAlignment.Left;
                //myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 2;
                myGrid.Children.Add(myLine);
            }
        }

        private void DrawCircles(Evnt[] eventsToDraw)
        {
            foreach (EvntCircle ce in (eventsToDraw.Where(x => x.IsSiteEvent == false)).Select(x => (EvntCircle)x))
            {
                Canvas canvas = new Canvas();
                myGrid.Children.Add(canvas);

                var circle = new System.Windows.Shapes.Ellipse();
                circle.Stroke = System.Windows.Media.Brushes.Yellow;
                if (ce.CircleCentre.Y >= ce.PosEventY)
                    circle.Width = circle.Height = 2 * (ce.CircleCentre.Y - ce.PosEventY);

                /*myLine.ali=
                myLine.Y1 = p.StartingPoing.y + ySiirto;
                myLine.X2 = p.EndingPoint.x + xSiirto;
                myLine.Y2 = p.EndingPoint.y + ySiirto;*/

                circle.StrokeThickness = 1;

                //myGrid.Children.Add(myLine);
                Canvas.SetLeft(circle, ce.CircleCentre.X - circle.Width / 2 + xSiirto);
                Canvas.SetTop(circle, ce.CircleCentre.Y - circle.Height / 2 + ySiirto);

                canvas.Children.Add(circle);


                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.HotPink;
                myLine.X1 = ce.CircleCentre.X + xSiirto - 1;
                myLine.Y1 = ce.CircleCentre.Y + ySiirto - 1;
                myLine.X2 = ce.CircleCentre.X + xSiirto + 1;
                myLine.Y2 = ce.CircleCentre.Y + ySiirto + 1;
                //myLine.HorizontalAlignment = HorizontalAlignment.Left;
                //myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 3;
                myGrid.Children.Add(myLine);
            }

            for (int i = 0; i < result.OldCircleEvents.Count; i++)
            {
                EvntCircle ce = result.OldCircleEvents[i];

                Canvas canvas = new Canvas();
                myGrid.Children.Add(canvas);

                var circle = new System.Windows.Shapes.Ellipse();
                circle.Stroke = System.Windows.Media.Brushes.LightGreen;
                if (ce.CircleCentre.Y >= ce.PosEventY)
                    circle.Width = circle.Height = 2 * (ce.CircleCentre.Y - ce.PosEventY);

                /*myLine.ali=
                myLine.Y1 = p.StartingPoing.y + ySiirto;
                myLine.X2 = p.EndingPoint.x + xSiirto;
                myLine.Y2 = p.EndingPoint.y + ySiirto;*/

                circle.StrokeThickness = 1;

                //myGrid.Children.Add(myLine);
                Canvas.SetLeft(circle, ce.CircleCentre.X - circle.Width / 2 + xSiirto);
                Canvas.SetTop(circle, ce.CircleCentre.Y - circle.Height / 2 + ySiirto);

                canvas.Children.Add(circle);


                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.HotPink;
                myLine.X1 = ce.CircleCentre.X + xSiirto - 1;
                myLine.Y1 = ce.CircleCentre.Y + ySiirto - 1;
                myLine.X2 = ce.CircleCentre.X + xSiirto + 1;
                myLine.Y2 = ce.CircleCentre.Y + ySiirto + 1;
                //myLine.HorizontalAlignment = HorizontalAlignment.Left;
                //myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 3;
                myGrid.Children.Add(myLine);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            SetUp();

        }

        private void iterateResult()
        {
            if (result.Events.Count == 0 && result.Ready) return;
            if (result.Events.Count > 0)
            {
                currentY = result.Events.PeakMax().YToHappen;
            }
            if (result != null)
            {
                history.Push(result);
            }
            result = new Mapseesharp.Program().Calculate(result);
        }

        private void iterate_Click(object sender, RoutedEventArgs e)
        {
            iterateResult();
            DrawEverything();
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            SetUp();
        }

        private void SetUp()
        {
            var prog = new Mapseesharp.Program();

            if (randomStart) inputSites = InputRandomizer.RandomInput(siteAmount, canvasWidth, canvasHeight);

            currentY = inputSites.OrderByDescending(x => x.Y).Select(x => x.Y).First();
            result = prog.Calculate(inputSites, canvasWidth, canvasHeight);
            DrawEverything();
        }

        //TODO make deep copies for this to work
        private void back_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count == 0) return;
            var prev = history.Pop();
            result = prev;
            DrawEverything();
        }

        private void toggleCircles_Click(object sender, RoutedEventArgs e)
        {
            showCircles = !showCircles;
            DrawEverything();
        }

        private void toEnd_Click(object sender, RoutedEventArgs e)
        {
            while (result.Ready == false)
            {
                iterateResult();
            }
            DrawEverything();
        }

        private void showInput_Click(object sender, RoutedEventArgs e)
        {
            string message = "Input: \n";

            foreach (Site site in inputSites)
            {
                message += site + "\n";
            }
            Clipboard.SetText(message);

            MessageBox.Show(message);
        }

        private void showParabolas_Click(object sender, RoutedEventArgs e)
        {
            drawParabolas = !drawParabolas;
            DrawEverything();
        }
    }
}
