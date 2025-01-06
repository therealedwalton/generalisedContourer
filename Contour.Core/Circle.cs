

namespace Contour.Core
{
    public record Circle
    {
        public Point Center { get; }
        public double Radius { get; }

        public Circle(Point center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public bool ContainsPoint(Point point)
        {
            double distance = Math.Sqrt(Math.Pow(point.x - Center.x, 2) +
                                  Math.Pow(point.y - Center.y, 2));

            return distance <= Radius;
        }
    }
}
