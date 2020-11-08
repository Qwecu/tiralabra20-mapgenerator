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
        public MainWindow()
        {
            InitializeComponent();

            int xSiirto = 200;
            int ySiirto = 200;

            int canvasWidth = 100;
            int canvasHeight = 100;

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

            foreach(Site site in testsites)
            {
                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.Black;
                myLine.X1 = site.x + xSiirto;
                myLine.Y1 = site.y + ySiirto;
                myLine.X2 = site.x + xSiirto + 1;
                myLine.Y2 = site.y + ySiirto + 1;
                //myLine.HorizontalAlignment = HorizontalAlignment.Left;
                //myLine.VerticalAlignment = VerticalAlignment.Center;
                myLine.StrokeThickness = 1;
                myGrid.Children.Add(myLine);
            }

            var prog = new mapseesharp.Program();
            var res = prog.Calculate(testsites);
            res.Add(new Edge(new mapseesharp.Point(0, 0), new mapseesharp.Point(canvasWidth, 0)));
            res.Add(new Edge(new mapseesharp.Point(0, 0), new mapseesharp.Point(0, canvasHeight)));
            res.Add(new Edge(new mapseesharp.Point(canvasWidth, 0), new mapseesharp.Point(canvasWidth, canvasWidth)));
            res.Add(new Edge(new mapseesharp.Point(0, canvasHeight), new mapseesharp.Point(canvasWidth, canvasWidth)));

            foreach (Edge p in res)
            {
                var myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.MediumSlateBlue;
                myLine.X1 = p.StartingPoing.x + xSiirto;
                myLine.Y1 = p.StartingPoing.y + ySiirto;
                myLine.X2 = p.EndingPoint.x + xSiirto;
                myLine.Y2 = p.EndingPoint.y + ySiirto;
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
                23.1193277310924, 32.2521008403361, -49.9999999999999, 260.75,
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




        }
    }
}
