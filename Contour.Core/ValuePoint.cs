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
            this.z = z;
        }

        public double z { get; set; }

        public override bool Equals(object? obj)
        {
            var item = obj as ValuePoint;

            if (item == null)
            {
                return false;
            }

            return ApproximatelyEqual(z, item.z) && base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), z);
        }
    }
}
