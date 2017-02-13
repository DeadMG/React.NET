using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React
{
    public abstract class Element<S, E> : IElement
        where E : Element<S, E>
        where S : class, IUpdatableElementState<E>
    {
        public IElementState Update(IElementState existing, UpdateContext context)
        {
            if (existing == null)
            {
                return (S)typeof(S).GetConstructor(new Type[] { typeof(E), typeof(UpdateContext) }).Invoke(new object[] { (E)this, context });
            }
            var existingOfType = existing as S;
            if (existingOfType == null)
            {
                existing.Dispose();
                return (S)typeof(S).GetConstructor(new Type[] { typeof(E), typeof(UpdateContext) }).Invoke(new object[] { (E)this, context });
            }
            existingOfType.Update((E)this, context);
            return existingOfType;
        }
    }
}
