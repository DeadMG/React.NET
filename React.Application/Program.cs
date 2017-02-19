using React.DirectRenderer;
using SharpDX.RawInput;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using React.Core;

namespace React.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var renderForm = new RenderForm("DirectReact sample"))
            {
                renderForm.ClientSize = new System.Drawing.Size(1280, 720);
                renderForm.AllowUserResizing = false;
                var bounds = new Bounds(x: 0, y: 0, width: 1280, height: 720);
                
                using (var renderer = new Renderer(renderForm.Handle, RootComponent.CreateElement(null), bounds, null))
                {
                    Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericMouse, DeviceFlags.None);
                    Device.MouseInput += (object sender, MouseInputEventArgs e) =>
                    {
                        var location = renderForm.PointToClient(Cursor.Position);
                        if (e.ButtonFlags == MouseButtonFlags.LeftButtonUp)
                        {
                            renderer.OnMouseClick(new LeftMouseUpEvent(location.X, location.Y));
                        }
                    };
                    RenderLoop.Run(renderForm, () =>
                    {
                        renderer.RenderFrame();
                    });
                }
            }
        }
    }
}
