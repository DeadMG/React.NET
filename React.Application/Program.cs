﻿using React.DirectRenderer;
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
using Redux.Box;
using React.Redux;

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
                    var store = new Store<StoreState, StoreState>(
                        new StoreState { Clicked = false, TextBoxState = new TextState(new List<TextPiece> { new TextPiece("test", false) }) },
                        (state, action) => action);

                    var rootElement = new ReduxProviderElement<StoreState, StoreState>(store, RootComponent.CreateElement(null));
                    var backgroundUpdater = new BackgroundUpdateContext(rootElement, renderer, null, bounds);
                    store.StateChanged += (existing, newState) => backgroundUpdater.OnNextUpdate(new ChangeEvent<StoreState>(existing, newState));
                    eventSource.Mouse += (ChangeEvent<MouseState> mouseEvent) => backgroundUpdater.OnNextUpdate(mouseEvent);
                    eventSource.Keyboard += (KeyboardEvent keyboardEvent) => backgroundUpdater.OnNextUpdate(keyboardEvent);
                    renderForm.Resize += (sender, _args) =>
                    {
                        var newbounds = new Bounds(x: 0, y: 0, width: renderForm.ClientSize.Width, height: renderForm.ClientSize.Height);
                        var resizeEvent = new ChangeEvent<ResizeState>(new ResizeState(bounds.Width, bounds.Height), new ResizeState(newbounds.Width, newbounds.Height));
                        bounds = newbounds;
                        backgroundUpdater.OnNextUpdate(resizeEvent);
                    };

                    // Get the ball rolling
                    backgroundUpdater.OnNextUpdate(() => { });
                    
                    RenderLoop.Run(renderForm, () =>
                    {
                        backgroundUpdater.EnsureRenderedOneFrame();
                    });
                }
            }
        }
    }
}
