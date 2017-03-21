using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class RenderContext
    {
        public RenderContext(Bounds bounds, IRenderer renderer, IComponentContext context, IUpdateContext updateContext, List<IEvent> events, HashSet<IDisposable> disposables)
        {
            this.Bounds = bounds;
            this.Renderer = renderer;
            this.Context = context;
            this.Disposables = disposables;
            this.UpdateContext = updateContext;
            this.Events = events;
        }

        public RenderContext WithBounds(Bounds b)
        {
            return new RenderContext(b, Renderer, Context, UpdateContext, Events, Disposables);
        }

        public Bounds Bounds { get; }
        public IRenderer Renderer { get; }
        public IComponentContext Context { get; }
        public IUpdateContext UpdateContext { get; }
        public HashSet<IDisposable> Disposables { get; }
        public List<IEvent> Events { get; }
    }
}
