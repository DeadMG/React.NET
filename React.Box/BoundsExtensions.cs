using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public static class BoundsExtensions
    {
        public static bool IsInBounds(this Bounds area, IPositionedEvent location)
        {
            return area.X < location.X && (area.X + area.Width) > location.X && area.Y < location.Y && (area.Y + area.Height) > location.Y;
        }

        public static Bounds Remaining(this Bounds original, LineDirection direction, Bounds occupied)
        {
            if (direction == LineDirection.Horizontal)
            {
                return new Bounds
                {
                    Height = original.Height,
                    Width = original.Width - occupied.Width,
                    X = original.X + occupied.Width,
                    Y = original.Y
                };
            }
            return new Bounds
            {
                Height = original.Height - occupied.Height,
                Width = original.Width,
                X = original.X,
                Y = original.Y + occupied.Height
            };
        }

        public static Bounds Sum(this Bounds original, LineDirection direction, IEnumerable<Bounds> occupied)
        {
            if (direction == LineDirection.Horizontal)
            {
                return new Bounds
                {
                    X = original.X,
                    Height = occupied.Max(item => item.Height),
                    Y = original.Y,
                    Width = occupied.Aggregate(0, (lhs, rhs) => lhs + rhs.Width)
                };
            }
            return new Bounds
            {
                X = original.X,
                Width = occupied.Max(item => item.Width),
                Y = original.Y,
                Height = occupied.Aggregate(0, (lhs, rhs) => lhs + rhs.Height)
            };
        }
    }
}
