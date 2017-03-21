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
    public class MenuComponent : StatelessComponent<MenuComponent>
    {
        public override IElement Render(StatelessComponentRenderContext<EmptyProps> context)
        {
            return new Line(new LineProps(LineDirection.Horizontal),
                MenuItemComponent.CreateElement(new MenuItemProps { Name = "File" }),
                MenuItemComponent.CreateElement(new MenuItemProps { Name = "Edit" }));
        }
    }
}
