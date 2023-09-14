using React.Box;
using React.Core;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;
using System;
using System.Linq;

namespace React.DirectRenderer
{
    public class Renderer : IDisposable, IRenderer
    {
        private readonly IntPtr hwnd;
        
        public readonly SharpDX.DXGI.SwapChain swapChain;
        public readonly SharpDX.Direct2D1.WindowRenderTarget d2dTarget;
        public readonly SharpDX.DirectWrite.Factory fontFactory;
        public readonly SharpDX.Direct2D1.Factory d2dFactory;

        private Bounds currentBounds;
        
        public void Resize(Bounds bounds)
        {
            currentBounds = bounds;
        }

        public Renderer(IntPtr hwnd, Bounds b)
        {
            this.hwnd = hwnd;
            this.currentBounds = b;
            d2dFactory = new SharpDX.Direct2D1.Factory(FactoryType.SingleThreaded);
            fontFactory = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);

            var properties = new RenderTargetProperties(new PixelFormat(Format.R8G8B8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            d2dTarget = new SharpDX.Direct2D1.WindowRenderTarget(d2dFactory, properties, new HwndRenderTargetProperties
            {
                Hwnd = hwnd,
                PixelSize = new Size2(currentBounds.Width, currentBounds.Height),
                PresentOptions = PresentOptions.Immediately
            });
        }

        public void RenderFrame(IElementState root)
        {
            d2dTarget.BeginDraw();
            d2dTarget.Clear(new SharpDX.Mathematics.Interop.RawColor4(32 / 255f, 103 / 255f, 178 / 255f, 1f));
            root.Render(this);
            d2dTarget.Flush();
            d2dTarget.EndDraw();
        }
        
        public void Dispose()
        {
            foreach (var disposable in typeof(Renderer).GetFields().Select(p => p.GetValue(this) as IDisposable))
                disposable?.Dispose();
        }
        
        public ITextElementState UpdateTextElementState(IElementState existing, TextElement t, RenderContext context, Bounds bounds)
        {
            return new TextElementState(existing as TextElementState, t, context, bounds);
        }

        public IElementState UpdateSolidColourElementState(IElementState existing, SolidColourElement b2, RenderContext context, Bounds bounds)
        {
            return new SolidColourElementState(existing as SolidColourElementState, b2, context, bounds);
        }

        public IElementState UpdateImageElementState(IElementState existing, ImageElement b2, RenderContext context, Bounds bounds)
        {
            return new ImageElementState(existing as ImageElementState, b2, context, bounds);
        }

        public static Renderer AssertRendererType(IRenderer renderer)
        {
            if (!(renderer is Renderer)) throw new InvalidOperationException("Can't render D2D compnents with a non-D2D renderer");
            return renderer as Renderer;
        }
    }
}
