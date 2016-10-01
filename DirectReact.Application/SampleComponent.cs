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
                new TextElement("FPS"),
                new TextElement(Props.Frame.ToString()));
        }
    }
}
