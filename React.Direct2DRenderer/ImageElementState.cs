using React.Core;
using SharpDX.Direct2D1;
using System.Collections.Generic;

namespace React.DirectRenderer
{
    public class ImageElementState : IKnownSizeElementState
    {
        private readonly Bitmap rawBitmap;
        private readonly System.Drawing.Image sourceImage;

        public ImageElementState(ImageElementState existing, ImageElement other, RenderContext context)
        {
            if (existing != null && ReferenceEquals(existing.sourceImage, other.Props.Image))
            {
                rawBitmap = existing.rawBitmap;
                sourceImage = existing.sourceImage;
            }
            else
            {
                var target = Renderer.AssertRendererType(context.Renderer).d2dTarget;
                using (var bmp = new System.Drawing.Bitmap(other.Props.Image))
                {
                    var bmpData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    using (var stream = new SharpDX.DataStream(bmpData.Scan0, bmpData.Stride * bmpData.Height, true, false))
                    {
                        var pFormat = new PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied);
                        var bmpProps = new BitmapProperties(pFormat);

                        rawBitmap = new Bitmap(target, new SharpDX.Size2(bmp.Width, bmp.Height), stream, bmpData.Stride, bmpProps);
                        bmp.UnlockBits(bmpData);
                        sourceImage = other.Props.Image;

                        stream.Dispose();
                    }
                }
            }
            
            context.Disposables.Add(other.Props.Image);
            context.Disposables.Add(rawBitmap);
        }

        public Dimensions Size => new Dimensions(sourceImage.Width, sourceImage.Height);

        public void FireEvents(IReadOnlyList<IEvent> events)
        {
        }

        public void Render(IRenderer r, Bounds bounds)
        {
            Renderer.AssertRendererType(r).d2dTarget.DrawBitmap(
                rawBitmap,
                new SharpDX.Mathematics.Interop.RawRectangleF(bounds.X, bounds.Y, bounds.X + bounds.Width, bounds.Y + bounds.Height),
                1.0f,
                BitmapInterpolationMode.Linear,
                null);
        }
    }
}
