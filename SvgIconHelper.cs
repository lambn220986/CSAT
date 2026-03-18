using SkiaSharp;
using Svg.Skia;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CSAT
{
    public static class SvgIconHelper
    {
        public static Icon SvgToIcon(string svgPath, int size = 64)
        {
            var svg = new SKSvg();
            svg.Load(svgPath);

            var bitmap = new SKBitmap(size, size);
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Transparent);

                var scaleX = size / svg.Picture.CullRect.Width;
                var scaleY = size / svg.Picture.CullRect.Height;
                var scale = Math.Min(scaleX, scaleY);

                canvas.Scale(scale);
                canvas.DrawPicture(svg.Picture);
            }

            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var ms = new MemoryStream(data.ToArray()))
            {
                using (var bmp = new Bitmap(ms))
                {
                    return Icon.FromHandle(bmp.GetHicon());
                }
            }
        }
        public static void LoadSvgToPictureBox(PictureBox pic, string file, Color color)
        {
            var svg = new SKSvg();
            svg.Load(file);

            var bitmap = new SKBitmap(pic.Width, pic.Height);

            using (var canvas = new SKCanvas(bitmap))
            using (var paint = new SKPaint())
            {
                canvas.Clear(SKColors.Transparent);

                var scaleX = pic.Width / svg.Picture.CullRect.Width;
                var scaleY = pic.Height / svg.Picture.CullRect.Height;
                var scale = Math.Min(scaleX, scaleY);
                canvas.Scale(scale);
                canvas.DrawPicture(svg.Picture, paint);
            }

            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            using (var ms = new MemoryStream(data.ToArray()))
            {
                var old = pic.Image;
                pic.Image = new Bitmap(ms);
                old?.Dispose();
            }

            bitmap.Dispose();
        }
    }
}
