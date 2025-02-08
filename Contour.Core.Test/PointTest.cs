using Contour.Core.Primitives.Behaviours;
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

        [Fact]
        public void CanAddColourBehaviour()
        {
           //Arrange
            Point point = new Point(0.0, 0.0);
            string colour = "#ff0000";

            //Act
            point.AddBehaviour(new ColourBehaviour { Colour = colour });

            //Assert
            Assert.Equal(colour, point.Behaviour<ColourBehaviour>().Colour);
        }

        [Fact]
        public void DoesntRequireBehaviour()
        {
            //Arrange
            Point point = new Point(0.0, 0.0);

            //Assert
            Assert.False(point.HasBehaviour<ColourBehaviour>());
            Assert.Null(point.Behaviour<ColourBehaviour>()?.Colour);
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
