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
        private class Update
        {
            public Action UserDefinedAction { get; set; }
            public IEvent ExternalEvent { get; set; }
        }

        private List<Update> queuedUpdates = new List<Update>();
        private bool running = false;

        private void OnNextUpdate(Update newUpdate)
        {
            lock (this)
            {
                queuedUpdates.Add(newUpdate);
                if (queuedUpdates.Count >= 1 && !running)
                {
                    ThreadPool.QueueUserWorkItem(obj =>
                    {
                        var externalEvents = new List<IEvent>();
                        while (true)
                        {
                            List<Update> updates = null;
                            lock (this)
                            {
                                running = true;
                                updates = queuedUpdates;
                                queuedUpdates = new List<Update>();
                            }
                            if (updates.Count == 0)
                            {
                                OnUpdatesFinished?.Invoke(externalEvents);
                                lock (this)
                                {
                                    if (updates.Count != 0)
                                    {
                                        externalEvents = new List<IEvent>();
                                        continue;
                                    }
                                    else
                                    {
                                        running = false;
                                        return;
                                    }
                                }
                            }
                            foreach (var update in updates)
                                update.UserDefinedAction?.Invoke();
                            externalEvents.AddRange(updates.Where(u => u.ExternalEvent != null).Select(u => u.ExternalEvent));
                        }
                    });
                }
            }
        }

        public void OnNextUpdate(Action a)
        {
            OnNextUpdate(new Update { UserDefinedAction = a });
        }

        public void OnNextUpdate(IEvent cause)
        {
            OnNextUpdate(new Update { ExternalEvent = cause });
        }

        public event Action<List<IEvent>> OnUpdatesFinished;
    }
}
