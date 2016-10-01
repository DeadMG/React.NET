using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public interface IElement<Renderer>
        where Renderer : IRenderer<Renderer>
    {
        IElementState<Renderer> Update(IElementState<Renderer> existing, Bounds b, Renderer r);
    }
}
