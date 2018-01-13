using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Core
{
    public class RenderContext
    {
        public RenderContext(IRenderer renderer, IComponentContext context, IUpdateContext updateContext, HashSet<IDisposable> disposables)
        {
            this.Renderer = renderer;
            this.Context = context;
            this.Disposables = disposables;
            this.UpdateContext = updateContext;
        }
        
        public IRenderer Renderer { get; }
        public IComponentContext Context { get; }
        public IUpdateContext UpdateContext { get; }
        public HashSet<IDisposable> Disposables { get; }
    }
}
