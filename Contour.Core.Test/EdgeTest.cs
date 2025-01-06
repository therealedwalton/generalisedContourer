using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core.Test
{
    public class EdgeTest
    {
        [Theory]
        [MemberData(nameof(EquivalentEdges))]
        public void EdgeExhibitValueEquality(Edge edge1, Edge edge2, bool expectedSame)
        {
            //Act
            bool equal = edge1.Equals(edge2);

            //Assert
            Assert.Equal(expectedSame, equal);
        }

        public static IEnumerable<object[]> EquivalentEdges =>
            new List<object[]>
            {
                new object[]
                {
                    new Edge(new Point(0.0, 0.0), new Point(1.0, 1.0)),
                    new Edge(new Point(0.0, 0.0), new Point(1.0, 1.0)),
                    true
                },
                new object[]
                {
                    new Edge(new Point(0.0, 0.0), new Point(1.0, 1.0)),
                    new Edge(new Point(1.0, 1.0), new Point(0.0, 0.0)),
                    true
                },
            };
    }
}
