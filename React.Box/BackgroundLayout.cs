using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class BackgroundLayout
    {
        private readonly IElement root;
        private readonly IRenderer renderer;
        private readonly IComponentContext context;
        private readonly IUpdateContext updateContext;

        private IElementState currentElementState;
        private HashSet<IDisposable> currentDisposables;
        private Bounds currentBounds;

        private void OnUpdatesFinished(List<IEvent> causes)
        {
            var newDisposables = new HashSet<IDisposable>();
            var newElementState = root.Update(currentElementState, new RenderContext(currentBounds, renderer, context, updateContext, causes, newDisposables));
            foreach(var disposable in currentDisposables)
            {
                if (!newDisposables.Contains(disposable))
                    disposable.Dispose();
            }
            currentDisposables = newDisposables;
            currentElementState = newElementState;
            renderer.RenderFrame(currentElementState);
        }

        public BackgroundLayout(IElement root, IRenderer renderer, IComponentContext context, IUpdateContext updateContext, Bounds initialBounds)
        {
            this.root = root;
            this.renderer = renderer;
            this.context = context;
            this.updateContext = updateContext;
            currentBounds = initialBounds;
            currentDisposables = new HashSet<IDisposable>();
            currentElementState = root.Update(null, new RenderContext(currentBounds, renderer, context, updateContext, new List<IEvent>(), currentDisposables));

            updateContext.OnUpdatesFinished += this.OnUpdatesFinished; 
        }
    }
}
