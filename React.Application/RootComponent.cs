﻿using React;
using React.DirectRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using React.Core;
using React.Box;
using React.Redux;

namespace React.Application
{
    public class RootComponent : ReduxComponent<StoreState, StoreState, RootComponent>
    {
        public RootComponent(EmptyProps props) : base()
        {
        }
        
        public override IElement<IElementState> Render(EmptyProps props, IReduxComponentContext<StoreState, StoreState> context)
        {
            return new SolidColourElement(new SolidColourElementProps(new Colour(r: 0.0f, g: 0.0f, b: 0.0f, a: 1.0f)),
                context.State.Clicked ? new StretchElement(this.RenderContents(context.State, context.Dispatch)) : this.RenderContents(context.State, context.Dispatch));
        }

        private IElement<IElementState> RenderContents(StoreState state, Action<StoreState> dispatch)
        {
            return new Line(new LineProps(LineDirection.Vertical),
                new TextElement(new TextElementProps("DirectReact Sample")),
                MenuComponent.CreateElement(null),
                new Line(new LineProps(LineDirection.Horizontal),
                    ProjectViewerComponent.CreateElement(null),
                    new Line(new LineProps(LineDirection.Horizontal),
                        new TextElement(new TextElementProps("Clicked:")),
                        new EventElement<ChangeEvent<MouseState>, IElementState>((mouse, elementState) => this.TextClicked(mouse, elementState, state, dispatch),
                            new TextElement(new TextElementProps(state.Clicked.ToString()))),
                        this.RenderRandomBox(state, dispatch))));
        }

        private IElement<IElementState> RenderRandomBox(StoreState state, Action<StoreState> dispatch)
        {
            return ControlledTextBox.CreateElement(new ControlledTextBoxProps(state.TextBoxState, onTextStateChange: sel => dispatch(new StoreState { Clicked = state.Clicked, TextBoxState = sel })));
        }

        private void TextClicked(ChangeEvent<MouseState> mouse, IElementState element, StoreState state, Action<StoreState> dispatch)
        {
            if (mouse.OriginalState.LeftButtonDown && !mouse.NewState.LeftButtonDown && element.BoundingBox.IsInBounds(mouse.NewState))
            {
                dispatch(new StoreState { Clicked = !state.Clicked, TextBoxState = state.TextBoxState });
            }
        }
    }
}
