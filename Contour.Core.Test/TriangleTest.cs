using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core.Test
{
    public class TriangleTest
    {
        [Theory]
        [MemberData(nameof(EquivalentTriangles))]
        public void TrianglesExhibitValueEquality(Triangle firstTriangle, Triangle secondTriangle, bool expectedToMatch)
        {
            //Act
            var same = firstTriangle.Equals(secondTriangle);

            //Assert
            Assert.Equal(expectedToMatch, same);
        }

        public static IEnumerable<object[]> EquivalentTriangles =>
            new List<object[]> 
            { 
                new object[] 
                { 
                    new Triangle(new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0)), 
                    new Triangle(new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0)), 
                    true 
                },
                new object[]
                {
                    new Triangle(new Point(0.0, 0.0), new Point(1.0, 1.0), new Point(1.0, 1.0)),
                    new Triangle(new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0)),
                    false
                }
            };
    }
}
