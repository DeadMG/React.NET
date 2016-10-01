using SharpDX.RawInput;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                var bounds = new Bounds
                {
                    X = 0,
                    Y = 0,
                    Width = 1280,
                    Height = 720
                };
                
                using (var renderer = new Renderer(renderForm.Handle, RootComponent.CreateElement(null), bounds))
                {
                    Device.RegisterDevice(SharpDX.Multimedia.UsagePage.Generic, SharpDX.Multimedia.UsageId.GenericMouse, DeviceFlags.None);
                    Device.MouseInput += (object sender, MouseInputEventArgs e) =>
                    {
                        var location = renderForm.PointToClient(Cursor.Position);
                        if (e.ButtonFlags == MouseButtonFlags.LeftButtonUp)
                        {
                            renderer.OnMouseClick(new ClickEvent
                            {                                
                                X = location.X,
                                Y = location.Y
                            });
                        }
                    };
                    RenderLoop.Run(renderForm, () =>
                    {
                        renderer.RenderFrame();
                        renderer.RenderTree(RootComponent.CreateElement(null), bounds);
                    });
                }
            }
        }
    }
}
