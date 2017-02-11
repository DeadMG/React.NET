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
            return new BackgroundElement(new Colour { R = 0.0f, G = 0.0f, A = 1.0f, B = 0.0f },
                new StretchElement(
                    new Line(LineDirection.Vertical,
                        new TextElement("DirectReact Sample"),
                        MenuComponent.CreateElement(null),
                        new Line(LineDirection.Horizontal,
                            ProjectViewerComponent.CreateElement(null),
                            SourceViewerComponent.CreateElement(null)))));
        }
    }
}
