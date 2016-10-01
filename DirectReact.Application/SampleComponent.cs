using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    public class SampleComponentProps
    {
        public int Frame;
    }

    public class SampleComponent : Component<SampleComponentProps, SampleComponent>
    {
        public SampleComponent(SampleComponentProps props)
            : base(props)
        {            
        }

        public override IElement Render()
        {
            return new VerticalList(
                new HorizontalList(
                    new VerticalList(
                        new Text("FPS"),
                        new Text(Props.Frame.ToString())),
                    new VerticalList(
                        new Text("Item1"),
                        new Text("Item2"),
                        new Text("Item3"))),
                new HorizontalList(
                    new Text("Item5")));
        }
    }
}
