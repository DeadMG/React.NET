using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class ControlledTextBoxProps
    {
        public ControlledTextBoxProps(string text, TextSelection[] selection = null, Action<TextSelection[]> onSelectionChange = null, Action<string> onTextChange = null)
        {
            this.Text = text ?? "";
            this.Selection = selection ?? new TextSelection[0];
            this.OnSelectionChange = onSelectionChange;
            this.OnTextChange = onTextChange;

            if (Selection.Any(p => p == null))
                throw new InvalidOperationException("Null text selection range");
            Func<int, bool> isInvalid = index => index < 0 || index > text.Length + 1;
            if (Selection.Any(p => isInvalid(p.StartIndex) || isInvalid(p.EndIndex)))
                throw new InvalidOperationException("Text selection range had bad indexes");
        }
        
        public string Text { get; }        
        public TextSelection[] Selection { get; }
        public Action<TextSelection[]> OnSelectionChange { get; }
        public Action<string> OnTextChange { get; }
    }

    public class ControlledTextBoxState
    {
        public int? SelectionStartTextIndex { get; set; }
    }

    public class ControlledTextBox : StatefulComponent<ControlledTextBoxProps, ControlledTextBoxState, ControlledTextBox>
    {
        public ControlledTextBox(ControlledTextBoxProps props)
            : base(new ControlledTextBoxState { SelectionStartTextIndex = null })
        {
        }

        public override IElement<IElementState> Render(ControlledTextBoxProps props, ControlledTextBoxState state, IComponentContext context)
        {
            return new EventElement<ChangeEvent<MouseState>, ITextElementState>((@event, textElementState) => this.OnTextMouse(@event, textElementState, props, state), 
                new TextElement(new TextElementProps(props.Text), 
                    props.Selection.Select(p => RenderSelection(p)).ToArray()));
        }
        
        private void OnTextMouse(ChangeEvent<MouseState> clickEvent, ITextElementState textElementState, ControlledTextBoxProps props, ControlledTextBoxState state)
        {
            var originalTextIndex = textElementState.GetTextIndex(clickEvent.OriginalState.X, clickEvent.OriginalState.Y);
            var newTextIndex = textElementState.GetTextIndex(clickEvent.NewState.X, clickEvent.NewState.Y);
            if (!originalTextIndex.HasValue || !newTextIndex.HasValue)
            {
                if (!clickEvent.OriginalState.LeftButtonDown && clickEvent.NewState.LeftButtonDown && props.Selection.Length != 0)
                {
                    props.OnSelectionChange(new TextSelection[] { });
                }
                return;
            }
            if (!clickEvent.OriginalState.LeftButtonDown && clickEvent.NewState.LeftButtonDown)
            {
                this.SetState(new ControlledTextBoxState { SelectionStartTextIndex = newTextIndex.Value });
                props.OnSelectionChange(new [] { new TextSelection(newTextIndex.Value, newTextIndex.Value) });
            }
            if (clickEvent.OriginalState.LeftButtonDown && clickEvent.NewState.LeftButtonDown && state.SelectionStartTextIndex.HasValue)
            {
                props.OnSelectionChange(new[] { new TextSelection(Math.Min(state.SelectionStartTextIndex.Value, newTextIndex.Value), Math.Max(state.SelectionStartTextIndex.Value, newTextIndex.Value)) });
            }
        }

        private TextuallyPositionedChild RenderSelection(TextSelection selection)
        {
            if (selection.StartIndex != selection.EndIndex) return RenderTextRangeSelection(selection);
            return RenderCaret(selection);
        }

        private TextuallyPositionedChild RenderCaret(TextSelection selection)
        {
            var caretColour = new Colour(r: 1.0f, g: 1.0f, b: 1.0f, a: 1.0f);
            Func<Bounds, Bounds> caretLocation = (Bounds bounds) => new Bounds(x: bounds.X, y: bounds.Y, width: 1, height: bounds.Height);
            return new TextuallyPositionedChild(selection, new SolidColourElement(new SolidColourElementProps(caretColour, caretLocation)));
        }

        private TextuallyPositionedChild RenderTextRangeSelection(TextSelection selection)
        {
            var selectionHighlightColour = new Colour(r: 0.0f, g: 0.75f, b: 1.0f, a: 0.5f);
            return new TextuallyPositionedChild(selection, new SolidColourElement(new SolidColourElementProps(selectionHighlightColour, null)));
        }
    }
}
