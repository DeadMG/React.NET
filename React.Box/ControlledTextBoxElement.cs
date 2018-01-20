using React.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace React.Box
{
    public class TextPiece
    {
        public TextPiece(string text, bool selected)
        {
            this.Text = text ?? "";
            this.Selected = selected;
        }

        public string Text { get; }
        public bool Selected { get; }
    }

    public class TextState
    {
        public TextState(List<TextPiece> pieces)
        {
            this.TextPieces = pieces ?? new List<TextPiece>();
        }
        
        public List<TextPiece> TextPieces { get; }
    }

    public class ControlledTextBoxProps
    {
        public ControlledTextBoxProps(TextState textState, Action<TextState> onTextStateChange)
        {
            this.TextState = textState ?? new TextState(null);
            this.OnTextStateChange = onTextStateChange;
        }

        public TextState TextState { get; }
        public Action<TextState> OnTextStateChange { get; }

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
            return new EventElement<ChangeEvent<MouseState>, IEventElementState<KeyboardEvent, ITextElementState>>((@event, eventElementState) => this.OnTextMouse(@event, eventElementState.Nested, props, state),
                new EventElement<KeyboardEvent, ITextElementState>((@event, textElementState) => this.OnTextKeyboard(@event, textElementState, props, state),
                    new TextElement(new TextElementProps(Text(props)),
                        Selections(props).Select(p => RenderSelection(p)).ToArray())));
        }

        private string Text(ControlledTextBoxProps props)
        {
            return String.Join("", props.TextState.TextPieces.Select(p => p.Text));
        }

        private IEnumerable<TextSelection> Selections(ControlledTextBoxProps props)
        {
            var index = 0;
            foreach (var piece in props.TextState.TextPieces)
            {
                if (!piece.Selected)
                {
                    index += piece.Text.Length;
                    continue;
                }
                yield return new TextSelection(index, index + piece.Text.Length);
                index += piece.Text.Length;
            }
        }

        private IEnumerable<TextPiece> ReplaceSelection(string value, List<TextPiece> pieces)
        {
            foreach(var piece in pieces)
            {
                if (piece.Selected)
                {
                    yield return new TextPiece(value, false);
                    yield return new TextPiece("", true);
                }
                else
                {
                    yield return piece;
                }
            }
        }

        private IEnumerable<TextPiece> RemoveAdjacentCharacters(bool forward, IEnumerable<TextPiece> pieces)
        {
            var piecesArr = pieces.ToArray();
            for(int i = 0; i < piecesArr.Length; ++i)
            {
                var piece = piecesArr[i];
                var nextIndex = forward ? i + 1 : i - 1;
                if (nextIndex >= 0 && nextIndex < piecesArr.Length && piecesArr[nextIndex].Selected)
                {
                    var newText = forward ? piece.Text.Substring(0, piece.Text.Length - 1) : piece.Text.Substring(1);
                    yield return new TextPiece(newText, false);
                }
                else
                {
                    yield return piecesArr[i];
                }
            }
        }

        private IEnumerable<TextPiece> RemoveRedundantPieces(IEnumerable<TextPiece> pieces)
        {
            return pieces.Where(p => p.Selected || p.Text.Length != 0);
        }

        private IEnumerable<TextPiece> CollapseToFirstSelection(List<TextPiece> pieces)
        {
            var targetPiece = pieces.First(p => p.Selected);

            // We cut all selections, then insert as appropriate. If there are no selections
            // to be inserted, add one at the start (this is the only case where this can happen).
            if (!pieces.Any(piece => piece == targetPiece && piece.Text.Length != 0) && 
                !pieces.Any(piece => pieces.IndexOf(piece) == pieces.IndexOf(targetPiece) - 1 && targetPiece.Text.Length == 0))
            {
                yield return new TextPiece("", true);
            }
            
            foreach (var piece in pieces)
            {
                if (piece == targetPiece && piece.Text.Length != 0)
                {
                    // Collapse to the start of the selection
                    yield return new TextPiece("", true);
                    yield return new TextPiece(piece.Text, false);
                    continue;                 
                }

                if (pieces.IndexOf(piece) == pieces.IndexOf(targetPiece) - 1 && targetPiece.Text.Length == 0)
                {
                    yield return new TextPiece(piece.Text.Substring(0, piece.Text.Length - 1), false);
                    yield return new TextPiece("", true);
                    yield return new TextPiece(piece.Text.Substring(piece.Text.Length - 1), false);
                    continue;
                }

                if (piece.Selected)
                {
                    // Cut any extra selections
                    yield return new TextPiece(piece.Text, false);
                    continue;
                }

                yield return piece;
            }
        }

        private IEnumerable<TextPiece> CollapseToLastSelection(List<TextPiece> pieces)
        {
            var targetPiece = pieces.First(p => p.Selected);

            foreach (var piece in pieces)
            {
                if (piece == targetPiece && piece.Text.Length != 0)
                {
                    // Collapse to the end of the selection
                    yield return new TextPiece(piece.Text, false);
                    yield return new TextPiece("", true);
                    continue;
                }

                if (pieces.IndexOf(piece) == pieces.IndexOf(targetPiece) + 1 && targetPiece.Text.Length == 0)
                {
                    yield return new TextPiece(piece.Text.Substring(0, 1), false);
                    yield return new TextPiece("", true);
                    yield return new TextPiece(piece.Text.Substring(1), false);
                    continue;
                }

                if (piece.Selected)
                {
                    // Cut any extra selections
                    yield return new TextPiece(piece.Text, false);
                    continue;
                }

                yield return piece;
            }

            // We cut all selections, then insert as appropriate. If there are no selections
            // to be inserted, add one at the start (this is the only case where this can happen).
            if (!pieces.Any(piece => piece == targetPiece && piece.Text.Length != 0) &&
                !pieces.Any(piece => pieces.IndexOf(piece) == pieces.IndexOf(targetPiece) + 1 && targetPiece.Text.Length == 0))
            {
                yield return new TextPiece("", true);
            }
        }

        private void OnTextKeyboard(KeyboardEvent keyboardEvent, ITextElementState textElementState, ControlledTextBoxProps props, ControlledTextBoxState state)
        {
            if (!keyboardEvent.WasUp) return;

            // Control characters
            var simplified = RemoveRedundantPieces(props.TextState.TextPieces).ToList();
            if (keyboardEvent.KeyName.ToLower() == "backspace")
            {
                var replaced = RemoveRedundantPieces(ReplaceSelection("", simplified).ToList()).ToList();
                props.OnTextStateChange(new TextState(RemoveAdjacentCharacters(true, replaced).ToList()));
                return;
            }

            if (keyboardEvent.KeyName.ToLower() == "delete")
            {
                var replaced = RemoveRedundantPieces(ReplaceSelection("", simplified).ToList()).ToList();
                props.OnTextStateChange(new TextState(RemoveAdjacentCharacters(false, replaced).ToList()));
                return;
            }

            if (keyboardEvent.KeyName.ToLower() == "left")
            {
                if (!props.TextState.TextPieces.Any(p => p.Selected)) return;
                var replaced = RemoveRedundantPieces(CollapseToFirstSelection(simplified)).ToList();
                props.OnTextStateChange(new TextState(replaced));
                return;
            }

            if (keyboardEvent.KeyName.ToLower() == "right")
            {
                if (!props.TextState.TextPieces.Any(p => p.Selected)) return;
                var replaced = RemoveRedundantPieces(CollapseToLastSelection(simplified)).ToList();
                props.OnTextStateChange(new TextState(replaced));
                return;
            }

            if (keyboardEvent.TextValue == null) return;

            props.OnTextStateChange(new TextState(ReplaceSelection(keyboardEvent.TextValue, props.TextState.TextPieces).ToList()));
        }

        private void OnTextMouse(ChangeEvent<MouseState> clickEvent, ITextElementState textElementState, ControlledTextBoxProps props, ControlledTextBoxState state)
        {
            var originalTextIndex = textElementState.GetTextIndex(clickEvent.OriginalState.X, clickEvent.OriginalState.Y);
            var newTextIndex = textElementState.GetTextIndex(clickEvent.NewState.X, clickEvent.NewState.Y);
            if (!originalTextIndex.HasValue || !newTextIndex.HasValue)
            {
                if (!clickEvent.OriginalState.LeftButtonDown && clickEvent.NewState.LeftButtonDown && props.TextState.TextPieces.Any(p => p.Selected && p.Text.Length != 0))
                {
                    props.OnTextStateChange(new TextState(new List<TextPiece> { new TextPiece(Text(props), false) }));
                }
                return;
            }
            if (!clickEvent.OriginalState.LeftButtonDown && clickEvent.NewState.LeftButtonDown)
            {
                this.SetState(new ControlledTextBoxState { SelectionStartTextIndex = newTextIndex.Value });
                
                props.OnTextStateChange(new TextState(new List<TextPiece> {
                    new TextPiece(Text(props).Substring(0, newTextIndex.Value), false),
                    new TextPiece("", true),
                    new TextPiece(Text(props).Substring(newTextIndex.Value), false)
                }));
            }
            if (clickEvent.OriginalState.LeftButtonDown && clickEvent.NewState.LeftButtonDown && state.SelectionStartTextIndex.HasValue)
            {
                var selectionStartIndex = Math.Min(state.SelectionStartTextIndex.Value, newTextIndex.Value);
                var selectionEndIndex = Math.Max(state.SelectionStartTextIndex.Value, newTextIndex.Value);
                props.OnTextStateChange(new TextState(new List<TextPiece>
                {
                    new TextPiece(Text(props).Substring(0, selectionStartIndex), false),
                    new TextPiece(Text(props).Substring(selectionStartIndex, selectionEndIndex - selectionStartIndex), true),
                    new TextPiece(Text(props).Substring(selectionEndIndex), false)
                }));
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
            return new TextuallyPositionedChild(selection, new SolidColourElement(new SolidColourElementProps(caretColour), new FixedWidthElement(1)));
        }

        private TextuallyPositionedChild RenderTextRangeSelection(TextSelection selection)
        {
            var selectionHighlightColour = new Colour(r: 0.0f, g: 0.75f, b: 1.0f, a: 0.5f);
            return new TextuallyPositionedChild(selection, new SolidColourElement(new SolidColourElementProps(selectionHighlightColour), new StretchElement(null)));
        }
    }
}
