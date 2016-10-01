using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class MenuComponent<Renderer> : Component<MenuComponent<Renderer>, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public override IElement<Renderer> Render()
        {
            return new Line<Renderer>(LineDirection.Horizontal,
                MenuItemComponent<Renderer>.CreateElement(new MenuItemProps { Name = "File" }),
                MenuItemComponent<Renderer>.CreateElement(new MenuItemProps { Name = "Edit" }));
        }
    }
}
