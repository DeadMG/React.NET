using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public interface IKnownSizeElementState : ICanFireEvents
    {
        void Render(IRenderer r, Bounds bounds);

        Dimensions Size { get; }
    }
}
