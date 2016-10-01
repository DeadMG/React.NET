﻿using System;
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
                new Text("File1"),
                new Text("File2"));
        }
    }
}
