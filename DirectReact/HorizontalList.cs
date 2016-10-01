using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class HorizontalList : Element<HorizontalListState, HorizontalList>
    {
        public HorizontalList(params IElement[] Children)
        {
            this.Children = Children;
        }

        public IElement[] Children { get; }
    }

    public class HorizontalListState : IUpdatableElementState<HorizontalList>
    {
        private List<IElementState> nestedElementStates;

        public HorizontalListState(HorizontalList e, Bounds b, Renderer r)
        {
            var originalBounds = b;
            nestedElementStates = new List<IElementState>();
            foreach (var child in e.Children)
            {
                var newState = child.Update(null, b, r);
                nestedElementStates.Add(newState);
                b = new Bounds
                {
                    Height = b.Height,
                    Width = b.Width - newState.Bounds.Width,
                    X = b.X + newState.Bounds.Width,
                    Y = b.Y 
                };
            }
            Bounds = new Bounds
            {
                X = originalBounds.X,
                Height = nestedElementStates.Max(item => item.Bounds.Height),
                Y = originalBounds.Y,
                Width = nestedElementStates.Aggregate(0, (lhs, rhs) => lhs + rhs.Bounds.Width)
            };
        }

        public void Update(HorizontalList other, Bounds b, Renderer r)
        {
            var originalBounds = b;
            var newStates = new List<IElementState>();
            for (int i = 0; i < Math.Max(nestedElementStates.Count, other.Children.Length); ++i)
            {
                if (i >= other.Children.Length)
                {
                    nestedElementStates[i].Dispose();
                    continue;
                }
                IElementState existingState = i >= nestedElementStates.Count ? null : nestedElementStates[i];
                var newState = other.Children[i].Update(existingState, b, r);
                b = new Bounds
                {
                    Height = b.Height,
                    Width = b.Width - newState.Bounds.Width,
                    X = b.X + newState.Bounds.Width,
                    Y = b.Y
                };
                newStates.Add(newState);
            }
            nestedElementStates = newStates;
            Bounds = new Bounds
            {
                X = originalBounds.X,
                Height = nestedElementStates.Max(item => item.Bounds.Height),
                Y = originalBounds.Y,
                Width = nestedElementStates.Aggregate(0, (lhs, rhs) => lhs + rhs.Bounds.Width)
            };
        }

        public Bounds Bounds { get; private set; }

        public void Dispose()
        {
            foreach (var state in nestedElementStates)
                state.Dispose();
        }

        public void Render(Renderer r)
        {
            foreach (var element in nestedElementStates)
            {
                element.Render(r);
            }
        }
    }
}
