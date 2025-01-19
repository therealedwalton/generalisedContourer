using Contour.Core;
using System.Runtime.CompilerServices;

namespace Contour.Core.Test
{
    public class DelaunayTriangulationTest
    {
        [Fact]
        public async Task CreatesSingleTriangleAround3DistinctPoints()
        {
            //Arrange
            var testPoints = new List<Point> { new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0) };

            var triangulation = new DelaunayTriangulation(testPoints);

            //Act
            var triangles = await triangulation.Triangulate();

            //Assert
            Assert.Equal(new List<Triangle> { new Triangle(testPoints) }, triangles);
        }

        [Fact]
        public async Task CreatesTriangulationAround4DistinctPoints()
        {
            //Arrange
            var testPoints = new List<Point> { new Point(0.0, 0.0), new Point(0.0, 0.9), new Point(1.0, 1.0), new Point(1.1, 0.1) };

            var triangulation = new DelaunayTriangulation(testPoints);

            //Act
            var triangles = await triangulation.Triangulate();

            //Assert
            Assert.Equal(new List<Triangle> 
            { 
                new Triangle(testPoints[0], testPoints[1], testPoints[3]), 
                new Triangle(testPoints[1], testPoints[2], testPoints[3]) 
            }, 
            triangles);
        }

        [Fact]
        public async Task HandlesRegularGridOfPoints()
        {
            //Arrange
            var testPoints = new List<Point> { new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0), new Point(1.0, 0.0) };

            var triangulation = new DelaunayTriangulation(testPoints);

            //Act
            var triangles = await triangulation.Triangulate();

            //Assert
            Assert.Equal(new List<Triangle>
            {
                new Triangle(testPoints[0], testPoints[1], testPoints[3]),
                new Triangle(testPoints[1], testPoints[2], testPoints[3])
            },
            triangles);
        }
    }
}