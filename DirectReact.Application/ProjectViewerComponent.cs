using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class ProjectViewerComponent<Renderer> : Component<ProjectViewerComponent<Renderer>, Renderer>
        where Renderer : IRenderer<Renderer>
    {
        public override IElement<Renderer> Render()
        {
            return new Line<Renderer>(LineDirection.Vertical,
                new TextElement<Renderer>("File1"),
                new TextElement<Renderer>("File2"));
        }
    }
}
