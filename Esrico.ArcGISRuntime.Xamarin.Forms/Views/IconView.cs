using SkiaSharp;
using SkiaSharp.Extended.Svg;
using SkiaSharp.Views.Forms;

using System;
using System.IO;
using System.Reflection;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Views
{
  using Extensions;
  using SKSvg = SkiaSharp.Extended.Svg.SKSvg;

  public class IconView : Frame
  {
    /// <summary>
    /// 
    /// </summary>
    private readonly SKCanvasView canvasView = new SKCanvasView();

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ResourceIdProperty = BindableProperty.Create(
      nameof(ResourceId),
      typeof(string),
      typeof(IconView),
      default(string),
      propertyChanged: RedrawCanvas);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void RedrawCanvas(BindableObject bindable, object oldValue, object newValue)
    {
      var icon = bindable as IconView;
      icon?.canvasView.InvalidateSurface();
    }

    /// <summary>
    /// 
    /// </summary>
    public string ResourceId
    {
      get => (string)GetValue(ResourceIdProperty);
      set => SetValue(ResourceIdProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public IconView()
    {
      Padding = new Thickness(0);
      HasShadow = false;

      Content = canvasView;
      canvasView.PaintSurface += CanvasView_PaintSurface;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
      var canvas = e.Surface.Canvas;
      canvas.Clear();
      if (string.IsNullOrEmpty(ResourceId))
      {
        return;
      }
      var currentAssembly = Assembly.GetExecutingAssembly();
      using (Stream stream = currentAssembly.GetStreamEmbeddedResource(ResourceId))
      {
        SKSvg svg = new SKSvg();
        svg.Load(stream);

        SKImageInfo info = e.Info;
        canvas.Translate(info.Width / 2f, info.Height / 2f);

        SKRect bounds = svg.ViewBox;
        float xRatio = info.Width / bounds.Width;
        float yRatio = info.Height / bounds.Height;

        float ratio = Math.Min(xRatio, yRatio);
        canvas.Scale(ratio);
        canvas.Translate(-bounds.MidX, -bounds.MidY);
        canvas.DrawPicture(svg.Picture);
      }
    }
  }
}