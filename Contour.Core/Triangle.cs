

namespace Contour.Core
{
    public class Triangle<T> where T : Point
    {
        public List<T> Vertices { get; }

        public Triangle(List<T> vertices)
        {
            this.Vertices = vertices;
        }

        public Triangle(params T[] vertices)
        {
            this.Vertices = vertices.ToList();
        }

        public override bool Equals(object? obj)
        {
            var item = obj as Triangle<T>;

            if(item == null)
            {
                return false;
            }

            return Vertices[0].Equals(item.Vertices[0]) && Vertices[1].Equals(item.Vertices[1]) && Vertices[2].Equals(item.Vertices[2]);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                for (int i = 0; i < this.Vertices.Count; i++)
                {
                    hash = hash * 23 + Vertices[i].x.GetHashCode();
                    hash = hash * 23 + Vertices[i].y.GetHashCode();
                }
                return hash;
            }
        }

        public Circle Circumcircle()
        {
            double d = 2 * (Vertices[0].x * (Vertices[1].y - Vertices[2].y) +
                       Vertices[1].x * (Vertices[2].y - Vertices[0].y) +
                       Vertices[2].x * (Vertices[0].y - Vertices[1].y));

            double centerX = ((Vertices[0].x * Vertices[0].x + Vertices[0].y * Vertices[0].y) * (Vertices[1].y - Vertices[2].y) +
                             (Vertices[1].x * Vertices[1].x + Vertices[1].y * Vertices[1].y) * (Vertices[2].y - Vertices[0].y) +
                             (Vertices[2].x * Vertices[2].x + Vertices[2].y * Vertices[2].y) * (Vertices[0].y - Vertices[1].y)) / d;

            double centerY = ((Vertices[0].x * Vertices[0].x + Vertices[0].y * Vertices[0].y) * (Vertices[2].x - Vertices[1].x) +
                             (Vertices[1].x * Vertices[1].x + Vertices[1].y * Vertices[1].y) * (Vertices[0].x - Vertices[2].x) +
                             (Vertices[2].x * Vertices[2].x + Vertices[2].y * Vertices[2].y) * (Vertices[1].x - Vertices[0].x)) / d;

            if (double.IsNaN(centerX)) { centerX = Vertices[0].x; }
            if (double.IsNaN(centerY)) { centerY = Vertices[0].y; }

            Point center = new Point(centerX, centerY);

            double rSquared = Math.Pow(Vertices[0].x - centerX, 2) + Math.Pow(Vertices[0].y - centerY, 2);
            double radius = rSquared > 0 ? Math.Sqrt(rSquared) : 0;

            return new Circle(center, radius);
        }
    }

    public class Triangle2D : Triangle<Point> { }
}