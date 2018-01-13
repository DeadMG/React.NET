using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public interface IRenderer
    {
        ITextElementState UpdateTextElementState(IElementState existing, TextElement element, RenderContext context, Bounds bounds);
        ISolidColourElementState UpdateSolidColourElementState(IElementState existing, SolidColourElement element, RenderContext context, Bounds bounds);
        IImageElementState UpdateImageElementState(IElementState existing, ImageElement element, RenderContext context, Bounds bounds);

        void RenderFrame(IElementState root);
    }
}
