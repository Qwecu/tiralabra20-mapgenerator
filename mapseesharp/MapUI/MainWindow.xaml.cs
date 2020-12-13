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

        Site[] inputSites = new Site[] {
                new Site(12,36),
                new Site(172,360),
                new Site(302,196),
                new Site(212,100),
                new Site(92,252),
                new Site(152,76),
                new Site(124,4),
                new Site(248,284)
            };

        Site[] inputSites5 = new Site[] {
        new Site(110, 162),
        new Site(331, 200),
        new Site(146, 266)};

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
            myLine.X1 = p.StartingPoint.x + xSiirto;
            myLine.Y1 = p.StartingPoint.y + ySiirto;
            myLine.X2 = p.EndingPoint.x + xSiirto;
            myLine.Y2 = p.EndingPoint.y + ySiirto;
            myLine.StrokeThickness = 2;
            myGrid.Children.Add(myLine);
        }

        foreach (Edge p in canvasedges)
        {
            var myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.LightSeaGreen;
            myLine.X1 = p.StartingPoint.x + xSiirto;
            myLine.Y1 = p.StartingPoint.y + ySiirto;
            myLine.X2 = p.EndingPoint.x + xSiirto;
            myLine.Y2 = p.EndingPoint.y + ySiirto;
            myLine.StrokeThickness = 2;
            myGrid.Children.Add(myLine);
        }


        for (int i = 0; i < result.BeachHalfEdges.Count; i++)
        {
            BeachHalfEdge p = result.BeachHalfEdges[i];

            var myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Red;
            myLine.X1 = p.startingX + xSiirto;
            myLine.Y1 = p.startingY + ySiirto;
            myLine.X2 = p.directionX + xSiirto;
            myLine.Y2 = p.directionY + ySiirto;
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
            if (ce.CircleCentre.y >= ce.PosEventY)
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

        for (int i = 0; i < result.OldCircleEvents.Count; i++)
        {
            EvntCircle ce = result.OldCircleEvents[i];

            Canvas canvas = new Canvas();
            myGrid.Children.Add(canvas);

            var circle = new System.Windows.Shapes.Ellipse();
            circle.Stroke = System.Windows.Media.Brushes.LightGreen;
            if (ce.CircleCentre.y >= ce.PosEventY)
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
