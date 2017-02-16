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
    }

    public class RootComponent : Component<EmptyProps, RootComponentState, RootComponent>
    {
        public RootComponent(EmptyProps props) : base(props, new RootComponentState { Clicked = false })
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
                        new TextElement(new TextElementProps(this.State.Clicked.ToString(), click => this.State = new RootComponentState { Clicked = !this.State.Clicked })),
                        this.RenderRandomBox())));
        }

        private IElement RenderRandomBox()
        {
            if (!State.Clicked) return null;
            return new SolidColourElement(new SolidColourElementProps(new Colour(r: 1.0f, g: 0.0f, b: 0.0f, a: 1.0f), bounds => new Bounds(x: bounds.X, y: bounds.Y, height: 40, width: 40)));
        }
    }
}
