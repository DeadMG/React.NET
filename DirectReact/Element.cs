using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public abstract class Element<S, E, Renderer> : IElement<Renderer>
        where E : Element<S, E, Renderer>
        where S : class, IUpdatableElementState<E, Renderer>
    {
        public IElementState<Renderer> Update(IElementState<Renderer> existing, Bounds bounds, Renderer r)
        {
            if (existing == null)
            {
                return (S)typeof(S).GetConstructor(new Type[] { typeof(E), typeof(Bounds), typeof(Renderer) }).Invoke(new object[] { (E)this, bounds, r });
            }
            var existingOfType = existing as S;
            if (existingOfType == null)
            {
                existing.Dispose();
                return (S)typeof(S).GetConstructor(new Type[] { typeof(E), typeof(Bounds), typeof(Renderer) }).Invoke(new object[] { (E)this, bounds, r });
            }
            existingOfType.Update((E)this, bounds, r);
            return existingOfType;
        }
    }
}
