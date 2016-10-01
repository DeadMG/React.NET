using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class MenuItemComponent<Renderer> : Component<MenuItemComponent<Renderer>, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public override IElement<Renderer> Render()
        {
            return new TextElement<Renderer>("MenuItem");
        }
    }
}
