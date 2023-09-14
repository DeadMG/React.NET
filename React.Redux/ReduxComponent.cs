using React.Box;
using React.Core;
using System;

namespace React.Redux
{
    public abstract class ReduxComponent<S, A, P, C> : StatelessComponent<P, C>
        where C : ReduxComponent<S, A, P, C>
    {
        public override IElement Render(P props, IComponentContext context)
        {
            var reduxContext = context as IReduxComponentContext<S, A>;
            if (reduxContext == null) throw new InvalidOperationException();
            return Render(props, reduxContext);
        }

        public abstract IElement Render(P props, IReduxComponentContext<S, A> redux);
    }

    public abstract class ReduxComponent<S, A, C> : ReduxComponent<S, A, EmptyProps, C>
        where C : ReduxComponent<S, A, C>
    {
    }
}
