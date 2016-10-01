using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class MenuItemComponent : Component<MenuItemComponent>
    {
        public override IElement Render()
        {
            return new Text("MenuItem");
        }
    }
}
