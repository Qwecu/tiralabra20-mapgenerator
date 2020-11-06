using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{
    class BeachArc : BeachObj
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
        public double HomeX { get { return Homesite.x; } }
        public double HomeY { get { return Homesite.y; } }

        public double LeftLimit { get; set; }
        public double RightLimit { get; set; }

        //Paraabelin etäisyys annetusta pisteestä
        //https://jacquesheunis.com/post/fortunes-algorithm/
        internal double DistFromDirectrixX(Site newsite)
        {
            double yf = Homesite.y;
            double xf = Homesite.x;
            double yd = newsite.y;
            double x = newsite.x;
            return (1.0 / (2.0 * (yf - yd)))
                * (x - xf) * (x - xf)
                + ((yf + yd) / 2.0);
        }
    }
}
