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
            return new BackgroundElement(new BackgroundElementProps { Colour = new Colour { R = 0.0f, G = 0.0f, A = 1.0f, B = 0.0f } },
                this.State.Clicked ? new StretchElement(this.RenderContents()) : this.RenderContents());
        }

        private IElement RenderContents()
        {
            return new Line(LineDirection.Vertical,
                new TextElement(new TextElementProps { Text = "DirectReact Sample" }),
                MenuComponent.CreateElement(null),
                new Line(LineDirection.Horizontal,
                    ProjectViewerComponent.CreateElement(null),
                    new Line(LineDirection.Horizontal,
                        new TextElement(new TextElementProps { Text = "Clicked:" }),
                        new TextElement(new TextElementProps { Text = this.State.Clicked.ToString(), OnMouseClick = click => this.State = new RootComponentState { Clicked = !this.State.Clicked } }),
                        this.RenderRandomBox())));
        }

        private IElement RenderRandomBox()
        {
            if (!State.Clicked) return null;
            return new SolidColourElement(new SolidColourElementProps
            {
                Colour = new Colour { R = 1.0f, G = 0.0f, B = 0.0f, A = 1.0f },
                Location = bounds => new Bounds(x: bounds.X, y: bounds.Y, height: 40, width: 40)
            });
        }
    }
}
