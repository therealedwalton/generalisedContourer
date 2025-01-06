namespace Contour.Core
{
    public class Point
    {
        public double x { get; set; } 
        public double y {get ;set; }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object? obj)
        {
            var item = obj as Point;

            if (item == null)
            {
                return false;
            }

            return ApproximatelyEqual(x, item.x) && ApproximatelyEqual(y, item.y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();

                return hash;
            }
        }

        private static bool ApproximatelyEqual(double one, double two)
        {
            return Math.Abs(one - two) < 1E-15;
        }
    }
}