using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public class ClassComponentElement<P, C> : Element<ClassComponentElementState<P, C>, ClassComponentElement<P, C>>
        where C : Component<P, C>
    {
        public ClassComponentElement(P Props)
        {
            this.Props = Props;
        }

        public P Props { get; }
    }

    public class ClassComponentElementState<P, C> : IElementState
        where C : Component<P, C>
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

        private UpdateContext latestContext;

        public ClassComponentElementState(ClassComponentElementState<P, C> existing, ClassComponentElement<P, C> parent, UpdateContext context)
        {
            ComponentInstance = existing?.ComponentInstance ?? CreateInstance(parent.Props);
            ComponentInstance.Context = context.Context;
            ComponentInstance.CreatingElementState = this;
            ComponentInstance.Props = parent.Props;
            RenderResult = ComponentInstance.Render().Update(existing?.RenderResult, context);
            latestContext = context;
        }

        internal void ForceUpdate()
        {
            RenderResult = ComponentInstance.Render().Update(RenderResult, latestContext);
        }
        
        public void Dispose()
        {
            (ComponentInstance as IDisposable)?.Dispose();
            RenderResult.Dispose();
        }
        
        public void Render(IRenderer r)
        {
            RenderResult?.Render(r);
        }
        
        public readonly C ComponentInstance;
        public IElementState RenderResult;
        public Bounds BoundingBox => RenderResult.BoundingBox;
    }
}
