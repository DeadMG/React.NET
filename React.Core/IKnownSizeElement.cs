using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public interface IKnownSizeElement<out ElementState>
        where ElementState : IKnownSizeElementState
    {
        ElementState Update(IKnownSizeElementState existing, RenderContext renderContext);
    }
}
