using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class CompoundElement : Element<CompoundElementState, CompoundElement>
    {
        public CompoundElement(params IElement[] Children)
        {
            this.Children = Children;
        }

        public IElement[] Children { get; }
    }

    public class CompoundElementState : IUpdatableElementState<CompoundElement>
    {
        private List<IElementState> nestedElementStates;

        public CompoundElementState(CompoundElement e, Renderer r)
        {
            nestedElementStates = e.Children.Select(p => p.Update(null, r)).ToList();
        }

        public void Update(CompoundElement other, Renderer r)
        {
            var newStates = new List<IElementState>();
            for (int i = 0; i < Math.Max(nestedElementStates.Count, other.Children.Length); ++i)
            {
                if (i >= other.Children.Length)
                {
                    nestedElementStates[i].Dispose();
                    continue;
                }
                IElementState existingState = i >= nestedElementStates.Count ? null : nestedElementStates[i];
                newStates.Add(other.Children[i].Update(existingState, r));
            }
            nestedElementStates = newStates;
        }
        
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
