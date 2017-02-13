using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React
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

    public class ClassComponentElementState<P, C> : IUpdatableElementState<ClassComponentElement<P, C>>
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

        public ClassComponentElementState(ClassComponentElement<P, C> parent, UpdateContext context)
        {
            ComponentInstance = CreateInstance(parent.Props);
            ComponentInstance.Context = context.Context;
            ComponentInstance.CreatingElementState = this;
            RenderResult = ComponentInstance.Render().Update(RenderResult, context);
            latestContext = context;
        }

        internal void ForceUpdate()
        {
            RenderResult = ComponentInstance.Render().Update(RenderResult, latestContext);
        }

        public void Update(ClassComponentElement<P, C> parent, UpdateContext context)
        {
            latestContext = context;
            ComponentInstance.Props = parent.Props;
            ComponentInstance.Context = context.Context;
            RenderResult = ComponentInstance.Render().Update(RenderResult, context);
        }

        public void Dispose()
        {
            var disposable = ComponentInstance as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
            RenderResult.Dispose();
        }
        
        public void Render(IRenderer r)
        {
            RenderResult.Render(r);
        }

        public void OnMouseClick(ClickEvent click)
        {
            RenderResult.OnMouseClick(click);
        }

        public readonly C ComponentInstance;
        public IElementState RenderResult;
        public Bounds BoundingBox => RenderResult.BoundingBox;
    }
}
