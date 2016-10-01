using DirectReact;
using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class RootComponent : Component<RootComponent, Renderer>
    {
        public override IElement<Renderer> Render()
        {
            return new Line<Renderer>(LineDirection.Vertical,
                new Text("DirectReact Sample"),
                MenuComponent.CreateElement(null),
                new Line<Renderer>(LineDirection.Horizontal,
                    ProjectViewerComponent.CreateElement(null),
                    SourceViewerComponent.CreateElement(null)));
        }
    }
}
