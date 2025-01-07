﻿
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

            var superTriangle = CreateSuperTriangle();

            triangles.Add(superTriangle);

            // Add points one by one
            foreach (var point in Points)
            {
                List<Triangle> badTriangles = FindTrianglesWithPointInsideCircumcircle(triangles, point);

                List<Edge> polygon = FindPolygonHoleBoundary(badTriangles);

                RemoveBadTriangles(triangles, badTriangles);

                TriangulatePolygonHole(triangles, point, polygon);
            }

            RemoveVerticesOnSuperTriangle(triangles, superTriangle);

            return triangles;
        }

        private static void RemoveVerticesOnSuperTriangle(List<Triangle> triangles, Triangle superTriangle)
        {
            triangles.RemoveAll(t => t.Vertices.Any(v => v == superTriangle.Vertices[0] || v == superTriangle.Vertices[1] || v == superTriangle.Vertices[2]));
        }

        private static void TriangulatePolygonHole(List<Triangle> triangles, Point point, List<Edge> polygon)
        {
            foreach (var edge in polygon)
            {
                triangles.Add(new Triangle(edge.Start, edge.End, point));
            }
        }

        private static List<Triangle> FindTrianglesWithPointInsideCircumcircle(List<Triangle> triangles, Point point)
        {
            var badTriangles = new List<Triangle>();

            foreach (var triangle in triangles)
            {
                if (triangle.Circumcircle().ContainsPoint(point))
                {
                    badTriangles.Add(triangle);
                }
            }

            return badTriangles;
        }

        private List<Edge> FindPolygonHoleBoundary(List<Triangle> badTriangles)
        {
            var polygon = new List<Edge>();

            foreach (var triangle in badTriangles)
            {
                for (int i = 0; i < 3; i++)
                {
                    Edge edge = new Edge(triangle.Vertices[i], triangle.Vertices[(i + 1) % 3]);

                    if (!IsSharedEdge(edge, badTriangles))
                    { 
                        polygon.Add(edge);
                    }
                }
            }

            return polygon;
        }

        private static void RemoveBadTriangles(List<Triangle> triangles, List<Triangle> badTriangles)
        {
            foreach (var triangle in badTriangles)
            {
                triangles.Remove(triangle);
            }
        }

        private Triangle CreateSuperTriangle()
        {
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

            var p1 = new Point(midX - 2 * dmax, midY - dmax);
            var p2 = new Point(midX, midY + 2 * dmax);
            var p3 = new Point(midX + 2 * dmax, midY - dmax);

            var superTriangle = new Triangle(p1, p2, p3);

            return superTriangle;
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
