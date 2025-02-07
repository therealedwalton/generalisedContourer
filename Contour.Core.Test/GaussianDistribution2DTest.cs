using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core.Test
{
    public class GaussianDistribution2DTest
    {
        [Theory]
        [InlineData(0, 0, 1, 1, 1)]
        [InlineData(5, -6, 100, 1, 1)]
        public async Task ReturnsAmplitudeAtCentre(double centreX, double centreY, double amplitude, double sigmaX, double sigmaY)
        {
            // Arrange
            var gaussian = new GaussianDistribution2D(new Point(centreX, centreY), amplitude, sigmaX, sigmaY);

            // Act
            var value = gaussian.Value(centreX, centreY);

            // Assert
            Assert.Equal(amplitude, value);
        }

        [Theory]
        [InlineData(-10, 0)]
        [InlineData(200, 0)]
        [InlineData(0, 36)]
        [InlineData(0, -2)]
        public async Task ReducesInAllDirectionsFromCentre(double offsetX, double offsetY)
        {
            // Arrange
            Point centre = new Point(25, 35);

            var gaussian = new GaussianDistribution2D(centre, 20, 10, 20);

            // Act
            var value = gaussian.Value(centre.x + offsetX, centre.y + offsetY);

            // Assert
            Assert.True(gaussian.Value(centre.x, centre.y) > value);
        }
    }
}
