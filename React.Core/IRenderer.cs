using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public interface IRenderer
    {
        IElementState UpdateTextElementState(IElementState existing, Bounds b, TextElement element, IComponentContext context, IEventLevel eventSource);
        IElementState UpdateSolidColourElementState(IElementState existing, Bounds b, SolidColourElement element, IComponentContext context, IEventLevel eventSource);
    }
}
