using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapseesharp
{
    public class BeachArc : BeachObj
    {
       // private Site s;

        //Tarvittaessa annetaan rajat, joihin paraabeli rajautuu oikealla ja vasemmalla
        public BeachArc(Site s, double limLeft = Double.NegativeInfinity, double limRight = Double.PositiveInfinity)
        {
            this.Homesite = s;
            LeftLimit = limLeft;
            RightLimit = limRight;
        }

        public Site Homesite { get; set; }
        public double HomeX { get { return Homesite.X; } }
        public double HomeY { get { return Homesite.Y; } }

        public double LeftLimit { get; set; }
        public double RightLimit { get; set; }

        //Paraabelin etäisyys annetusta pisteestä swipelinella
        //kaava johdettu kauniisti sivulla https://jacquesheunis.com/post/fortunes-algorithm/
        internal double DistFromDirectrixX(Site newsite)
        {
            double yf = Homesite.Y;
            double xf = Homesite.X;
            double yd = newsite.Y;
            double x = newsite.X;
            double yCoordinate = (1.0 / (2.0 * (yf - yd)))
                * (x - xf) * (x - xf)
                + ((yf + yd) / 2.0);
            double distance = yCoordinate - yd;
            return distance;
        }

        public override string ToString()
        {
            return "BeachArc homesite (" + HomeX + "; " + HomeY + "), limits: Left " + LeftLimit + ", right " + RightLimit;
        }

        internal double DistFromDirectrixX(Point intersection)
        {
            double yf = Homesite.Y;
            double xf = Homesite.X;
            double yd = intersection.y;
            double x = intersection.x;
            double yCoordinate = (1.0 / (2.0 * (yf - yd)))
                * (x - xf) * (x - xf)
                + ((yf + yd) / 2.0);
            double distance = yCoordinate - yd;
            return distance;
        }
    }
}
