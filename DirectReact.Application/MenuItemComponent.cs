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

    public class MenuItemComponent : Component<MenuItemProps, MenuItemComponent>
    {
        public MenuItemComponent(MenuItemProps props) : base(props)
        {
        }

        public override IElement Render()
        {
            return new TextElement(this.Props.Name);
        }
    }
}
