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
        public ControlledTextBoxProps(string text, Action<string> onTextChange = null, Action<TextSelection[]> onSelectionChange = null, TextSelection[] selection = null, Action<MouseEvent, Bounds> onMouse = null)
            : base(onMouse)
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

    public class ControlledTextBox : Component<ControlledTextBoxProps, ControlledTextBoxState, ControlledTextBox>
    {
        public ControlledTextBox(ControlledTextBoxProps props) 
            : base(props, new ControlledTextBoxState { SelectionStartTextIndex = null })
        {
        }

        public override IElement Render()
        {
            var selection = this.Props.Selection;
            if (selection.Length == 0)
            {
                return new TextElement(new TextElementProps(this.Props.Text, onMouse: this.OnTextClick));
            }
            return new TextElement(new TextElementProps(text: this.Props.Text, onMouse: this.OnTextClick),
                Props.Selection.Select(p => RenderSelection(p)).ToArray());
        }

        private void OnTextClick(TextMouseEvent clickEvent, Bounds bounds)
        {
            if (!clickEvent.OriginalState.TextIndex.HasValue || !clickEvent.NewState.TextIndex.HasValue) return;
            if (!clickEvent.OriginalState.Mouse.LeftButtonDown && clickEvent.NewState.Mouse.LeftButtonDown)
            {
                this.State = new ControlledTextBoxState { SelectionStartTextIndex = clickEvent.NewState.TextIndex.Value };
                Props.OnSelectionChange?.Invoke(new[] { new TextSelection(clickEvent.NewState.TextIndex.Value, clickEvent.NewState.TextIndex.Value) });
            }
            if (clickEvent.OriginalState.Mouse.LeftButtonDown && clickEvent.NewState.Mouse.LeftButtonDown && this.State.SelectionStartTextIndex.HasValue)
            {
                Props.OnSelectionChange?.Invoke(new[] { new TextSelection(Math.Min(this.State.SelectionStartTextIndex.Value, clickEvent.NewState.TextIndex.Value), Math.Max(this.State.SelectionStartTextIndex.Value, clickEvent.NewState.TextIndex.Value)) });
            }
        }

        private IElement RenderUnselectedText()
        {
            return new TextElement(new TextElementProps(this.Props.Text, onMouse: this.OnTextClick));
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
