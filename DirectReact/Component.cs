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
        public abstract IElement Render();

        public static ClassComponentElement<P, C> CreateElement(P currentProps)
        {
            return new ClassComponentElement<P, C>(props => CreateInstance(props), currentProps);
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

    public abstract class Component<P, S, C> : Component<P, C>
        where C : Component<P, S, C>
    {
        public Component(P props)
            : base(props)
        {
        }

        public S State { get; internal set; }
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
