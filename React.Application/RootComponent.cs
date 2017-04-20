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
        public TextSelection[] Selection { get; set; }
    }

    public class RootComponent : StatefulComponent<EmptyProps, RootComponentState, RootComponent>
    {
        public RootComponent(EmptyProps props) : base(new RootComponentState { Clicked = false, Selection = new TextSelection[] { new TextSelection(2, 2) } })
        {
        }
        
        public override IElement<IElementState> Render(EmptyProps props, RootComponentState state, IComponentContext context)
        {
            return new BackgroundElement(new BackgroundElementProps(new Colour(r: 0.0f, g: 0.0f, b: 0.0f, a: 1.0f)),
                state.Clicked ? new StretchElement(this.RenderContents(state)) : this.RenderContents(state));
        }

        private IElement<IElementState> RenderContents(RootComponentState state)
        {
            return new Line(new LineProps(LineDirection.Vertical),
                new TextElement(new TextElementProps("DirectReact Sample")),
                MenuComponent.CreateElement(null),
                new Line(new LineProps(LineDirection.Horizontal),
                    ProjectViewerComponent.CreateElement(null),
                    new Line(new LineProps(LineDirection.Horizontal),
                        new TextElement(new TextElementProps("Clicked:")),
                        new EventElement<ChangeEvent<MouseState>, IElementState>((mouse, elementState) => this.TextClicked(mouse, elementState, state),
                            new TextElement(new TextElementProps(state.Clicked.ToString()))),
                        this.RenderRandomBox(state))));
        }

        private IElement<IElementState> RenderRandomBox(RootComponentState state)
        {
            return ControlledTextBox.CreateElement(new ControlledTextBoxProps(text: "test", selection: state.Selection, onSelectionChange: (sel) => this.SetState(new RootComponentState { Clicked = state.Clicked, Selection = sel })));
        }

        private void TextClicked(ChangeEvent<MouseState> mouse, IElementState textElementState, RootComponentState state)
        {
            if (mouse.OriginalState.LeftButtonDown && !mouse.NewState.LeftButtonDown && textElementState.BoundingBox.IsInBounds(mouse.NewState))
            {
                this.SetState(new RootComponentState { Clicked = !state.Clicked, Selection = state.Selection });
            }
        }
    }
}
