using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class UpdateContext
    {
        public UpdateContext(Bounds bounds, IRenderer renderer, IComponentContext context, IEventLevel eventSource)
        {
            this.Bounds = bounds;
            this.Renderer = renderer;
            this.Context = context;
            this.EventSource = eventSource;
        }

        public UpdateContext WithBounds(Bounds b)
        {
            return new UpdateContext(b, Renderer, Context, EventSource);
        }

        public Bounds Bounds { get; }
        public IRenderer Renderer { get; }
        public IComponentContext Context { get; }
        public IEventLevel EventSource { get; }
    }
}
