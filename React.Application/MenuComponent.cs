using React.Box;
using React.Core;

namespace React.Application
{
    public class MenuComponent : StatelessComponent<MenuComponent>
    {
        public override IElement Render(EmptyProps props, IComponentContext context)
        {
            return new Line(new LineProps(LineDirection.Horizontal),
                MenuItemComponent.CreateElement(new MenuItemProps { Name = "File" }),
                MenuItemComponent.CreateElement(new MenuItemProps { Name = "Edit" }));
        }
    }
}
