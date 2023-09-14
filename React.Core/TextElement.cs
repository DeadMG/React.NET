namespace React.Core
{
    public class TextuallyPositionedChild
    {
        public TextuallyPositionedChild(TextSelection location, IElement<IElementState> child)
        {
            this.Location = location;
            this.Child = child;
        }

        public TextSelection Location { get; }
        public IElement<IElementState> Child { get; }
    }

    public class TextElementProps
    {
        public TextElementProps(string text)
        {
            this.Text = text;
        }

        public string Text { get; }
    }
    
    public class TextElement : IElement<ITextElementState>
    {
        public TextElement(TextElementProps props, params TextuallyPositionedChild[] children)
        {
            this.Props = props;
            this.Children = children ?? new TextuallyPositionedChild[0];
        }

        public ITextElementState Update(IElementState existing, RenderContext context, Bounds bounds)
        {
            return context.Renderer.UpdateTextElementState(existing, this, context, bounds);
        }

        public TextuallyPositionedChild[] Children { get; }
        public TextElementProps Props { get; }
    }
}
