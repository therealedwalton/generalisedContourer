

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

        public static bool ContainsPoint(Circle circle, Point point)
        {
            return true;
        }
    }
}
