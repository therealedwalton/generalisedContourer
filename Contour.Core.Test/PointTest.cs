using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core.Test
{
    public class PointTest
    {
        [Theory]
        [MemberData(nameof(EquivalentPoints))]
        public void PointsExhibitValueEquality(Point point1, Point point2, bool expectedSame)
        {
            //Act
            bool equal = point1.Equals(point2);

            //Assert
            Assert.Equal(expectedSame, equal);
        }

        public static IEnumerable<object[]> EquivalentPoints =>
            new List<object[]>
            {
                new object[]
                {
                    new Point(0.0, 0.0),
                    new Point(0.0, 0.0),
                    true
                },
                new object[]
                {
                    new Point(0.0, 0.0),
                    new Point(1.0, 0.0),
                    false
                },
                new object[]
                {
                    new Point(10.0, 9.9),
                    new Point(10.0 + 1E-16, 9.9),
                    true
                },
            };
    }
}
