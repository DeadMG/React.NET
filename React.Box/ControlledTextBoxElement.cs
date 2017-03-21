using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class ControlledTextBoxProps : PrimitiveProps
    {
        public ControlledTextBoxProps(string text, Action<string> onTextChange = null, Action<TextSelection[]> onSelectionChange = null, TextSelection[] selection = null, Action<MouseEvent, Bounds> onMouse = null, Action<KeyboardEvent> onKeyboard = null)
            : base(onMouse, onKeyboard)
        {
            this.OnTextChange = onTextChange;
            this.OnSelectionChange = onSelectionChange;
            this.Text = text ?? "";
            this.Selection = selection ?? new TextSelection[0];
            if (Selection.Any(p => p == null))
                throw new InvalidOperationException("Null text selection range");
            Func<int, bool> isInvalid = index => index < 0 || index > text.Length + 1;
            if (Selection.Any(p => isInvalid(p.StartIndex) || isInvalid(p.EndIndex)))
                throw new InvalidOperationException("Text selection range had bad indexes");
        }

        public Action<string> OnTextChange { get; }
        public string Text { get; }

        public Action<TextSelection[]> OnSelectionChange { get; }
        public TextSelection[] Selection { get; }
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

        public override IElement Render(StatefulComponentRenderContext<ControlledTextBoxProps, ControlledTextBoxState> context)
        {
            return new TextElement(new TextElementProps(text: context.Props.Text, onMouse: (mouseEvent, bounds) => this.OnTextClick(mouseEvent, bounds, context.Props, context.State)),
                context.Props.Selection.Select(p => RenderSelection(p)).ToArray());
        }

        private void OnTextClick(TextMouseEvent clickEvent, Bounds bounds, ControlledTextBoxProps props, ControlledTextBoxState state)
        {
            props.OnMouse?.Invoke(new MouseEvent(clickEvent.OriginalState.Mouse, clickEvent.NewState.Mouse), bounds);
            if (!clickEvent.OriginalState.TextIndex.HasValue || !clickEvent.NewState.TextIndex.HasValue)
            {
                if (!clickEvent.OriginalState.Mouse.LeftButtonDown && clickEvent.NewState.Mouse.LeftButtonDown && props.Selection.Length != 0)
                {
                    props.OnSelectionChange(new TextSelection[] { });
                }
                return;
            }
            if (!clickEvent.OriginalState.Mouse.LeftButtonDown && clickEvent.NewState.Mouse.LeftButtonDown)
            {
                this.SetState(new ControlledTextBoxState { SelectionStartTextIndex = clickEvent.NewState.TextIndex.Value });
                props.OnSelectionChange?.Invoke(new[] { new TextSelection(clickEvent.NewState.TextIndex.Value, clickEvent.NewState.TextIndex.Value) });
            }
            if (clickEvent.OriginalState.Mouse.LeftButtonDown && clickEvent.NewState.Mouse.LeftButtonDown && state.SelectionStartTextIndex.HasValue)
            {
                props.OnSelectionChange?.Invoke(new[] { new TextSelection(Math.Min(state.SelectionStartTextIndex.Value, clickEvent.NewState.TextIndex.Value), Math.Max(state.SelectionStartTextIndex.Value, clickEvent.NewState.TextIndex.Value)) });
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
