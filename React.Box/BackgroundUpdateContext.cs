using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace React.Box
{
    public class BackgroundUpdateContext : IUpdateContext
    {
        // Immutable- no sync required
        private readonly IElement<IElementState> root;
        private readonly IRenderer renderer;
        private readonly IComponentContext context;

        // Accessed ONLY from background thread after initialization
        private IElementState currentElementState;
        private HashSet<IDisposable> currentDisposables;
        private Bounds currentBounds;

        private class Update
        {
            public Action UserDefinedAction { get; set; }
            public IEvent ExternalEvent { get; set; }
        }

        // Accessed from multiple threads- lock this
        private List<Update> queuedUpdates = new List<Update>();
        private bool running = false;

        private void OnNextUpdate(Update newUpdate)
        {
            lock (this)
            {
                queuedUpdates.Add(newUpdate);
                if (thereAreNoQueuedUpdates() || running)
                {
                    return;
                }
                running = true;
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    while (true)
                    {
                        List<Update> updates = fetchQueuedUpdates();
                        if (thereAreQueuedUp(updates))
                        {
                            handle(updates);
                        }
                        else
                        {
                            OnUpdatesFinished();
                            lock (this)
                            {
                                if (newUpdatesHaveArrived())
                                {
                                    continue;
                                }
                                else
                                {
                                    running = false;
                                    return;
                                }
                            }
                        }
                    }
                });
            }
        }

        private bool thereAreNoQueuedUpdates()
        {
            return queuedUpdates.Count < 1;
        }

        private List<Update> fetchQueuedUpdates()
        {
            lock (this)
            {
                updates = queuedUpdates;
                queuedUpdates = new List<Update>();
                return updates;
            }
        }

        private bool thereAreQueuedUp(List<Update> updates)
        {
            return updates.Count != 0;
        }

        private bool newUpdatesHaveArrived()
        {
            return queuedUpdates.Count != 0;
        }

        private void handle(List<Update> updates)
        {
            foreach (var update in updates)
            {
                invokeUserDefinedAction(update);
                handleExternalEvent(update);
            }
        }

        private void invokeUserDefinedAction(Update update)
        {
            update.UserDefinedAction?.Invoke();
        }

        private void handleExternalEvent(Update update)
        {
            if (update.ExternalEvent != null)
            {
                currentElementState.FireEvents(new List<IEvent> { update.ExternalEvent });
            }
        }


        private void OnUpdatesFinished()
        {
            var newDisposables = new HashSet<IDisposable>();
            var newElementState = root.Update(currentElementState, new RenderContext(currentBounds, renderer, context, this, newDisposables));
            foreach (var disposable in currentDisposables)
            {
                if (!newDisposables.Contains(disposable))
                    disposable.Dispose();
            }
            currentDisposables = newDisposables;
            currentElementState = newElementState;
            renderer.RenderFrame(currentElementState);
        }
        
        public void OnNextUpdate(Action a)
        {
            OnNextUpdate(new Update { UserDefinedAction = a });
        }

        public void OnNextUpdate(IEvent cause)
        {
            OnNextUpdate(new Update { ExternalEvent = cause });
        }
        
        public BackgroundUpdateContext(IElement<IElementState> root, IRenderer renderer, IComponentContext context, Bounds initialBounds)
        {
            this.root = root;
            this.renderer = renderer;
            this.context = context;
            currentBounds = initialBounds;
            currentDisposables = new HashSet<IDisposable>();
            currentElementState = root.Update(null, new RenderContext(currentBounds, renderer, context, this, currentDisposables));
            renderer.RenderFrame(currentElementState);
        }

        private bool first = true;
        public void EnsureRenderedOneFrame()
        {
            if (first)
            {
                OnNextUpdate(() => { });
                first = false;
            }
        }
    }
}
