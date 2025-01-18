using AngleSharp.Dom;
using Bunit;
using Contour.Client;
using Contour.Core;

namespace Contour.Client.Test
{
    public class IconComponentTests : TestContext
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
    }
}