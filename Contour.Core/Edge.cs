using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contour.Core
{
    public class Edge
    {
        public Point Start { get; }
        public Point End { get; }

        public Edge(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge other)
            {
                return (Start.Equals(other.Start) && End.Equals(other.End)) ||
                       (Start.Equals(other.End) && End.Equals(other.Start));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Start.x, Start.y, End.x, End.y);
        }
    }
}
