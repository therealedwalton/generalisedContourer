using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core.Test
{
    public class ContourGeneratorTest
    {
        [Fact]
        public async Task CreatesAContourLineAcrossSingleTriangle()
        {
            //Arrange
            var testPoints = new List<Triangle<ValuePoint>> { new Triangle<ValuePoint>(new ValuePoint(0.0, 0.0, 0), new ValuePoint(0.0, 1.0, 0), new ValuePoint(1.0, 1.0, 1)) };

            var generator = new ContourGenerator();

            //Act
            var contourLines = await generator.GenerateContour(testPoints, 0.5);

            //Assert
            Assert.Equal(new List<Edge> { new Edge(new Point(0.5, 1.0), new Point(0.5, 0.5)) }, contourLines);
        }

        [Fact]
        public async Task GeneratesNoLinesWhenAllValuesAboveContourLevel()
        {
            //Arrange
            var testPoints = new List<Triangle<ValuePoint>> { new Triangle<ValuePoint>(new ValuePoint(0.0, 0.0, 0), new ValuePoint(0.0, 1.0, 0), new ValuePoint(1.0, 1.0, 1)) };

            var generator = new ContourGenerator();

            //Act
            var contourLines = await generator.GenerateContour(testPoints, 2);

            //Assert
            Assert.Equal(new List<Edge> { }, contourLines);
        }
    }
}
