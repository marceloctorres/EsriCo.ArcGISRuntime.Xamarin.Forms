using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;

using Prism.Commands;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI {
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DrawToolBarView : ContentView {
    /// <summary>
    /// 
    /// </summary>
    private const string DrawGrapichsOverlayId = "DrawGraphicsOverlay";

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(
      nameof(Color),
      typeof(Color),
      typeof(DrawToolBarView),
      propertyChanged: OnColorPropertyChanged,
      defaultValue: Color.DarkCyan);

    /// <summary>
    /// 
    /// </summary>
    public Color Color {
      get => (Color)GetValue(ColorProperty);
      set => SetValue(ColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      if(newValue != null) {
        view.Color = (Color)newValue;
        view.DrawingProcess.Color = view.Color;
      }
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
    private static void OnOrientationChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      var oldOrientation = (StackOrientation)oldValue;
      var newOrientation = (StackOrientation)newValue;
      if(oldOrientation != newOrientation) {
        view.SetControlTemplate(newOrientation);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="orientation"></param>
    private void SetControlTemplate(StackOrientation orientation) => ControlTemplate = orientation == StackOrientation.Horizontal
          ? (ControlTemplate)Resources["HorizontalLayoutTemplate"]
          : (ControlTemplate)Resources["VerticalLayoutTemplate"];

    /// <summary>
    /// 
    /// </summary>
    public StackOrientation Orientation {
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
    public bool IsDrawing {
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
    public MapView MapView {
      get => (MapView)GetValue(MapViewProperty);
      set => SetValue(MapViewProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnMapViewPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
      var panelView = bindable as DrawToolBarView;
      if(newValue is MapView newMapView) {
        if(newMapView.Map != null) {
          panelView.CheckMap(newMapView);
        }
        else {
          newMapView.PropertyChanged += (s, e) => {
            if(e.PropertyName == nameof(newMapView.Map)) {
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
    public void CheckMap(MapView mapView) {
      IsVisible = mapView != null && mapView.Map != null;
      if(IsVisible) {
        if(mapView.GraphicsOverlays == null) {
          mapView.GraphicsOverlays = new GraphicsOverlayCollection();
        }
        var graphicsOverlay = mapView.GraphicsOverlays.Where(g => g.Id == DrawGrapichsOverlayId).FirstOrDefault();
        if(graphicsOverlay == null) {
          DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
          mapView.GraphicsOverlays.Add(DrawingProcess.DrawGraphicsOverlay);
        }
        DrawingProcess.MapView = mapView;
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
    public ImageSource DrawPointToolImage {
      get => (ImageSource)GetValue(DrawPointToolImageProperty);
      set => SetValue(DrawPointToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawPointToolImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      if(newValue == null) {
        view.DrawPointToolImage = ImageSource.FromStream(() =>
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
    public ImageSource DrawPolylineToolImage {
      get => (ImageSource)GetValue(DrawPolylineToolImageProperty);
      set => SetValue(DrawPolylineToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawPolylineToolImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      if(newValue == null) {
        view.DrawPolylineToolImage = ImageSource.FromStream(() =>
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
    public ImageSource DrawPolygonToolImage {
      get => (ImageSource)GetValue(DrawPolygonToolImageProperty);
      set => SetValue(DrawPolygonToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawPolygonToolImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      if(newValue == null) {
        view.DrawPolygonToolImage = ImageSource.FromStream(() =>
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
    public ImageSource DrawRectangleToolImage {
      get => (ImageSource)GetValue(DrawRectangleToolImageProperty);
      set => SetValue(DrawRectangleToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawRectangleToolImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      if(newValue == null) {
        view.DrawRectangleToolImage = ImageSource.FromStream(() =>
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
    public ImageSource DrawEraseToolImage {
      get => (ImageSource)GetValue(DrawEraseToolImageProperty);
      set => SetValue(DrawEraseToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawEraseToolImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      if(newValue == null) {
        view.DrawEraseToolImage = ImageSource.FromStream(() =>
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
    public ImageSource DrawFreehandLineToolImage {
      get => (ImageSource)GetValue(DrawFreehandLineToolImageProperty);
      set => SetValue(DrawFreehandLineToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void DrawFreehandLineToolImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      if(newValue == null) {
        view.DrawFreehandLineToolImage = ImageSource.FromStream(() =>
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
    public ImageSource DrawTextToolImage {
      get => (ImageSource)GetValue(DrawTextToolImageProperty);
      set => SetValue(DrawTextToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void DrawTextToolImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      if(newValue == null) {
        view.DrawTextToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_text"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty DrawNoneToolImageProperty = BindableProperty.Create(
      nameof(DrawNoneToolImage),
      typeof(ImageSource),
      typeof(DrawToolBarView),
      propertyChanged: OnDrawNoneToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource DrawNoneToolImage {
      get => (ImageSource)GetValue(DrawNoneToolImageProperty);
      set => SetValue(DrawNoneToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnDrawNoneToolImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as DrawToolBarView;
      if(newValue == null) {
        view.DrawNoneToolImage = ImageSource.FromStream(() =>
          typeof(DrawToolBarView).Assembly.GetStreamEmbeddedResource(@"ic_cancel"));
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
    private DrawingProcess DrawingProcess { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Geometry Geometry { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public DrawToolBarView() {
      InitializeComponent();

      IsVisible = false;
      var asm = GetType().Assembly;

      DrawPointToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_point"));
      DrawPolylineToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_polyline"));
      DrawPolygonToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_polygon"));
      DrawRectangleToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_rectangle"));
      DrawEraseToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_erase"));
      DrawFreehandLineToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_freehandline"));
      DrawTextToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_text"));
      DrawNoneToolImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_cancel"));

      DrawingProcess = new DrawingProcess() {
        SketchEditor = new SketchEditor(),
        Color = Color,
        DrawGraphicsOverlay = new GraphicsOverlay() { Id = DrawGrapichsOverlayId }
      };
      DrawingProcess.SketchEditor.GeometryChanged += SketchEditor_GeometryChanged;
      DrawingProcess.PropertyChanged += DrawingProcess_PropertyChanged;

      OKCommand = new DelegateCommand<string>((s) => DrawText(s));
      Dialog = new DrawTextToolDialog() { AcceptCommand = OKCommand };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawingProcess_PropertyChanged(object sender, PropertyChangedEventArgs e) {
      if(e.PropertyName == nameof(DrawingProcess.IsDrawing) && DrawingProcess.IsDrawing) {
        IsDrawing = DrawingProcess.IsDrawing;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SketchEditor_GeometryChanged(object sender, GeometryChangedEventArgs e) {
      Debug.WriteLine("x");
      Debug.WriteLine($"NewGeometry: {e.NewGeometry.ToJson()}");
      if(Geometry != null && Geometry.Equals(e.NewGeometry)) {
        IsDrawing = false;
      }
      else {
        Geometry = e.NewGeometry;
      }
    }

    /// <summary>
    /// 
    /// </summary>7
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawPointToolClicked(object sender, EventArgs e) =>
      await DrawingProcess.DrawGeometryAsync(SketchCreationMode.Point);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawPolylineToolClicked(object sender, EventArgs e) =>
      await DrawingProcess.DrawGeometryAsync(SketchCreationMode.Polyline);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawFreehandLineToolClicked(object sender, EventArgs e) =>
      await DrawingProcess.DrawGeometryAsync(SketchCreationMode.FreehandLine);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="senter"></param>
    /// <param name="e"></param>
    private void DrawTextToolClicked(object senter, EventArgs e) {
      if(Parent is Layout<View> layout) {
        if(!layout.Children.Contains(Dialog)) {
          layout.Children.Add(Dialog);
        }
        Dialog.IsVisible = true;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    private async void DrawText(string text) =>
      await DrawingProcess.DrawGeometryAsync(SketchCreationMode.Point, text);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawPolygonToolClicked(object sender, EventArgs e) =>
      await DrawingProcess.DrawGeometryAsync(SketchCreationMode.Polygon);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void DrawRectangleToolClicked(object sender, EventArgs e) =>
      await DrawingProcess.DrawGeometryAsync(SketchCreationMode.Rectangle);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawEraseToolClicked(object sender, EventArgs e) {
      DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
      if(DrawingProcess.SketchEditor.CancelCommand.CanExecute(null)) {
        DrawingProcess.SketchEditor.CancelCommand.Execute(null);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawNoneToolClicked(object sender, EventArgs e) {
      DrawingProcess.SketchEditor.Stop();
      if(DrawingProcess.SketchEditor.CancelCommand != null && DrawingProcess.SketchEditor.CancelCommand.CanExecute(null)) {
        DrawingProcess.SketchEditor.CancelCommand.Execute(null);
      }
      IsDrawing = false;
    }
  }
}