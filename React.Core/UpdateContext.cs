using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class UpdateContext
    {
        public UpdateContext(Bounds bounds, IRenderer renderer, IComponentContext context)
        {
            this.Bounds = bounds;
            this.Renderer = renderer;
            this.Context = context;
        }

        public Bounds Bounds { get; }
        public IRenderer Renderer { get; }
        public IComponentContext Context { get; }
    }
}
