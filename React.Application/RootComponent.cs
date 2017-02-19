using React;
using React.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;
using React.Box;

namespace React.Application
{
    public class RootComponentState
    {
        public bool Clicked { get; set; }
        public TextSelection Selection { get; set; }
    }

    public class RootComponent : Component<EmptyProps, RootComponentState, RootComponent>
    {
        public RootComponent(EmptyProps props) : base(props, new RootComponentState { Clicked = false, Selection = new TextSelection(2, 2) })
        {
        }

        public override IElement Render()
        {
            return new BackgroundElement(new BackgroundElementProps { Colour = new Colour(r: 0.0f, g: 0.0f, b: 0.0f, a: 1.0f) },
                this.State.Clicked ? new StretchElement(this.RenderContents()) : this.RenderContents());
        }

        private IElement RenderContents()
        {
            return new Line(LineDirection.Vertical,
                new TextElement(new TextElementProps("DirectReact Sample")),
                MenuComponent.CreateElement(null),
                new Line(LineDirection.Horizontal,
                    ProjectViewerComponent.CreateElement(null),
                    new Line(LineDirection.Horizontal,
                        new TextElement(new TextElementProps("Clicked:")),
                        new TextElement(new TextElementProps(this.State.Clicked.ToString(), onMouseClick: click => this.State = new RootComponentState { Clicked = !this.State.Clicked, Selection = this.State.Selection })),
                        this.RenderRandomBox())));
        }

        private IElement RenderRandomBox()
        {
            var selection = State.Clicked ? new[] { this.State.Selection } : null;
            return ControlledTextBox.CreateElement(new ControlledTextBoxProps(text: "test", selection: selection, onSelectionChange: sel => this.State = new RootComponentState { Clicked = this.State.Clicked, Selection = sel[0] }));
        }
    }
}
