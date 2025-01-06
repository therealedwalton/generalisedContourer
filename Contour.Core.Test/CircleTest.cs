using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core.Test
{
    public class CircleTest
    {
        [Fact]
        void CanDetermineIfContainsPoint()
        {
            //Arrange
            var circle = new Circle(new Point(1.0, 1.0), 1.0);
            var point = new Point(0.5, 0.5);

            //Act
            var isInCircle = Circle.ContainsPoint(circle, point);

            //Assert
            Assert.True(isInCircle);
        }
    }
}
