using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public interface IElement
    {
        IElementState Update(IElementState existing, UpdateContext context);
    }
}
