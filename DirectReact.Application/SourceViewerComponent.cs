using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class SourceViewerComponent : Component<SourceViewerComponent>
    {
        public override IElement Render()
        {
            return new Text("Source");
        }
    }
}
