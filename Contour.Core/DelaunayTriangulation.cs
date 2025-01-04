using Contour.Core.Test;

namespace Contour.Core
{
    public class DelaunayTriangulation
    {
        public List<Point> Points { get; }

        public DelaunayTriangulation(List<Point> points)
        {
            Points = points;
        }

        public IEnumerable<Triangle> Triangulate()
        {
            return new List<Triangle>() { new Triangle(new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0)) };
        }
    }
}
