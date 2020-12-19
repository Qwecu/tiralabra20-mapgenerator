namespace Mapseesharp
{
    public class Program
    {
        private VoronoiCalculator vCalc;

        public ResultObject Calculate(Site[] sites, int width, int height)
        {
            vCalc = new VoronoiCalculator();
            return vCalc.Setup(sites, width, height);
        }

        public ResultObject Calculate(ResultObject res)
        {
            vCalc = new VoronoiCalculator();
            return vCalc.Iterate(res.Events, res.FinishedEdges, res.Beachline, res.OldCircleEvents, res.Width, res.Height);
        }

        static void Main(string[] args)
        {
            
        }
    }
}
