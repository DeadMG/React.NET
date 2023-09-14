using React.Core;

namespace React.Box
{
    public class EmptyProps { };
        
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

        public abstract IElement<IElementState> Render(P props, IComponentContext context);
    }
}
