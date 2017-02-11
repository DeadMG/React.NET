using DirectReact;
using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class RootComponent : Component<RootComponent>
    {
        public override IElement Render()
        {
            return new Line(LineDirection.Vertical,
                new TextElement("DirectReact Sample"),
                MenuComponent.CreateElement(null),
                new Line(LineDirection.Horizontal,
                    ProjectViewerComponent.CreateElement(null),
                    SourceViewerComponent.CreateElement(null)));
        }
    }
}
