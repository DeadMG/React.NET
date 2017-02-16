using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D;
using React.Core;
using React.Box;

namespace React.DirectRenderer
{
    public class Renderer : IDisposable, IRenderer
    {
        public readonly SharpDX.Direct3D11.RenderTargetView renderTargetView;
        public readonly SharpDX.Direct3D11.Device device;
        public readonly SharpDX.Direct3D11.DeviceContext1 context;
        public readonly SharpDX.Direct3D11.Texture2D backBuffer;
        public readonly SharpDX.DXGI.SwapChain swapChain;
        public readonly SharpDX.DXGI.Surface backBufferSurface;
        public readonly SharpDX.Direct2D1.RenderTarget d2dTarget;
        public readonly SharpDX.DirectWrite.Factory fontFactory;
        public readonly SharpDX.Direct2D1.Factory d2dFactory;

        private IElementState state;

        public Renderer(IntPtr outputHandle, IElement renderable, Bounds b, IComponentContext initialContext)
        {
            SwapChainDescription description = new SwapChainDescription()
            {
                ModeDescription = new ModeDescription(b.Width, b.Height, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Usage = Usage.RenderTargetOutput,
                BufferCount = 2,
                SwapEffect = SwapEffect.FlipSequential,
                IsWindowed = true,
                OutputHandle = outputHandle
            };
            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.Debug | DeviceCreationFlags.BgraSupport, description, out device, out swapChain);
            context = device.ImmediateContext.QueryInterface<SharpDX.Direct3D11.DeviceContext1>();
            backBuffer = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0);
            renderTargetView = new SharpDX.Direct3D11.RenderTargetView(device, backBuffer);
            
            backBufferSurface = swapChain.GetBackBuffer<Surface>(0);
            d2dFactory = new SharpDX.Direct2D1.Factory();
            var properties = new RenderTargetProperties(new PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            d2dTarget = new RenderTarget(d2dFactory, backBufferSurface, properties);
            
            fontFactory = new SharpDX.DirectWrite.Factory();
            RenderTree(renderable, b, initialContext);
        }
        
        public void RenderTree(IElement renderable, Bounds bounds, IComponentContext o)
        {
            if (renderable == null) throw new InvalidOperationException();
            state = renderable.Update(state, new UpdateContext(bounds, this, o));
        }

        public void RenderFrame()
        {
            context.Rasterizer.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF
            {
                Height = 720,
                Width = 1280,
                X = 0,
                Y = 0,
                MinDepth = 0,
                MaxDepth = 1
            });
            context.OutputMerger.SetRenderTargets(renderTargetView);
            context.ClearRenderTargetView(renderTargetView, new SharpDX.Mathematics.Interop.RawColor4(32 / 255f, 103 / 255f, 178 / 255f, 1f));
            d2dTarget.BeginDraw();
            state.Render(this);
            d2dTarget.EndDraw();
            swapChain.Present(1, PresentFlags.None);
        }
        
        public void Dispose()
        {
            var disposables = typeof(Renderer).GetFields().Select(p => p.GetValue(this) as IDisposable).Where(i => i != null);
            foreach (var disposable in disposables)
                disposable.Dispose();
        }

        public void OnMouseClick(ClickEvent click)
        {
            if (state.BoundingBox.IsInBounds(click))
                state.OnMouseClick(click);
        }

        public IElementState UpdateTextElementState(IElementState existing, Bounds b, TextElement t, IComponentContext o)
        {
            return new TextElementState(t, b, this, o);
        }

        public IElementState UpdateSolidColourElementState(IElementState existing, Bounds b1, SolidColourElement b2, IComponentContext o)
        {
            var context = new UpdateContext(b1, this, o);
            var existingBackground = existing as SolidColourElementState;
            if (existingBackground == null)
            {
                existing?.Dispose();
                return new SolidColourElementState(b2, context);
            }
            existingBackground.Update(b2, context);
            return existingBackground;
        }

        public static Renderer AssertRendererType(IRenderer renderer)
        {
            if (!(renderer is Renderer)) throw new InvalidOperationException("Can't render D2D compnents with a non-D2D renderer");
            return renderer as Renderer;
        }
    }
}
