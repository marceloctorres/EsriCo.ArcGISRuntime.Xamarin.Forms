using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;

using Prism.Mvvm;

using Color = System.Drawing.Color;


namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  /// <summary>
  /// 
  /// </summary>
  internal class DrawingProcess : BindableBase
  {
    /// <summary>
    /// 
    /// </summary>
    internal MapView MapView { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Color Color { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private SketchEditor _sketchEditor;

    /// <summary>
    /// 
    /// </summary>
    internal SketchEditor SketchEditor
    {
      get => _sketchEditor;
      set => SetProperty(ref _sketchEditor, value);
    }

    private bool _isDrawing;

    /// <summary>
    /// 
    /// </summary>
    internal bool IsDrawing
    {
      get => _isDrawing;
      set => SetProperty(ref _isDrawing, value);
    }

    /// <summary>
    /// 
    /// </summary>
    internal GraphicsOverlay DrawGraphicsOverlay { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry> PointCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry> MultiPointCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry> PolylineCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry> FreehandLineCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    internal Action<Geometry> PolygonCreated { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    internal async Task DrawGeometryAsync(SketchCreationMode mode, string text = null)
    {
      try
      {
        IsDrawing = true;
        MapView.SketchEditor = SketchEditor;

        Geometry geometry = await SketchEditor.StartAsync(mode, false);
        Symbol symbol = null;

        if (!string.IsNullOrEmpty(text))
        {
          symbol = TextSymbol(text);
        }
        else
        {
          switch (mode)
          {
            case SketchCreationMode.Point:
              symbol = PointSymbol();
              PointCreated?.Invoke(geometry);
              break;
            case SketchCreationMode.Multipoint:
              symbol = PointSymbol();
              MultiPointCreated?.Invoke(geometry);
              break;
            case SketchCreationMode.Polyline:
              symbol = PolylineSymbol();
              PolylineCreated?.Invoke(geometry);
              break;
            case SketchCreationMode.FreehandLine:
              symbol = PolylineSymbol();
              FreehandLineCreated?.Invoke(geometry);
              break;
            default:
              symbol = PolygonSymbol();
              PolygonCreated?.Invoke(geometry);
              break;
          }
        }
        DrawGraphicsOverlay?.Graphics.Add(new Graphic() { Geometry = geometry, Symbol = symbol });
        IsDrawing = false;
      }
      catch (TaskCanceledException ex)
      {
        Debug.WriteLine(ex.Message);
      }
      catch (Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
      finally
      {
        IsDrawing = false;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Symbol PointSymbol()
    {
      Color fillColor = Color.FromArgb(128, Color);
      return new SimpleMarkerSymbol()
      {
        Color = fillColor,
        Size = 20,
        Style = SimpleMarkerSymbolStyle.Circle
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Symbol PolylineSymbol() => new SimpleLineSymbol()
    {
      Color = Color,
      Style = SimpleLineSymbolStyle.Solid,
      Width = 2
    };

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Symbol PolygonSymbol()
    {
      Color fillColor = Color.FromArgb(128, Color);
      return new SimpleFillSymbol()
      {
        Color = fillColor,
        Outline = new SimpleLineSymbol()
        {
          Color = Color,
          Style = SimpleLineSymbolStyle.Solid,
          Width = 2
        },
        Style = SimpleFillSymbolStyle.Solid
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Symbol TextSymbol(string text) => new TextSymbol()
    {
      Text = text,
      Color = Color.Black,
      HorizontalAlignment = HorizontalAlignment.Center,
      VerticalAlignment = VerticalAlignment.Middle,
      Size = 20
    };

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args"></param>
    protected override void OnPropertyChanged(PropertyChangedEventArgs args)
    {
      base.OnPropertyChanged(args);
      if (args.PropertyName == nameof(SketchEditor))
      {
        SketchEditor.GeometryChanged += SketchEditor_GeometryChanged;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SketchEditor_GeometryChanged(object sender, GeometryChangedEventArgs e)
    {
      if (e.NewGeometry.Equals(e.OldGeometry))
      {
        Debug.WriteLine(e);
      }
    }
  }

}
