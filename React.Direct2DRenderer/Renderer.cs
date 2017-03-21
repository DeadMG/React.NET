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
        public SharpDX.Direct3D11.RenderTargetView renderTargetView;
        public readonly SharpDX.Direct3D11.Device device;
        public readonly SharpDX.Direct3D11.DeviceContext1 context;
        public SharpDX.Direct3D11.Texture2D backBuffer;
        public readonly SharpDX.DXGI.SwapChain swapChain;
        public SharpDX.DXGI.Surface backBufferSurface;
        public SharpDX.Direct2D1.RenderTarget d2dTarget;
        public readonly SharpDX.DirectWrite.Factory fontFactory;
        public readonly SharpDX.Direct2D1.Factory d2dFactory;

        private Bounds currentBounds;

        private void InitSwapchain()
        {
            backBuffer = swapChain.GetBackBuffer<SharpDX.Direct3D11.Texture2D>(0);
            renderTargetView = new SharpDX.Direct3D11.RenderTargetView(device, backBuffer);
            backBufferSurface = swapChain.GetBackBuffer<Surface>(0);
            var properties = new RenderTargetProperties(new PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            d2dTarget = new RenderTarget(d2dFactory, backBufferSurface, properties);
        }
        
        public void Resize(Bounds bounds)
        {
            currentBounds = bounds;
            backBuffer?.Dispose();
            backBufferSurface?.Dispose();
            renderTargetView?.Dispose();
            d2dTarget?.Dispose();

            swapChain.ResizeBuffers(2, bounds.Width, bounds.Height, Format.R8G8B8A8_UNorm, SwapChainFlags.None);

            InitSwapchain();
        }

        public Renderer(IntPtr outputHandle, Bounds b)
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
            d2dFactory = new SharpDX.Direct2D1.Factory();
            fontFactory = new SharpDX.DirectWrite.Factory();
            
            Resize(b);
        }

        public void RenderFrame(IElementState root)
        {
            context.Rasterizer.SetViewport(new SharpDX.Mathematics.Interop.RawViewportF
            {
                Height = currentBounds.Height,
                Width = currentBounds.Width,
                X = currentBounds.X,
                Y = currentBounds.Y,
                MinDepth = 0,
                MaxDepth = 1
            });
            context.OutputMerger.SetRenderTargets(renderTargetView);
            context.ClearRenderTargetView(renderTargetView, new SharpDX.Mathematics.Interop.RawColor4(32 / 255f, 103 / 255f, 178 / 255f, 1f));
            d2dTarget.BeginDraw();
            root.Render(this);
            d2dTarget.EndDraw();
            swapChain.Present(1, PresentFlags.None);
        }
        
        public void Dispose()
        {
            var disposables = typeof(Renderer).GetFields().Select(p => p.GetValue(this) as IDisposable).Where(i => i != null);
            foreach (var disposable in disposables)
                disposable.Dispose();
        }
        
        public IElementState UpdateTextElementState(IElementState existing, TextElement t, RenderContext context)
        {
            return new TextElementState(existing as TextElementState, t, context);
        }

        public IElementState UpdateSolidColourElementState(IElementState existing, SolidColourElement b2, RenderContext context)
        {
            return new SolidColourElementState(existing as SolidColourElementState, b2, context);
        }

        public static Renderer AssertRendererType(IRenderer renderer)
        {
            if (!(renderer is Renderer)) throw new InvalidOperationException("Can't render D2D compnents with a non-D2D renderer");
            return renderer as Renderer;
        }
    }
}
