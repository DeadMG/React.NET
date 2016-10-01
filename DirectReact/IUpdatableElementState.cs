using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public interface IUpdatableElementState<E, Renderer> : IElementState<Renderer>
        where E : IElement<Renderer>
        where Renderer : IRenderer<Renderer>
    {
        void Update(E other, Bounds b, Renderer r);
    }
}
