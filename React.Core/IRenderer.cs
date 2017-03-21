using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public interface IRenderer
    {
        IElementState UpdateTextElementState(IElementState existing, TextElement element, RenderContext context);
        IElementState UpdateSolidColourElementState(IElementState existing, SolidColourElement element, RenderContext context);

        void RenderFrame(IElementState root);
    }
}
