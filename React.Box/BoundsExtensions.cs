using React.Core;
using System.Collections.Generic;
using System.Linq;

namespace React.Box
{
    public static class BoundsExtensions
    {
        public static bool IsInBounds(this Bounds area, MouseState location)
        {
            return area.X < location.X && (area.X + area.Width) > location.X && area.Y < location.Y && (area.Y + area.Height) > location.Y;
        }

        public static Bounds Remaining(this Bounds original, LineDirection direction, Bounds occupied)
        {
            if (direction == LineDirection.Horizontal)
            {
                return new Bounds(x: original.X + occupied.Width, y: original.Y, width: original.Width - occupied.Width, height: original.Height);
            }
            return new Bounds(x: original.X, y: original.Y + occupied.Height, width: original.Width, height: original.Height - occupied.Height);
        }

        public static Bounds Sum(this Bounds original, LineDirection direction, IEnumerable<Bounds> occupied)
        {
            if (direction == LineDirection.Horizontal)
            {
                return new Bounds(x: original.X, y: original.Y, width: occupied.Aggregate(0, (lhs, rhs) => lhs + rhs.Width), height: occupied.Max(item => item.Height));
            }
            return new Bounds(x: original.X, y: original.Y, width: occupied.Max(item => item.Width), height: occupied.Aggregate(0, (lhs, rhs) => lhs + rhs.Height));
        }
    }
}
