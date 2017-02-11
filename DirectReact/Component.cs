using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class EmptyProps { };

    public abstract class Component<P, C>
        where C : Component<P, C>
    {
        public Component(P props)
        {
            this.Props = props;
        }

        public P Props { get; internal set; }
        public IComponentContext Context { get; internal set; }

        public abstract IElement Render();

        public static ClassComponentElement<P, C> CreateElement(P currentProps)
        {
            return new ClassComponentElement<P, C>(currentProps);
        }

        internal ClassComponentElementState<P, C> CreatingElementState;
    }
    
    public abstract class Component<P, S, C> : Component<P, C>
        where C : Component<P, S, C>
    {
        private S realState;

        public Component(P props, S initialState)
            : base(props)
        {
            realState = initialState;
        }

        public S State { get { return realState; } set { realState = value; CreatingElementState.ForceUpdate(); } }
    }

    public abstract class Component<C> : Component<EmptyProps, C>
        where C : Component<C>
    {
        public Component()
            : base(new EmptyProps())
        {
        }
    }
}
