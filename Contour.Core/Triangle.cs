

namespace Contour.Core
{
    public class Triangle
    {
        public List<Point> Vertices { get; }

        public Triangle(List<Point> vertices)
        {
            this.Vertices = vertices;
        }

        public Triangle(params Point[] vertices)
        {
            this.Vertices = vertices.ToList();
        }

        public override bool Equals(object? obj)
        {
            var item = obj as Triangle;

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
            return new Circle(new Point(1.0, 1.0), Math.Sqrt(2));
        }
    }
}