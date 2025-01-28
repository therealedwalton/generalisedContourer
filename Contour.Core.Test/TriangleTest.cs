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
        [MemberData(nameof(CircumcircleData))]
        public void CanCalculateCircumcircle(Triangle<Point> triangle, Circle expectedCircumcircle)
        {
            //Act
            var circumcircle = triangle.Circumcircle();

            //Assert
            Assert.Equal(expectedCircumcircle, circumcircle);
        }

        [Theory]
        [MemberData(nameof(EquivalentTriangles))]
        public void TrianglesExhibitValueEquality(Triangle<Point> firstTriangle, Triangle<Point> secondTriangle, bool expectedToMatch)
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
                    new Triangle<Point>(new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0)), 
                    new Triangle<Point>(new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0)), 
                    true 
                },
                new object[]
                {
                    new Triangle<Point>(new Point(0.0, 0.0), new Point(1.0, 1.0), new Point(1.0, 1.0)),
                    new Triangle<Point>(new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0)),
                    false
                }
            };

        public static IEnumerable<object[]> CircumcircleData =>
            new List<object[]>
            {
                new object[]
                {
                    new Triangle<Point>(new Point(0.0, 0.0), new Point(0.0, 1.0), new Point(1.0, 1.0)),
                    new Circle(new Point(0.5, 0.5), Math.Sqrt(2*Math.Pow(0.5,2)))
                },
                new object[]
                {
                    new Triangle<Point>(new Point(0.0, 0.0), new Point(0.0, 2.0), new Point(2.0, 0.0)),
                    new Circle(new Point(1.0, 1.0), Math.Sqrt(2))
                },
                new object[]
                {
                    new Triangle<Point>(new Point(0.0, 0.0), new Point(0.0, 0.0), new Point(0.0, 0.0)),
                    new Circle(new Point(0.0, 0.0), 0)
                },
            };
    }
}
