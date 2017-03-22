using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class RenderContext
    {
        public RenderContext(Bounds bounds, IRenderer renderer, IComponentContext context, IUpdateContext updateContext, HashSet<IDisposable> disposables)
        {
            this.Bounds = bounds;
            this.Renderer = renderer;
            this.Context = context;
            this.Disposables = disposables;
            this.UpdateContext = updateContext;
        }

        public RenderContext WithBounds(Bounds b)
        {
            return new RenderContext(b, Renderer, Context, UpdateContext, Disposables);
        }

        public Bounds Bounds { get; }
        public IRenderer Renderer { get; }
        public IComponentContext Context { get; }
        public IUpdateContext UpdateContext { get; }
        public HashSet<IDisposable> Disposables { get; }
    }
}
