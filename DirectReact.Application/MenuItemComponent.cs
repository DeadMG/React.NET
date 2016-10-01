using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class MenuItemProps
    {
        public string Name { get; set; }
        public Action OnClick { get; set; }
    }

    public class MenuItemComponent<Renderer> : Component<MenuItemProps, MenuItemComponent<Renderer>, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public MenuItemComponent(MenuItemProps props) : base(props)
        {
        }

        public override IElement<Renderer> Render()
        {
            return new TextElement<Renderer>(this.Props.Name);
        }
    }
}
