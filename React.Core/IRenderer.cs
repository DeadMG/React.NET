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

        IElementState UpdateSolidColourElementState(IElementState existing, SolidColourElement element, RenderContext context, Bounds bounds);
        IKnownSizeElementState UpdateSolidColourElementState(IKnownSizeElementState existing, KnownSizeSolidColourElement element, RenderContext context);

        IElementState UpdateImageElementState(IElementState existing, ImageElement element, RenderContext context, Bounds bounds);
        IKnownSizeElementState UpdateImageElementState(IKnownSizeElementState existing, ImageElement element, RenderContext context);

        void RenderFrame(IElementState root);
    }
}
