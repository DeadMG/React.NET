using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class ProjectViewerComponent : Component<ProjectViewerComponent, Renderer>
    {
        public override IElement<Renderer> Render()
        {
            return new Line<Renderer>(LineDirection.Vertical,
                new Text("File1"),
                new Text("File2"));
        }
    }
}
