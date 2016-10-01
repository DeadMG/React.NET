using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class Bounds
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public static bool IsInBounds(Bounds area, IPositionedEvent location)
        {
            return area.X < location.X && (area.X + area.Width) > location.X && area.Y < location.Y && (area.Y + area.Height) > location.Y;
        }

        public static Bounds Remaining(LineDirection direction, Bounds original, Bounds occupied)
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

        public static Bounds Sum(LineDirection direction, Bounds original, IEnumerable<Bounds> occupied)
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
