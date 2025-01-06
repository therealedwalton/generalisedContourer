using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core.Test
{
    public class CircleTest
    {
        [Theory]
        [MemberData(nameof(IsInsideData))]
        void CanDetermineIfContainsPoint(Circle circle, Point point, bool expectedInside)
        {
            //Act
            var isInCircle = circle.ContainsPoint(point);

            //Assert
            Assert.Equal(expectedInside, isInCircle);
        }

        public static IEnumerable<object[]> IsInsideData =>
            new List<object[]>
            {
                new object[]
                {
                    new Circle(new Point(1.0, 1.0), 1.0),
                    new Point(0.5, 0.5),
                    true
                },
                new object[]
                {
                    new Circle(new Point(1.0, 1.0), 1.0),
                    new Point(0.0, 0.0),
                    false
                },
            };
    }
}
