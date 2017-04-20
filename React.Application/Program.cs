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
using System.Drawing;
using React.Box;

namespace React.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var renderForm = new RenderForm("DirectReact sample"))
            {
                renderForm.ClientSize = new System.Drawing.Size(1280, 720);
                var bounds = new Bounds(x: 0, y: 0, width: renderForm.ClientSize.Width, height: renderForm.ClientSize.Height);
                var first = true;
                var eventSource = new RawInputEventSource(() => {
                    if (first)
                    {
                        first = false;
                        return renderForm.PointToClient(Cursor.Position);
                    }
                    if (renderForm.Focused) return renderForm.PointToClient(Cursor.Position);
                    return null;
                });
                
                using (var renderer = new Renderer(renderForm.Handle, bounds))
                {
                    var rootElement = RootComponent.CreateElement(null);
                    var backgroundUpdater = new BackgroundUpdateContext(rootElement, renderer, null, bounds);
                    eventSource.Mouse += (ChangeEvent<MouseState> mouseEvent) => backgroundUpdater.OnNextUpdate(mouseEvent);
                    eventSource.Keyboard += (ChangeEvent<KeyboardState> keyboardEvent) => backgroundUpdater.OnNextUpdate(keyboardEvent);
                    renderForm.Resize += (sender, _args) =>
                    {
                        var newbounds = new Bounds(x: 0, y: 0, width: renderForm.ClientSize.Width, height: renderForm.ClientSize.Height);
                        var resizeEvent = new ChangeEvent<ResizeState>(new ResizeState(bounds.Width, bounds.Height), new ResizeState(newbounds.Width, newbounds.Height));
                        bounds = newbounds;
                        backgroundUpdater.OnNextUpdate(resizeEvent);
                    };
                    RenderLoop.Run(renderForm, () =>
                    {
                        backgroundUpdater.EnsureRenderedOneFrame();
                    });
                }
            }
        }
    }
}
