using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectReact.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var renderForm = new RenderForm("DirectReact sample"))
            {
                renderForm.ClientSize = new System.Drawing.Size(1280, 720);
                renderForm.AllowUserResizing = false;

                int frame = 0;
                using (var renderer = new Renderer(renderForm.Handle, SampleComponent.CreateElement(new SampleComponentProps { Frame = frame })))
                {
                    RenderLoop.Run(renderForm, () =>
                    {
                        renderer.RenderFrame();
                        renderer.RenderTree(SampleComponent.CreateElement(new SampleComponentProps { Frame = frame++ }));
                    });
                }
            }
        }
    }
}
