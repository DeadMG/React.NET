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
        public ControlledTextBoxProps(string text, Action<string> onChange = null, Action<TextSelection[]> onSelectionChange = null, TextSelection[] selection = null, Action<LeftMouseUpEvent> onClick = null)
            : base(onClick)
        {
            this.OnChange = onChange;
            this.OnSelectionChange = onSelectionChange;
            this.Text = text ?? "";
            this.Selection = selection ?? new TextSelection[0];
            if (Selection.Any(p => p == null))
                throw new InvalidOperationException("Null text selection range");
            Func<int, bool> isInvalid = index => index < 0 || index > text.Length + 1;
            if (Selection.Any(p => isInvalid(p.StartIndex) || isInvalid(p.EndIndex)))
                throw new InvalidOperationException("Text selection range had bad indexes");
        }

        public Action<string> OnChange { get; }
        public string Text { get; }

        public Action<TextSelection[]> OnSelectionChange { get; }
        public TextSelection[] Selection { get; }
    }

    public class ControlledTextBox : Component<ControlledTextBoxProps, ControlledTextBox>
    {
        public ControlledTextBox(ControlledTextBoxProps props) 
            : base(props)
        {
        }

        public override IElement Render()
        {
            var selection = this.Props.Selection;
            if (selection.Length == 0)
            {
                return new TextElement(new TextElementProps(this.Props.Text, onMouseClick: this.Props.OnMouseClick));
            }
            return new TextElement(new TextElementProps(
                    text: this.Props.Text,
                    children: Props.Selection.Select(p => RenderSelection(p)).ToArray(),
                    onMouseClick: this.OnTextClick));
        }

        private void OnTextClick(TextClickEvent clickEvent)
        {
            Props.OnSelectionChange?.Invoke(new[] { new TextSelection(0, clickEvent.TextIndex) });
        }

        private IElement RenderUnselectedText()
        {
            return new TextElement(new TextElementProps(this.Props.Text, onMouseClick: this.Props.OnMouseClick));
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
if t