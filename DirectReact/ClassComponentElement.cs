using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class ClassComponentElement<P, C, Renderer> : Element<ClassComponentElementState<P, C, Renderer>, ClassComponentElement<P, C, Renderer>, Renderer>
        where C : Component<P, C, Renderer>
    {
        internal readonly Func<P, C> Component;

        public ClassComponentElement(Func<P, C> Component, P Props)
        {
            this.Component = Component;
            this.Props = Props;
        }

        public P Props { get; }        
    }

    public class ClassComponentElementState<P, C, Renderer> : IUpdatableElementState<ClassComponentElement<P, C, Renderer>, Renderer>
        where C : Component<P, C, Renderer>
    {
        private Bounds latestBounds;

        public ClassComponentElementState(ClassComponentElement<P, C, Renderer> parent, Bounds b, Renderer r)
        {
            ComponentInstance = parent.Component(parent.Props);            
            RenderResult = ComponentInstance.Render().Update(RenderResult, b, r);
            latestBounds = b;
            var statefulComponent = ComponentInstance as IStatefulComponent;
            if (statefulComponent != null)
            {
                statefulComponent.OnStateSet = () =>
                {
                    RenderResult = ComponentInstance.Render().Update(RenderResult, latestBounds, r);
                };
            }
        }

        public void Update(ClassComponentElement<P, C, Renderer> parent, Bounds b, Renderer r)
        {
            latestBounds = b;
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

        public void OnMouseClick(ClickEvent click)
        {
            RenderResult.OnMouseClick(click);
        }

        public readonly C ComponentInstance;
        public IElementState<Renderer> RenderResult;

        public Bounds BoundingBox
        {
            get
            {
                return RenderResult.BoundingBox;
            }
        }
    }
}
