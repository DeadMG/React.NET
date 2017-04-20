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
        
        public override IElement<IElementState> Render(MenuItemProps props, IComponentContext context)
        {
            return new TextElement(new TextElementProps(props.Name));
        }
    }
}
