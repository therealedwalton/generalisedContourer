using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core
{
    public class ValuePoint : Point
    {
        public ValuePoint(double x, double y, double z) : base(x, y)
        {
        }

        public double z { get; set; }
    }
}
