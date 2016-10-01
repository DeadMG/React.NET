using DirectReact;
using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class RootComponent<Renderer> : Component<RootComponent<Renderer>, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public override IElement<Renderer> Render()
        {
            return new Line<Renderer>(LineDirection.Vertical,
                new TextElement<Renderer>("DirectReact Sample"),
                MenuComponent<Renderer>.CreateElement(null),
                new Line<Renderer>(LineDirection.Horizontal,
                    ProjectViewerComponent<Renderer>.CreateElement(null),
                    SourceViewerComponent<Renderer>.CreateElement(null)));
        }
    }
}
