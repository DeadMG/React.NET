using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public class EmptyProps { };

    public abstract class Component<P, C, Renderer>
        where C : Component<P, C, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public Component(P props)
        {
            this.Props = props;
        }

        public P Props { get; internal set; }
        public abstract IElement<Renderer> Render();

        public static ClassComponentElement<P, C, Renderer> CreateElement(P currentProps)
        {
            return new ClassComponentElement<P, C, Renderer>(props => CreateInstance(props), currentProps);
        }

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
    }

    public interface IStatefulComponent
    {
        Action OnStateSet { get; set; }
    }

    public abstract class Component<P, S, C, Renderer> : Component<P, C, Renderer>, IStatefulComponent
        where C : Component<P, S, C, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        private S realState;

        public Component(P props, S initialState)
            : base(props)
        {
            realState = initialState;
        }

        public S State { get { return realState; } set { realState = value; OnStateSet(); } }
        public Action OnStateSet { get; set; }
    }

    public abstract class Component<C, Renderer> : Component<EmptyProps, C, Renderer>
        where C : Component<C, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public Component()
            : base(new EmptyProps())
        {
        }
    }
}
