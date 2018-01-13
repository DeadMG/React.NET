using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public class StatelessComponentElement<P, C> : Element<StatelessComponentElementState<P, C>, StatelessComponentElement<P, C>>
        where C : StatelessComponent<P, C>
    {
        public StatelessComponentElement(P Props)
        {
            this.Props = Props;
        }

        public P Props { get; }
    }

    public class StatelessComponentElementState<P, C> : IElementState
        where C : StatelessComponent<P, C>
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
        private readonly C ComponentInstance;

        public StatelessComponentElementState(StatelessComponentElementState<P, C> existing, StatelessComponentElement<P, C> parent, RenderContext context, Bounds bounds)
        {
            ComponentInstance = existing?.ComponentInstance ?? CreateInstance(parent.Props);
            renderResult = ComponentInstance.Render(parent.Props, context.Context).Update(existing?.renderResult, context, bounds);
            if (ComponentInstance is IDisposable)
            {
                context.Disposables.Add(ComponentInstance as IDisposable);
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
