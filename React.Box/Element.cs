using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public abstract class Element<S, E> : IElement<S>
        where E : Element<S, E>
        where S : class, IElementState
    {
        public S Update(IElementState existing, RenderContext context, Bounds bounds)
        {
            var result = (S)typeof(S).GetConstructor(new Type[] { typeof(S), typeof(E), typeof(RenderContext), typeof(Bounds) }).Invoke(new object[] { existing as S, (E)this, context, bounds });
            if (result is IDisposable)
            {
                context.Disposables.Add(result as IDisposable);
            }
            return result;
        }
    }
}
