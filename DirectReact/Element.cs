using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public abstract class Element<S, E> : IElement
        where E : Element<S, E>
        where S : class, IUpdatableElementState<E>
    {
        public IElementState Update(IElementState existing, Renderer r)
        {
            if (existing == null)
            {
                return (S)typeof(S).GetConstructor(new Type[] { typeof(E), typeof(Renderer) }).Invoke(new object[] { (E)this, r });
            }
            var existingOfType = existing as S;
            if (existingOfType == null)
            {
                existing.Dispose();
                return (S)typeof(S).GetConstructor(new Type[] { typeof(E), typeof(Renderer) }).Invoke(new object[] { (E)this, r });
            }
            existingOfType.Update((E)this, r);
            return existingOfType;
        }
    }
}
