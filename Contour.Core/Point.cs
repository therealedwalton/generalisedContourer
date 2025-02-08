using Contour.Core.Primitives.Behaviours;

namespace Contour.Core
{
    public class Point
    {
        public double x { get; set; } 
        public double y {get ;set; }

        public List<IPointBehaviour> Behaviours { get; private set; } = new List<IPointBehaviour>();

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public void AddBehaviour(IPointBehaviour behaviour)
        {
            Behaviours.Add(behaviour);
        }

        public T Behaviour<T>() where T : IPointBehaviour
        {
            return Behaviours.OfType<T>().FirstOrDefault();
        }

        public bool HasBehaviour<T>() where T : IPointBehaviour
        {
            return Behaviours.OfType<T>().Any();
        }

        public override bool Equals(object? obj)
        {
            var item = obj as Point;

            if (item == null)
            {
                return false;
            }

            return ApproximatelyEqual(x, item.x) && ApproximatelyEqual(y, item.y);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        protected static bool ApproximatelyEqual(double one, double two)
        {
            return Math.Abs(one - two) < 1E-15;
        }
    }
}