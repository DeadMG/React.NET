using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class MenuComponent : Component<MenuComponent, Renderer>
    {
        public override IElement<Renderer> Render()
        {
            return new Line<Renderer>(LineDirection.Horizontal,
                MenuItemComponent.CreateElement(null),
                MenuItemComponent.CreateElement(null));
        }
    }
}
