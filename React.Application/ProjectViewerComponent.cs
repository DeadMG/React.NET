using React.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;

namespace React.Application
{
    public class ProjectViewerComponent : Component<ProjectViewerComponent>
    {
        public override IElement Render()
        {
            return new Line(LineDirection.Vertical,
                new TextElement(new TextElementProps { Text = "File1" }),
                new TextElement(new TextElementProps { Text = "File2" }));
        }
    }
}
