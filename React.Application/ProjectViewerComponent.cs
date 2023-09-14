using React.Box;
using React.Core;
using System.Drawing;

namespace React.Application
{
    public class ProjectViewerComponent : StatelessComponent<ProjectViewerComponent>
    {
        private static readonly Image puppyOwl = Image.FromFile("puppyowl.png");

        public override IElement Render(EmptyProps props, IComponentContext context)
        {
            return new Line(new LineProps(LineDirection.Vertical),
                new TextElement(new TextElementProps("File1")),
                new TextElement(new TextElementProps("File2")),
                new ImageElement(new ImageElementProps(puppyOwl)));
        }
    }
}
