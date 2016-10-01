using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact
{
    public interface IElementState : IDisposable
    {
        void Render(Renderer r);
        Bounds Bounds { get; }
    }
}
