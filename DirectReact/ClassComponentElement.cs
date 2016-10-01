using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class ClassComponentElement<P, C> : Element<ClassComponentElementState<P, C>, ClassComponentElement<P, C>>
        where C : Component<P, C>
    {
        internal readonly Func<P, C> Component;

        public ClassComponentElement(Func<P, C> Component, P Props)
        {
            this.Component = Component;
            this.Props = Props;
        }

        public P Props { get; }        
    }

    public class ClassComponentElementState<P, C> : IUpdatableElementState<ClassComponentElement<P, C>>
        where C : Component<P, C>
    {
        public ClassComponentElementState(ClassComponentElement<P, C> parent, Bounds b, Renderer r)
        {
            ComponentInstance = parent.Component(parent.Props);
            RenderResult = ComponentInstance.Render().Update(RenderResult, b, r);
        }

        public void Update(ClassComponentElement<P, C> parent, Bounds b, Renderer r)
        {
            ComponentInstance.Props = parent.Props;
            RenderResult = ComponentInstance.Render().Update(RenderResult, b, r);
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
        
        public void Render(Renderer r)
        {
            RenderResult.Render(r);
        }
        
        public readonly C ComponentInstance;
        public IElementState RenderResult;

        public Bounds BoundingBox
        {
            get
            {
                return RenderResult.BoundingBox;
            }
        }
    }
}
