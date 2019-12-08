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
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(LayerListPanelView),
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
      var view = bindable as DrawingStatusView;
      var oldMapView = oldValue as MapView;
      var newMapView = newValue as MapView;
      view.AddDrawStatusChangedHandler(oldMapView, newMapView);
    }

    /// <summary>
    /// 
    /// </summary> c 
    /// <param name="oldMapView"></param>
    /// <param name="newMapView"></param>
    public void AddDrawStatusChangedHandler(MapView oldMapView, MapView newMapView)
    {
      if(oldMapView != null)
      {
        oldMapView.DrawStatusChanged -= MapViewDrawStatusChanged;
      }
      if(newMapView != null)
      {
        newMapView.DrawStatusChanged += MapViewDrawStatusChanged;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MapViewDrawStatusChanged(object sender, Esri.ArcGISRuntime.UI.DrawStatusChangedEventArgs e)
    {
      IsVisible = e.Status == DrawStatus.InProgress;
    }

    /// <summary>
    /// 
    /// </summary>
    public DrawingStatusView()
    {
      InitializeComponent();
    }
  }
}