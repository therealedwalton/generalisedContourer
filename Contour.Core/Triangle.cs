
namespace Contour.Core.Test
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

            return this.GetHashCode() == item.GetHashCode();
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
    }
}