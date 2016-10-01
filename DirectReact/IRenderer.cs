using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public interface IRenderer<R>
        where R : IRenderer<R>
    {
        IElementState<R> UpdateTextElementState(IElementState<R> existing, Bounds b, TextElement<R> element);
        IElementState<R> UpdateBackgroundElementState(IElementState<R> existing, Bounds b, BackgroundElement<R> element);
    }
}
