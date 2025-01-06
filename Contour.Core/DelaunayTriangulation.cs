
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
            if (Points.Count < 3)
                throw new ArgumentException("At least 3 points are required for triangulation");

            var triangles = new List<Triangle>();

            // Create super triangle that contains all points
            double minX = Points.Min(p => p.x);
            double minY = Points.Min(p => p.y);
            double maxX = Points.Max(p => p.x);
            double maxY = Points.Max(p => p.y);

            double dx = maxX - minX;
            double dy = maxY - minY;
            double dmax = Math.Max(dx, dy);
            double midX = (minX + maxX) / 2;
            double midY = (minY + maxY) / 2;

            Point p1 = new Point(midX - 2 * dmax, midY - dmax);
            Point p2 = new Point(midX, midY + 2 * dmax);
            Point p3 = new Point(midX + 2 * dmax, midY - dmax);

            triangles.Add(new Triangle(p1, p2, p3));

            // Add points one by one
            foreach (var point in Points)
            {
                List<Triangle> badTriangles = new List<Triangle>();

                // Find all triangles where point lies inside circumcircle
                foreach (var triangle in triangles)
                {
                    if (triangle.Circumcircle().ContainsPoint(point))
                        badTriangles.Add(triangle);
                }

                List<Edge> polygon = new List<Edge>();

                // Find boundary of polygonal hole
                foreach (var triangle in badTriangles)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Edge edge = new Edge(triangle.Vertices[i],
                                           triangle.Vertices[(i + 1) % 3]);

                        if (!IsSharedEdge(edge, badTriangles))
                            polygon.Add(edge);
                    }
                }

                // Remove bad triangles
                foreach (var triangle in badTriangles)
                    triangles.Remove(triangle);

                // Re-triangulate the polygonal hole
                foreach (var edge in polygon)
                {
                    triangles.Add(new Triangle(edge.Start, edge.End, point));
                }
            }

            // Remove triangles that share vertices with super triangle
            triangles.RemoveAll(t =>
                t.Vertices.Any(v => v == p1 || v == p2 || v == p3));

            return triangles;
        }

        private bool IsSharedEdge(Edge edge, List<Triangle> triangles)
        {
            int count = 0;
            foreach (var triangle in triangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    Edge triEdge = new Edge(triangle.Vertices[i],
                                          triangle.Vertices[(i + 1) % 3]);
                    if (edge.Equals(triEdge))
                        count++;
                }
            }
            return count > 1;
        }
    }
}
