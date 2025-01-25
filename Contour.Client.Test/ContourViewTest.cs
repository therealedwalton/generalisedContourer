using AngleSharp.Dom;
using Bunit;
using Contour.Client.Components;
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
            var mappingY = cut.Instance.MappingY;

            // Act
            var circles = cut.FindAll("circle");

            // Assert
            Assert.Equal(points.Count, circles.Count);

            for (int i = 0; i < points.Count; i++) 
            {
                Assert.Equal(points[i].x, double.Parse(circles[i].GetAttribute("cx")));
                Assert.Equal(points[i].y, mappingY(double.Parse(circles[i].GetAttribute("cy"))));
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
            var mappingY = cut.Instance.MappingY;

            // Act
            var lines = cut.FindAll("line");

            // Assert
            Assert.Equal(triangles.Count * 3, lines.Count);
            AssertLinesMatchTriangles(triangles, lines, mappingY);
        }

        [Fact]
        public void ShouldDisplayAsCartesianCoordinates()
        {
            // Arrange
            var points = new List<Point>()
            {
                new Point(5, 5),
                new Point(0, 0),
                new Point(10, 10),
            };

            var cut = RenderComponent<ContourView>(parameters => parameters.Add(p => p.Points, points));

            // Act
            var circles = cut.FindAll("circle");

            // Assert
            Assert.True(double.Parse(circles.First().GetAttribute("cx")) < double.Parse(circles.Last().GetAttribute("cx")), "Higher x value did not get drawn as higher svg x position");
            Assert.True(double.Parse(circles.First().GetAttribute("cy")) > double.Parse(circles.Last().GetAttribute("cy")), "Higher y value did not get drawn as lower svg y position");

        }

        [Theory]
        [MemberData(nameof(ScaleTestPoints))]
        public void ShouldScaleViewToFitPoints(IEnumerable<Point> points)
        {
            // Arrange
            var cut = RenderComponent<ContourView>(parameters => parameters.Add(p => p.Points, points));
            var mappingY = cut.Instance.MappingY;

            // Act
            var svg = cut.Find("svg");

            // Assert
            (double x, double y, double width, double height) = SvgViewBox(svg.GetAttribute("viewBox"));

            AssertPointsFitInViewbox(points, mappingY, x, y, width, height);
        }

        private static void AssertPointsFitInViewbox(IEnumerable<Point> points, Func<double, double> mappingY, double x, double y, double width, double height)
        {
            var minX = points.Min(p => p.x);
            var minY = mappingY(points.Max(p => p.y));

            var pointsWidth = points.Max(p => p.x) - minX;
            var pointsHeight = mappingY(points.Min(p => p.y)) - minY;

            Assert.True(x <= minX, $"Viewbox x ({x}) was not less than the minimum x value ({minX})");
            Assert.True(y <= minY, $"Viewbox y ({y}) was not less than the minimum y value({minY})");

            Assert.True(x + width >= minX + pointsWidth, $"Viewbox extent ({x + width}) was not greater than the maximum x value ({minX + pointsWidth})");
            Assert.True(y + height >= minY + pointsHeight, $"Viewbox y ({y + height}) was not less than the maximum y value({minY + pointsHeight})");

            Assert.True(x + width < minX + pointsWidth * 2, $"Viewbox extent ({x + width}) was significantly greater than the maximum x value ({minX + pointsWidth})");
            Assert.True(y + height < minY + pointsHeight * 2, $"Viewbox y ({y + height}) was significantly less than the maximum y value({minY + pointsHeight})");
        }

        private static void AssertLinesMatchTriangles(List<Triangle> triangles, IRefreshableElementCollection<IElement> lines, Func<double, double> mappingY)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Assert.Equal(triangles[i].Vertices[j].x, double.Parse(lines[i * 3 + j].GetAttribute("x1")));
                    Assert.Equal(triangles[i].Vertices[j].y, mappingY(double.Parse(lines[i * 3 + j].GetAttribute("y1"))));

                    var upperIndex = j + 1 > 2 ? 0 : j + 1;
                    Assert.Equal(triangles[i].Vertices[upperIndex].x, double.Parse(lines[i * 3 + j].GetAttribute("x2")));
                    Assert.Equal(triangles[i].Vertices[upperIndex].y, mappingY(double.Parse(lines[i * 3 + j].GetAttribute("y2"))));
                }
            }
        }

        private static (double x, double y, double width, double height) SvgViewBox(string viewBox)
        {
            var viewBoxParts = viewBox.Split(' ');
            return (double.Parse(viewBoxParts[0]), double.Parse(viewBoxParts[1]), double.Parse(viewBoxParts[2]), double.Parse(viewBoxParts[3]));
        }

        public static IEnumerable<object[]> ScaleTestPoints =>
            new List<object[]>
            {
                new object[]
                {
                    new List<Point>
                    {
                        new Point(0.0, 0.0),
                        new Point(10.0, 0.0),
                        new Point(0.0, 10.0),
                    },
                },
                new object[]
                {
                    new List<Point>
                    {
                        new Point(0.0, 0.0),
                        new Point(1000.0, 0.0),
                        new Point(0.0, 1000.0),
                    }
                },
            };
    }
}