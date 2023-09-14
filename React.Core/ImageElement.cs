using System.Drawing;

namespace React.Core
{
    public class ImageElementProps
    {
        public ImageElementProps(Image image)
        {
            this.Image = image;
        }

        public Image Image { get; }
    }

    public class ImageElement : IElement
    {
        public ImageElement(ImageElementProps props)
        {
            this.Props = props;
        }

        public ImageElementProps Props { get; }

        public IElementState Update(IElementState existing, RenderContext context, Bounds bounds)
        {
            return context.Renderer.UpdateImageElementState(existing, this, context, bounds);
        }
    }
}
