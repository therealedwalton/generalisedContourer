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
            var points = new List<ValuePoint>()
            {
                new ValuePoint(5, 5, 0),
                new ValuePoint(20, 14, 1),
                new ValuePoint(78, 90, 2),
                new ValuePoint(45, 64, 1)
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ShouldDisplayTriangulationOfPoints(bool showTrianglulation)
        {
            // Arrange
            var testPoints = new List<ValuePoint> { new ValuePoint(0.0, 0.0, 1), new ValuePoint(0.0, 1.0, 1), new ValuePoint(1.0, 1.0, 1), new ValuePoint(1.0, 0.0, 1) };
            List<Triangle<Point>> triangles = new List<Triangle<Point>>();

            if(showTrianglulation)
            {
                triangles.Add(new Triangle<Point>(testPoints[0], testPoints[1], testPoints[3]));
                triangles.Add(new Triangle<Point>(testPoints[1], testPoints[2], testPoints[3]));
            }
            var plotSettings = new PlotSettings { ShowTriangulation = showTrianglulation };

            var cut = RenderComponent<ContourView>(parameters => parameters
                .Add(p => p.Points, testPoints)
                .Add(p => p.PlotSettings, plotSettings));

            var mappingY = cut.Instance.MappingY;

            // Act
            var lines = cut.FindAll(".tri-line");

            // Assert
            Assert.Equal(triangles.Count * 3, lines.Count);
            AssertLinesMatchTriangles(triangles, lines, mappingY);
        }

        [Fact]
        public void ShouldDisplayAsCartesianCoordinates()
        {
            // Arrange
            var points = new List<ValuePoint>()
            {
                new ValuePoint(5, 5, 1),
                new ValuePoint(0, 0, 0),
                new ValuePoint(10, 10, 1),
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

        [Fact]
        public void ShouldScaleElementsToFitView()
        {
            // Arrange
            var smallPlotPoints = CreatePointSet(10);
            var largePlotPoints = CreatePointSet(1000);

            var cut = RenderComponent<ContourView>(parameters => parameters
                .Add(p => p.Points, smallPlotPoints)
                .Add(p => p.PlotSettings, new PlotSettings { ShowTriangulation = true }));
            var mappingY = cut.Instance.MappingY;

            // Act
            var smallPlotCircleR = cut.Find("circle").GetAttribute("r");
            var smallPlotLineWidth = cut.Find("line").GetAttribute("stroke-width");

            cut.SetParametersAndRender(parameters => parameters.Add(p => p.Points, largePlotPoints));

            var largePlotCircle = cut.Find("circle");
            var largePlotLine = cut.Find("line");

            // Assert
            Assert.True(double.Parse(smallPlotCircleR) < double.Parse(largePlotCircle.GetAttribute("r")), "Circle radius did not increase for large plots");
            Assert.True(double.Parse(smallPlotLineWidth) < double.Parse(largePlotLine.GetAttribute("stroke-width")), "line width did not increase for large plots");
        }

        [Fact]
        public void ShouldDisplayASingleContour()
        {
            // Arrange
            var testPoints = new List<ValuePoint> { new ValuePoint(0.0, 0.0, 0), new ValuePoint(0.0, 1.0, 1), new ValuePoint(1.0, 1.0, 1) };
            var contourLevels = new List<ContourLevel> { new ContourLevel { Value = 0.5, Colour = "#ff0000" } };

            var cut = RenderComponent<ContourView>(parameters => parameters
                .Add(p => p.Points, testPoints)
                .Add(p => p.ContourLevels, contourLevels));
            var mappingY = cut.Instance.MappingY;

            // Act
            var contourLines = cut.FindAll(".contour-line");

            // Assert
            Assert.Equal(contourLevels.Count, contourLines.Count);
            AssertLinesMatch(new List<Edge> { new Edge(new ValuePoint(0.0, 0.5, 0.5), new ValuePoint(0.5, 0.5, 0.5)) }, contourLines, mappingY);
        }

        [Fact]
        public void ShouldDisplayMultipleContours()
        {
            // Arrange
            var testPoints = new List<ValuePoint> { new ValuePoint(0.0, 0.0, 0), new ValuePoint(0.0, 1.0, 1), new ValuePoint(1.0, 1.0, 1) };
            var contourLevels = new List<ContourLevel> { new ContourLevel { Value = 0.5, Colour = "#ff0000" }, new ContourLevel { Value = 0.75, Colour = "#00ff00" } };

            var cut = RenderComponent<ContourView>(parameters => parameters
                .Add(p => p.Points, testPoints)
                .Add(p => p.ContourLevels, contourLevels));
            var mappingY = cut.Instance.MappingY;

            // Act
            var contourLevelGroups = cut.FindAll(".contour-level-group").ToList();

            // Assert
            Assert.Equal(contourLevels.Count, contourLevelGroups.Count);
            
            for (int i = 0; i < contourLevels.Count; i++)
            {
                Assert.Equal(contourLevels[i].Colour, contourLevelGroups[i].QuerySelector(".contour-line").GetAttribute("stroke"));
            }
        }

        [Fact]
        public void ShouldClearTrianglesAndLinesWhenCleared()
        {
            // Arrange
            var testPoints = new List<ValuePoint> { new ValuePoint(0.0, 0.0, 0), new ValuePoint(0.0, 1.0, 1), new ValuePoint(1.0, 1.0, 1) };
            var contourLevels = new List<ContourLevel> { new ContourLevel { Value = 0.5, Colour = "#ff0000" }, new ContourLevel { Value = 0.75, Colour = "#00ff00" } };

            var cut = RenderComponent<ContourView>(parameters => parameters
                .Add(p => p.Points, testPoints)
                .Add(p => p.ContourLevels, contourLevels)
                .Add(p => p.PlotSettings, new PlotSettings() { ShowTriangulation = true }));
            var mappingY = cut.Instance.MappingY;

            // Act
            var initialContourLines = cut.FindAll(".contour-line"); 
            var initialTriangleLines = cut.FindAll(".tri-line");

            testPoints.Clear();
            cut.SetParametersAndRender(parameters => parameters.Add(p => p.Points, testPoints));

            // Assert
            Assert.True(initialContourLines.Count > 0);
            Assert.True(initialTriangleLines.Count > 0);

            Assert.Empty(cut.FindAll(".contour-line"));
            Assert.Empty(cut.FindAll(".tri-line"));
        }

        [Fact]
        public void ShouldNotTrianglateOrContourWihtoutEnoughPoints()
        {
            // Arrange
            var testPoints = new List<ValuePoint> { new ValuePoint(0.0, 0.0, 0), new ValuePoint(0.0, 1.0, 1) };
            var contourLevels = new List<ContourLevel> { new ContourLevel { Value = 0.5, Colour = "#ff0000" }, new ContourLevel { Value = 0.75, Colour = "#00ff00" } };

            // Act
            var cut = RenderComponent<ContourView>(parameters => parameters
                .Add(p => p.Points, testPoints)
                .Add(p => p.ContourLevels, contourLevels)
                .Add(p => p.PlotSettings, new PlotSettings() { ShowTriangulation = true }));
            var mappingY = cut.Instance.MappingY;

            // Assert
            Assert.Empty(cut.FindAll(".contour-line"));
            Assert.Empty(cut.FindAll(".tri-line"));
        }

        private static List<ValuePoint> CreatePointSet(double typicalValue)
        {
            return new List<ValuePoint>
            {
                new ValuePoint(0.0, 0.0, typicalValue),
                new ValuePoint(typicalValue, 0.0, 0),
                new ValuePoint(0.0, typicalValue, typicalValue),
            };
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

        private static void AssertLinesMatchTriangles(List<Triangle<Point>> triangles, IRefreshableElementCollection<IElement> lines, Func<double, double> mappingY)
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

        private static void AssertLinesMatch(IEnumerable<Edge> expected, IRefreshableElementCollection<IElement> actual, Func<double, double> mappingY)
        {
            for (int i = 0; i < expected.Count(); i++)
            {
                var expectedLine = expected.ElementAt(i);
                var actualLine = actual.ElementAt(i);

                Assert.Equal(expectedLine.Start.x, double.Parse(actualLine.GetAttribute("x1")));
                Assert.Equal(expectedLine.Start.y, mappingY(double.Parse(actualLine.GetAttribute("y1"))));
                Assert.Equal(expectedLine.End.x, double.Parse(actualLine.GetAttribute("x2")));
                Assert.Equal(expectedLine.End.y, mappingY(double.Parse(actualLine.GetAttribute("y2"))));
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
                    CreatePointSet(10)
                },
                new object[]
                {
                    CreatePointSet(1000)
                },
            };
    }
}