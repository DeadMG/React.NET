using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public class EmptyProps { };

    public class StatelessComponentRenderContext<P>
    {
        public StatelessComponentRenderContext(P props, IComponentContext context)
        {
            this.Props = props;
            this.Context = context;
        }

        public P Props { get; }
        public IComponentContext Context { get; }
    }
    
    public abstract class StatelessComponent<C> : StatelessComponent<EmptyProps, C>
        where C : StatelessComponent<C>
    {
    }

    public abstract class StatelessComponent<P, C>
        where C : StatelessComponent<P, C>
    {        
        public static StatelessComponentElement<P, C> CreateElement(P currentProps)
        {
            return new StatelessComponentElement<P, C>(currentProps);
        }

        public abstract IElement Render(StatelessComponentRenderContext<P> context);
    }
}
