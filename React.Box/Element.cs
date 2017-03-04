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
        public IElementState Update(IElementState existing, UpdateContext context)
        {
            var existingOfType = existing as S;
            if (existingOfType == null)
            {
                existing?.Dispose();
                return (S)typeof(S).GetConstructor(new Type[] { typeof(S), typeof(E), typeof(UpdateContext) }).Invoke(new object[] { null, (E)this, context });
            }
            return (S)typeof(S).GetConstructor(new Type[] { typeof(S), typeof(E), typeof(UpdateContext) }).Invoke(new object[] { existingOfType, (E)this, context });
        }
    }
}
