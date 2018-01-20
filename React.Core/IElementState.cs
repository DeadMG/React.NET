using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public interface IElementState : ICanFireEvents
    {
        void Render(IRenderer r);

        Bounds BoundingBox { get; }
    }
}
