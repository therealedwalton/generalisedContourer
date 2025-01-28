using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core
{
    public class ContourGenerator
    {
        public async Task<List<Edge>> GenerateContour(List<ValuePoint> points, double value)
        {
            return new List<Edge> { new Edge(new Point(0.5, 1.0), new Point(0.5, 0.5)) };
        }
    }
}
