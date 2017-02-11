using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class ProjectViewerComponent : Component<ProjectViewerComponent>
    {
        public override IElement Render()
        {
            return new Line(LineDirection.Vertical,
                new TextElement("File1"),
                new TextElement("File2"));
        }
    }
}
