using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class MenuItemComponent : Component<MenuItemComponent, Renderer>
    {
        public override IElement<Renderer> Render()
        {
            return new Text("MenuItem");
        }
    }
}
