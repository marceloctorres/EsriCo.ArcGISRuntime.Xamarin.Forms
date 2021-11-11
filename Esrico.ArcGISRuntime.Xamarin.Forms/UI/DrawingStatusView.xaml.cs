using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DrawingStatusView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IndicatorColorProperty = BindableProperty.Create(
      nameof(IndicatorColorProperty),
      typeof(Color),
      typeof(DrawingStatusView),
      propertyChanged: OnIndicatorColorPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnIndicatorColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      DrawingStatusView view = bindable as DrawingStatusView;
      Color newColor = (Color)newValue;
      view.IndicatorColor = newColor;
    }

    /// <summary>
    /// 
    /// </summary>
    public Color IndicatorColor
    {
      get => (Color)GetValue(IndicatorColorProperty);
      set
      {
        SetValue(IndicatorColorProperty, value);
        OnPropertyChanged(nameof(IndicatorColor));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(DrawingStatusView),
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
      DrawingStatusView view = bindable as DrawingStatusView;
      MapView oldMapView = oldValue as MapView;
      MapView newMapView = newValue as MapView;
      view.AddDrawStatusChangedHandler(oldMapView, newMapView);
    }

    /// <summary>
    /// 
    /// </summary> c 
    /// <param name="oldMapView"></param>
    /// <param name="newMapView"></param>
    public void AddDrawStatusChangedHandler(MapView oldMapView, MapView newMapView)
    {
      if (oldMapView != null)
      {
        oldMapView.DrawStatusChanged -= MapViewDrawStatusChanged;
      }
      if (newMapView != null)
      {
        newMapView.DrawStatusChanged += MapViewDrawStatusChanged;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MapViewDrawStatusChanged(object sender, DrawStatusChangedEventArgs e) => IsVisible = e.Status == DrawStatus.InProgress;

    /// <summary>
    /// 
    /// </summary>
    public DrawingStatusView()
    {
      InitializeComponent();
      IsVisible = false;
      IndicatorColor = (Color)ActivityIndicator.ColorProperty.DefaultValue;
    }
  }
}