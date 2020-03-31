using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class MeasurementBarView : PanelView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(
      nameof(Color),
      typeof(Color),
      typeof(MeasurementBarView),
      defaultValue: Color.DarkCyan,
      propertyChanged: OnColorPropertyChanged);

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
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue != null)
      {
        view.Color = (Color)newValue;
        view.DrawingProcess.Color = view.Color;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private const string DrawGrapichsOverlayId = "MeasurementGraphicsOverlay";

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty PointMeasurementToolImageProperty = BindableProperty.Create(
      nameof(PointMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnPointMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource PointMeasurementToolImage
    {
      get => (ImageSource)GetValue(PointMeasurementToolImageProperty);
      set => SetValue(PointMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnPointMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue == null)
      {
        view.PointMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_coord"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty LineMeasurementToolImageProperty = BindableProperty.Create(
      nameof(LineMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnLineMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource LineMeasurementToolImage
    {
      get => (ImageSource)GetValue(LineMeasurementToolImageProperty);
      set => SetValue(LineMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnLineMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue == null)
      {
        view.LineMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_distance"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty AreaMeasurementToolImageProperty = BindableProperty.Create(
      nameof(AreaMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnAreaMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource AreaMeasurementToolImage
    {
      get => (ImageSource)GetValue(AreaMeasurementToolImageProperty);
      set => SetValue(AreaMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnAreaMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue == null)
      {
        view.AreaMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_area"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(MeasurementBarView),
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
      var panelView = bindable as MeasurementBarView;
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
          DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
          mapView.GraphicsOverlays.Add(DrawingProcess.DrawGraphicsOverlay);
        }
        if(mapView.SketchEditor == null)
        {
          mapView.SketchEditor = new SketchEditor();
        }
        DrawingProcess.SketchEditor = mapView.SketchEditor;
        DrawingProcess.SketchEditor.GeometryChanged += SketchEditor_GeometryChanged;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsDrawingProperty = BindableProperty.Create(
      nameof(IsDrawing),
      typeof(bool),
      typeof(MeasurementBarView),
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
    private DrawingProcess DrawingProcess { get; set; }

    private string _text;

    /// <summary>
    /// 
    /// </summary>
    public string ResultText
    {
      get => _text;
      set 
      { _text = value; 
        OnPropertyChanged(nameof(ResultText)); 
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public MeasurementBarView()
    {
      InitializeComponent();
      IsVisible = false;

      PointMeasurementToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_coord"));
      LineMeasurementToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_distance"));
      AreaMeasurementToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_area"));

      DrawingProcess = new DrawingProcess()
      {
        Color = Color,
        DrawGraphicsOverlay = new GraphicsOverlay() { Id = DrawGrapichsOverlayId }
      };
      DrawingProcess.PropertyChanged += DrawingProcess_PropertyChanged;

      DrawingProcess.PointCreated = PointCreated;
    }

    /// <param name="e"></param>
    private void SketchEditor_GeometryChanged(object sender, GeometryChangedEventArgs e)
    {
      Debug.WriteLine(e.NewGeometry);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="geometry"></param>
    private void PointCreated(Geometry geometry)
    {
      ResultText = "Punto";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawingProcess_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if(e.PropertyName == nameof(DrawingProcess.IsDrawing))
      {
        IsDrawing = DrawingProcess.IsDrawing;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void PointMeasurementToolClicked(object sender, EventArgs e) {
      DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
      await DrawingProcess.DrawGeometry(SketchCreationMode.Point);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void LineMeasurementToolClicked(object sender, EventArgs e)
    {
      DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
      await DrawingProcess.DrawGeometry(SketchCreationMode.Polyline);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void AreaMeasurementToolClicked(object sender, EventArgs e)
    {
      DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
      await DrawingProcess.DrawGeometry(SketchCreationMode.Polygon);
    }
  }
}