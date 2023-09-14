using React.Box;
using React.Core;
using System;

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
        
        public override IElement Render(MenuItemProps props, IComponentContext context)
        {
            return new TextElement(new TextElementProps(props.Name));
        }
    }
}
