using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mapseesharp
{
    public class ResultObject
    {
        public List<BeachObj> beachline { get; set; }
        public List<Edge> finishedEdges { get; set; }

        public ResultObject(List<Edge> finishedEdges, List<BeachObj> beachline)
        {
            this.finishedEdges = finishedEdges;
            this.beachline = beachline;
        }
    }
}
