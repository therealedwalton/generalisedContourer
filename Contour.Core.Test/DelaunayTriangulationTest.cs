using Contour.Core;

namespace Contour.Core.Test
{
    public class DelaunayTriangulationTest
    {
        [Fact]
        public void CreatesSingleTriangleAround3DistinctPoints()
        {
            //Arrange
            var testPoints = new List<Point> { new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0) };

            var triangulation = new DelaunayTriangulation(testPoints);

            //Act
            var triangles = triangulation.Triangulate();

            //Assert
            Assert.Equal(new List<Triangle> { new Triangle(testPoints) }, triangles);
        }
    }
}