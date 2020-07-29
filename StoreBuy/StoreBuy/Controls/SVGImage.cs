using System;
using System.IO;
using System.Reflection;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace StoreBuy.Controls
{
    public class SVGImage : ContentView
    {
        
        public static readonly BindableProperty SourceProperty = BindableProperty.Create(
          nameof(Source), typeof(string), typeof(SVGImage), default(string), propertyChanged: RedrawCanvas);

        private readonly SKCanvasView canvasView = new SKCanvasView();

        public SVGImage()
        {
            this.Padding = new Thickness(0);
            this.BackgroundColor = Color.Transparent;
            this.Content = this.canvasView;
            this.canvasView.PaintSurface += this.CanvasView_PaintSurface;
        }

        
        public string Source
        {
            get => (string)this.GetValue(SourceProperty);
            set => this.SetValue(SourceProperty, value);
        }

        public static void RedrawCanvas(BindableObject bindable, object oldValue, object newValue)
        {
            SVGImage sVGImage = bindable as SVGImage;
            sVGImage?.canvasView.InvalidateSurface();
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKCanvas skCanvas = args.Surface.Canvas;
            skCanvas.Clear();

            if (string.IsNullOrEmpty(this.Source))
            {
                return;
            }

            var assembly = typeof(SVGImage).GetTypeInfo().Assembly.GetName();

            using (Stream stream = typeof(SVGImage).GetTypeInfo().Assembly.GetManifestResourceStream(assembly.Name + ".Images." + Source))
            {
                SkiaSharp.Extended.Svg.SKSvg skSVG = new SkiaSharp.Extended.Svg.SKSvg();
                skSVG.Load(stream);
                SKImageInfo imageInfo = args.Info;
                skCanvas.Translate(imageInfo.Width / 2f, imageInfo.Height / 2f);
                SKRect rectBounds = skSVG.ViewBox;
                float xRatio = imageInfo.Width / rectBounds.Width;
                float yRatio = imageInfo.Height / rectBounds.Height;
                float minRatio = Math.Min(xRatio, yRatio);
                skCanvas.Scale(minRatio);
                skCanvas.Translate(-rectBounds.MidX, -rectBounds.MidY);
                skCanvas.DrawPicture(skSVG.Picture);
            }
        }
    }
}
