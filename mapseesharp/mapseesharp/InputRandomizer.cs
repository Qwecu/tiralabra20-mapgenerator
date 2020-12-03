using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{
    public static class InputRandomizer
    {
        public static Site[] RandomInput(int sites, int width, int height)
        {
            HashSet<double> xCoordinates = new HashSet<double>();
            HashSet<double> yCoordinates = new HashSet<double>();

            Site[] output = new Site[sites];

            Random random = new Random();

            for(int i = 0; i < sites; i++)
            {
                double x = random.NextDouble() * width;
                double y = random.NextDouble() * height;

                while (xCoordinates.Contains(x))
                {
                    x = random.NextDouble() * width;
                }

                while (yCoordinates.Contains(x))
                {
                    y = random.NextDouble() * height;
                }
                xCoordinates.Add(x);
                yCoordinates.Add(y);

                output[i] = new Site(x, y);

            }

            return output;         


        }
    }
}
