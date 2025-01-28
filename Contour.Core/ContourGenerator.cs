
namespace Contour.Core
{
    public class ContourGenerator
    {
        public async Task<List<Edge>> GenerateContour(List<Triangle<ValuePoint>> triangles, double contourLevel)
        {
            var contourLines = new List<Edge>();

            foreach (var triangle in triangles)
            {
                var intersectionPoints = new List<ValuePoint>();

                CheckEdgeIntersection(triangle.Vertices[0], triangle.Vertices[1], contourLevel, intersectionPoints);
                CheckEdgeIntersection(triangle.Vertices[1], triangle.Vertices[2], contourLevel, intersectionPoints);
                CheckEdgeIntersection(triangle.Vertices[2], triangle.Vertices[0], contourLevel, intersectionPoints);

                if (intersectionPoints.Count == 2)
                {
                    var contourLine = new Edge(intersectionPoints[0], intersectionPoints[1]);
                    contourLines.Add(contourLine);
                }
            }

            return contourLines;
        }

        private void CheckEdgeIntersection(ValuePoint start, ValuePoint end, double level, List<ValuePoint> intersectionPoints)
        {
            if (LevelCrossesEdge(start, end, level))
            {
                var intersection = CalculateIntersection(start, end, level);

                intersectionPoints.Add(new ValuePoint(intersection.x, intersection.y, level));
            }
        }

        private static Point CalculateIntersection(ValuePoint start, ValuePoint end, double level)
        {
            double ratio = (level - start.z) / (end.z - start.z);
            var x = start.x + ratio * (end.x - start.x);
            var y = start.y + ratio * (end.y - start.y);

            return new Point(x, y);
        }

        private static bool LevelCrossesEdge(ValuePoint start, ValuePoint end, double level)
        {
            return (start.z <= level && end.z >= level) || (start.z >= level && end.z <= level);
        }
    }
}
