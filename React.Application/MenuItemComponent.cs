using React.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;
using React.Box;

namespace React.Application
{
    public class MenuItemProps
    {
        public string Name { get; set; }
        public Action OnClick { get; set; }
    }

    public class MenuItemComponent : StatelessComponent<MenuItemProps, MenuItemComponent>
    {
        public MenuItemComponent(MenuItemProps props)
        {
        }
        
        public override IElement Render(StatelessComponentRenderContext<MenuItemProps> context)
        {
            return new TextElement(new TextElementProps(context.Props.Name));
        }
    }
}
