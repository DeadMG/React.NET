using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class VerticalList : Element<VerticalListState, VerticalList>
    {
        public VerticalList(params IElement[] Children)
        {
            this.Children = Children;
        }

        public IElement[] Children { get; }
    }

    public class VerticalListState : IUpdatableElementState<VerticalList>
    {
        private List<IElementState> nestedElementStates;

        public VerticalListState(VerticalList e, Bounds b, Renderer r)
        {
            var originalBounds = b;
            nestedElementStates = new List<IElementState>();
            foreach (var child in e.Children)
            {
                var newState = child.Update(null, b, r);
                nestedElementStates.Add(newState);
                b = new Bounds
                {
                    Height = b.Height - newState.Bounds.Height,
                    Width = b.Width,
                    X = b.X,
                    Y = b.Y + newState.Bounds.Height
                };
            }
            Bounds = new Bounds
            {
                X = originalBounds.X,
                Width = originalBounds.Width,
                Y = originalBounds.Y,
                Height = nestedElementStates.Aggregate(0, (lhs, rhs) => lhs + rhs.Bounds.Height)
            };
        }

        public void Update(VerticalList other, Bounds b, Renderer r)
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
                    Height = b.Height - newState.Bounds.Height,
                    Width = b.Width,
                    X = b.X,
                    Y = b.Y + newState.Bounds.Height
                };
                newStates.Add(newState);
            }
            nestedElementStates = newStates;
            Bounds = new Bounds
            {
                X = originalBounds.X,
                Width = originalBounds.Width,
                Y = originalBounds.Y,
                Height = nestedElementStates.Aggregate(0, (lhs, rhs) => lhs + rhs.Bounds.Height)
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
