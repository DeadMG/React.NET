using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Box
{
    public abstract class Element<S, E> : IElement
        where E : Element<S, E>
        where S : class, IElementState
    {
        public IElementState Update(IElementState existing, RenderContext context)
        {
            var result = (S)typeof(S).GetConstructor(new Type[] { typeof(S), typeof(E), typeof(RenderContext) }).Invoke(new object[] { existing as S, (E)this, context });
            if (result is IDisposable)
            {
                context.Disposables.Add(result as IDisposable);
            }
            return result;
        }
    }
}
