using DirectReact.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class SourceViewerState
    {
        public bool Clicked { get; set; }
    }

    public class SourceViewerComponent : Component<EmptyProps, SourceViewerState, SourceViewerComponent>
    {
        public SourceViewerComponent(EmptyProps props) : base(props, new SourceViewerState { Clicked = false })
        {
        }

        public override IElement Render()
        {
            return new Line(LineDirection.Horizontal,
                new TextElement("Clicked:"),
                new TextElement(this.State.Clicked.ToString())) { OnMouseClick = click => this.State = new SourceViewerState { Clicked = !this.State.Clicked } };
        }
    }
}
