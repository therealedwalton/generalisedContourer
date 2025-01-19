using AngleSharp.Dom;
using Bunit;
using Contour.Client.Views;
using Contour.Core;

namespace Contour.Client.Test
{
    public class ContourViewTests : TestContext
    {
        [Fact]
        public void ShouldRenderSvg()
        {
            // Arrange
            var cut = RenderComponent<ContourView>();

            // Act
            var svg = cut.Find("svg");

            // Assert
            Assert.NotNull(svg);
        }

        [Fact]
        public void ShouldDisplayASeriesOfPoints()
        {
            // Arrange
            var points = new List<Point>()
            {
                new Point(5, 5),
                new Point(20, 14),
                new Point(78, 90),
                new Point(45, 64)
            };

            var cut = RenderComponent<ContourView>(parameters => parameters.Add(p => p.Points, points));

            // Act
            var circles = cut.FindAll("circle");

            // Assert
            Assert.Equal(points.Count, circles.Count);

            for (int i = 0; i < points.Count; i++) 
            {
                Assert.Equal(points[i].x, double.Parse(circles[i].GetAttribute("cx")));
                Assert.Equal(points[i].y, double.Parse(circles[i].GetAttribute("cy")));
            }
        }

        [Fact]
        public void ShouldDisplayTriangulationOfPoints()
        {
            // Arrange
            var testPoints = new List<Point> { new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0), new Point(1.0, 0.0) };
            var triangles = new List<Triangle>
            {
                new Triangle(testPoints[0], testPoints[1], testPoints[3]),
                new Triangle(testPoints[1], testPoints[2], testPoints[3])
            };

            var cut = RenderComponent<ContourView>(parameters => parameters.Add(p => p.Points, testPoints));

            // Act
            var lines = cut.FindAll("line");

            // Assert
            Assert.Equal(triangles.Count * 3, lines.Count);

            for (int i = 0; i < triangles.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.Equal(triangles[i].Vertices[j].x, double.Parse(lines[i * 3 + j].GetAttribute("x1")));
                    Assert.Equal(triangles[i].Vertices[j].y, double.Parse(lines[i * 3 + j].GetAttribute("y1")));

                    var upperIndex = j + 1 > 2 ? 0 : j + 1;
                    Assert.Equal(triangles[i].Vertices[upperIndex].x, double.Parse(lines[i * 3 + j].GetAttribute("x2")));
                    Assert.Equal(triangles[i].Vertices[upperIndex].y, double.Parse(lines[i * 3 + j].GetAttribute("y2")));
                }
            }
        }

        [Fact]
        public void ShouldDisplayAsCartesianCoordinates()
        {
            //TODO
        }
    }
}