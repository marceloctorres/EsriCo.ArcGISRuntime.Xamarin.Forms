using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;

using Prism.Commands;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Color = System.Drawing.Color;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DrawToolBarView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    private const string DrawGrapichsOverlayId = "DrawGraphicsOverlay";

    public static readonly BindableProperty ColorProperty = BindableProperty.Create(
      nameof(Color),
      typeof(Color),
      typeof(DrawToolBarView),
      defaultValue: Color.DarkCyan);

    /// <summary>
    /// 
    /// </summary>
    public Color Color
    {
      get => (Color)GetValue(ColorProperty);
      set => SetValue(ColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(
      nameof(Orientation),
      typeof(StackOrientation),
      typeof(DrawToolBarView),
      defaultValue: StackOrientation.Horizontal, propertyChanged: OnOrientationChanged);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as DrawToolBarView;
      var oldOrientation = (StackOrientation)oldValue;
      var newOrientation = (StackOrientation)newValue;
      if(oldOrientation != newOrientation)
      {
        view.SetControlTemplate(newOrientation);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="orientation"></param>
    private void SetControlTemplate(StackOrientation orientation)
    {
      if(orientation == StackOrientation.Horizontal)
      {
        ControlTemplate = (ControlTemplate)Resources[ "HorizontalLayoutTemplate" ];
      }
      else
      {
        ControlTemplate = (ControlTemplate)Resources[ "VerticalLayoutTemplate" ];
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public StackOrientation Orientation
    {
      get => (StackOrientation)GetValue(OrientationProperty);
      set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsDrawingProperty = BindableProperty.Create(
      nameof(IsDrawing),
      typeof(bool),
      typeof(DrawToolBarView),
      defaultValue: false,
      defaultBindingMode: BindingMode.OneWayToSource);

    /// <summary>
    /// 
    /// </summary>
    public bool IsDrawing
    {
      get => (bool)GetValue(IsDrawingProperty);
      set => SetValue(IsDrawingProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(DrawToolBarView),
      propertyChanged: OnMapViewPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    public MapView MapView
    {
      get => (MapView)GetValue(MapViewProperty);
      set => SetValue(MapViewProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnMapViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var panelView = bindable as DrawToolBarView;
      if(newValue is MapView newMapView)
      {
        if(newMapView.Map != null)
        {
          panelView.CheckMap(newMapView);
        }
        else
        {
          newMapView.PropertyChanged += (s, e) =>
          {
            if(e.PropertyName == nameof(newMapView.Map))
            {
              panelView.CheckMap(newMapView);
            }
          };
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    public void CheckMap(MapView mapView)
    {
      IsVisible = mapView != null && mapView.Map != null;
      if(IsVisible)
      {
        if(mapView.GraphicsOverlays == null)
        {
          mapView.GraphicsOverlays = new GraphicsOverlayCollection();
        }
        var graphicsOverlay = mapView.GraphicsOverlays.Where(g => g.Id == DrawGrapichsOverlayId).FirstOrDefault();
        if(graphicsOverlay == null)
        {
          DrawGraphicsOverlay.Graphics.Clear();
          mapView.GraphicsOverlays.Add(DrawGraphicsOverlay);
        }
        mapView.SketchEditor = SketchEditor;
        SketchEditor.Style.FeedbackVertexSymbol = null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawPointToolImageProperty = BindableProperty.Create(
      nameof(DrawPointToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawPointToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawPointToolImage
    {
      get => (ImageSource)GetValue(DrawPointToolImageProperty);
      set => SetValue(DrawPointToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawPointToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as NavigationBarView;
      if(newValue == null)
      {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_point"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawPolylineToolImageProperty = BindableProperty.Create(
      nameof(DrawPolylineToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawPolylineToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawPolylineToolImage
    {
      get => (ImageSource)GetValue(DrawPolylineToolImageProperty);
      set => SetValue(DrawPolylineToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawPolylineToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as NavigationBarView;
      if(newValue == null)
      {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_polyline"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawPolygonToolImageProperty = BindableProperty.Create(
      nameof(DrawPolygonToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawPolygonToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawPolygonToolImage
    {
      get => (ImageSource)GetValue(DrawPolygonToolImageProperty);
      set => SetValue(DrawPolygonToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawPolygonToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as NavigationBarView;
      if(newValue == null)
      {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_polygon"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawRectangleToolImageProperty = BindableProperty.Create(
      nameof(DrawRectangleToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawRectangleToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawRectangleToolImage
    {
      get => (ImageSource)GetValue(DrawRectangleToolImageProperty);
      set => SetValue(DrawRectangleToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawRectangleToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as NavigationBarView;
      if(newValue == null)
      {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_rectangle"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawEraseToolImageProperty = BindableProperty.Create(
      nameof(DrawEraseToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawEraseToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawEraseToolImage
    {
      get => (ImageSource)GetValue(DrawEraseToolImageProperty);
      set => SetValue(DrawEraseToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawEraseToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as NavigationBarView;
      if(newValue == null)
      {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_erase"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawFreehandLineToolImageProperty = BindableProperty.Create(
      nameof(DrawFreehandLineToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: DrawFreehandLineToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawFreehandLineToolImage
    {
      get => (ImageSource)GetValue(DrawFreehandLineToolImageProperty);
      set => SetValue(DrawFreehandLineToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void DrawFreehandLineToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as NavigationBarView;
      if(newValue == null)
      {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_erase"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawTextToolImageProperty = BindableProperty.Create(
      nameof(DrawTextToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: DrawTextToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawTextToolImage
    {
      get => (ImageSource)GetValue(DrawTextToolImageProperty);
      set => SetValue(DrawTextToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void DrawTextToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as NavigationBarView;
      if(newValue == null)
      {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_text"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private SketchEditor _sketchEditor;

    /// <summary>
    /// 
    /// </summary>
    /// <summary>
    /// 
    /// </summary>
    public SketchEditor SketchEditor
    {
      get => _sketchEditor;
      set
      {
        _sketchEditor = value;
        OnPropertyChanged(nameof(SketchEditor));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public ICommand OKCommand { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ModalPanelView Dialog { get; set; }

    /// <summary>
    /// 
    /// </summary>
    private GraphicsOverlay DrawGraphicsOverlay { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DrawToolBarView()
    {
      InitializeComponent();

      IsVisible = false;
      DrawPointToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_point"));
      DrawPolylineToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_polyline"));
      DrawPolygonToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_polygon"));
      DrawRectangleToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_rectangle"));
      DrawEraseToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_erase"));
      DrawFreehandLineToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_freehandline"));
      DrawTextToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_text"));

      DrawGraphicsOverlay = new GraphicsOverlay()
      {
        Id = DrawGrapichsOverlayId
      };
      SketchEditor = new SketchEditor();
      SketchEditor.GeometryChanged += SketchEditor_GeometryChanged;
      OKCommand = new DelegateCommand<string>((s) => DrawText(s));
      Dialog = new DrawTextToolDialog() { AcceptCommand = OKCommand };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SketchEditor_GeometryChanged(object sender, GeometryChangedEventArgs e)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    /// <returns></returns>
    private async Task DrawGeometry(SketchCreationMode mode, string text = null)
    {
      try
      {
        //if (IsDrawing)
        //{
        //  SketchEditor.Stop();
        //  if (SketchEditor.CancelCommand.CanExecute(null))
        //  {
        //    SketchEditor.CancelCommand.Execute(null);
        //  }
        //}
        IsDrawing = true;
        var geometry = await SketchEditor.StartAsync(mode, false);
        Symbol symbol = null;

        if(!string.IsNullOrEmpty(text))
        {
          symbol = TextSymbol(text);
        }
        else
        {
          switch(mode)
          {
            case SketchCreationMode.Point:
              symbol = PointSymbol();
              break;
            case SketchCreationMode.Multipoint:
              symbol = PointSymbol();
              break;
            case SketchCreationMode.Polyline:
              symbol = PolylineSymbol();
              break;
            case SketchCreationMode.FreehandLine:
              symbol = PolylineSymbol();
              break;
            default:
              symbol = PolygonSymbol();
              break;
          }
        }
        DrawGraphicsOverlay.Graphics.Add(new Graphic() { Geometry = geometry, Symbol = symbol });
        IsDrawing = false;
      }
      catch(TaskCanceledException ex)
      {
        Debug.WriteLine(ex.Message);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// 
    /// </summary>7
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawPointToolClicked(object sender, EventArgs e) => await DrawGeometry(SketchCreationMode.Multipoint);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawPolylineToolClicked(object sender, EventArgs e) => await DrawGeometry(SketchCreationMode.Polyline);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawFreehandLineToolClicked(object sender, EventArgs e) => await DrawGeometry(SketchCreationMode.FreehandLine);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="senter"></param>
    /// <param name="e"></param>
    private void DrawTextToolClicked(object senter, EventArgs e)
    {
      if(Parent is Layout<View> layout)
      {
        if(!layout.Children.Contains(Dialog))
        {
          layout.Children.Add(Dialog);
        }
        Dialog.IsVisible = true;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    private async void DrawText(string text) => await DrawGeometry(SketchCreationMode.Point, text);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawPolygonToolClicked(object sender, EventArgs e) => await DrawGeometry(SketchCreationMode.Polygon);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawRectangleToolClicked(object sender, EventArgs e) => await DrawGeometry(SketchCreationMode.Rectangle);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawEraseToolClicked(object sender, EventArgs e)
    {
      DrawGraphicsOverlay.Graphics.Clear();
      if(SketchEditor.CancelCommand.CanExecute(null))
      {
        SketchEditor.CancelCommand.Execute(null);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Symbol PointSymbol()
    {
      var fillColor = Color.FromArgb(128, Color);
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
      var fillColor = Color.FromArgb(128, Color);
      return new SimpleFillSymbol()
      {
        Color = fillColor,
        Outline = new SimpleLineSymbol()
        {
          Color = Color.DarkCyan,
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
  }
}