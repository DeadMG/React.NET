using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public class StatefulComponentElement<P, S, C> : Element<StatefulComponentElementState<P, S, C>, StatefulComponentElement<P, S, C>>
        where C : StatefulComponent<P, S, C>
    {
        public StatefulComponentElement(P Props)
        {
            this.Props = Props;
        }

        public P Props { get; }
    }

    public class StatefulComponentElementState<P, S, C> : IElementState
        where C : StatefulComponent<P, S, C>
    {
        private static C CreateInstance(P currentProps)
        {
            var normalConstructor = typeof(C).GetConstructor(new Type[] { typeof(P) });
            if (normalConstructor != null)
            {
                return (C)normalConstructor.Invoke(new object[] { currentProps });
            }
            if (typeof(P) == typeof(EmptyProps))
            {
                var specialEmptyConstructor = typeof(C).GetConstructor(new Type[0]);
                if (specialEmptyConstructor != null)
                {
                    return (C)specialEmptyConstructor.Invoke(new object[0]);
                }
            }
            throw new InvalidOperationException();
        }

        private readonly IElementState renderResult;
        private readonly C componentInstance;

        public StatefulComponentElementState(StatefulComponentElementState<P, S, C> existing, StatefulComponentElement<P, S, C> parent, RenderContext context)
        {
            componentInstance = existing?.componentInstance ?? CreateInstance(parent.Props);
            componentInstance.updateContext = context.UpdateContext;
            renderResult = componentInstance.Render(parent.Props, componentInstance.state, context.Context).Update(existing?.renderResult, context);
            if (componentInstance is IDisposable)
            {
                context.Disposables.Add(componentInstance as IDisposable);
            }
        }

        public void Render(IRenderer r)
        {
            renderResult?.Render(r);
        }

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
            renderResult.FireEvents(events);
        }

        public Bounds BoundingBox => renderResult.BoundingBox;
    }
}
