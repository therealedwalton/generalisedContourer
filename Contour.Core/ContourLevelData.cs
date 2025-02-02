using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core
{
    public class ContourLevelData
    {
        public ContourLevel Level { get; set; } = new ContourLevel();

        public List<Edge> Edges { get; set; } = new List<Edge>();
    }
}
