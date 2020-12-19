namespace Mapseesharp
{
    /// <summary>
    /// Program.
    /// </summary>
    public class Program
    {
        private VoronoiCalculator vCalc;

        /// <summary>
        /// Iterates the algorithm.
        /// </summary>
        /// <param name="sites">Sites.</param>
        /// <param name="width">Width of canvas.</param>
        /// <param name="height">Height of canvas.</param>
        /// <returns>Next iteration.</returns>
        public ResultObject Calculate(Site[] sites, int width, int height)
        {
            this.vCalc = new VoronoiCalculator();
            return this.vCalc.Setup(sites, width, height);
        }

        /// <summary>
        /// Iterates the algorithm.
        /// </summary>
        /// <param name="res">Next iteration object.</param>
        /// <returns>Next iteration.</returns>
        public ResultObject Calculate(ResultObject res)
        {
            this.vCalc = new VoronoiCalculator();
            return this.vCalc.Iterate(res.Events, res.FinishedEdges, res.Beachline, res.OldCircleEvents, res.Width, res.Height);
        }

        private static void Main(string[] args)
        {
        }
    }
}
