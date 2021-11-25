using System;
using System.Diagnostics;
using System.Windows.Input;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Xamarin.Forms;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI {
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class NavigationBarView : ContentView {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(NavigationBarView),
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
      var panelView = bindable as NavigationBarView;
      if(newValue is MapView newMapView) {
        if(newMapView.Map != null) {
          panelView.CheckMap(newMapView.Map);
        }
        else {
          newMapView.PropertyChanged += (s, e) => {
            if(e.PropertyName == nameof(newMapView.Map)) {
              panelView.CheckMap(newMapView.Map);
            }
          };
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    public void CheckMap(Map map) => IsVisible = map != null;

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ZoomInCommandProperty = BindableProperty.Create(
      nameof(ZoomInCommand),
      typeof(ICommand),
      typeof(NavigationBarView));

    /// <summary>
    /// 
    /// </summary>
    public ICommand ZoomInCommand {
      get => (ICommand)GetValue(ZoomInCommandProperty);
      set => SetValue(ZoomInCommandProperty, value);
    }

    public static readonly BindableProperty ZoomOutCommandProperty = BindableProperty.Create(
      nameof(ZoomOutCommand),
      typeof(ICommand),
      typeof(NavigationBarView));

    /// <summary>
    /// 
    /// </summary>
    public ICommand ZoomOutCommand {
      get => (ICommand)GetValue(ZoomOutCommandProperty);
      set => SetValue(ZoomOutCommandProperty, value);
    }

    public static readonly BindableProperty HomeCommandProperty = BindableProperty.Create(
      nameof(HomeCommand),
      typeof(ICommand),
      typeof(NavigationBarView));

    /// <summary>
    /// 
    /// </summary>
    public ICommand HomeCommand {
      get => (ICommand)GetValue(HomeCommandProperty);
      set => SetValue(HomeCommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ZoomInButtonImageProperty = BindableProperty.Create(
      nameof(ZoomInButtonImage),
      typeof(ImageSource),
      typeof(NavigationBarView),
      propertyChanged: OnZoomInButtonImageChanged);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnZoomInButtonImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as NavigationBarView;
      if(newValue == null) {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(NavigationBarView).Assembly.GetStreamEmbeddedResource(@"ic_plus"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public ImageSource ZoomInButtonImage {
      get => (ImageSource)GetValue(ZoomInButtonImageProperty);
      set => SetValue(ZoomInButtonImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ZoomOutButtonImageProperty = BindableProperty.Create(
      nameof(ZoomOutButtonImage),
      typeof(ImageSource),
      typeof(NavigationBarView),
      propertyChanged: OnZoomOutButtonImageChanged);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnZoomOutButtonImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as NavigationBarView;
      if(newValue == null) {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(NavigationBarView).Assembly.GetStreamEmbeddedResource(@"ic_minus"));
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public ImageSource ZoomOutButtonImage {
      get => (ImageSource)GetValue(ZoomOutButtonImageProperty);
      set => SetValue(ZoomOutButtonImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty HomeButtonImageProperty = BindableProperty.Create(
      nameof(HomeButtonImage),
      typeof(ImageSource),
      typeof(NavigationBarView),
      propertyChanged: OnHomeButonImageChanged);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnHomeButonImageChanged(BindableObject bindable, object oldValue, object newValue) {
      var view = bindable as NavigationBarView;
      if(newValue == null) {
        view.ZoomInButtonImage = ImageSource.FromStream(() =>
          typeof(NavigationBarView).Assembly.GetStreamEmbeddedResource(@"ic_home"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public ImageSource HomeButtonImage {
      get => (ImageSource)GetValue(HomeButtonImageProperty);
      set => SetValue(HomeButtonImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public double ZoomFactor { get; set; }

    public NavigationBarView() {
      try {
        InitializeComponent();
        ZoomFactor = 2.0;
        IsVisible = false;

        ZoomInButtonImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_plus"));
        ZoomOutButtonImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_minus"));
        HomeButtonImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_home"));

      }
      catch(Exception ex) {
        Debug.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="factor"></param>
    private async void UpdateViewpoint(double factor) {
      var viewpoint = MapView.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
      if(viewpoint != null) {
        var targetGeo = viewpoint.TargetGeometry as Envelope;
        var eb = new EnvelopeBuilder(targetGeo);
        eb.Expand(factor);

        viewpoint = new Viewpoint(eb.Extent);
        await MapView.SetViewpointAsync(viewpoint);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private async void SetMapInitialViewpoint() {
      if(MapView.Map != null) {
        await MapView.SetViewpointAsync(MapView.Map.InitialViewpoint);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ZoomInButtonClicked(object sender, EventArgs e) {
      if(ZoomInCommand != null) {
        if(ZoomInCommand.CanExecute(null)) {
          ZoomInCommand.Execute(null);
        }
      }
      else {
        UpdateViewpoint(1 / ZoomFactor);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HomeButtonClicked(object sender, EventArgs e) {
      if(HomeCommand != null) {
        if(HomeCommand.CanExecute(null)) {
          HomeCommand.Execute(null);
        }
      }
      else {
        SetMapInitialViewpoint();
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ZoomOutButtonClicked(object sender, EventArgs e) {
      if(ZoomOutCommand != null) {
        if(ZoomOutCommand.CanExecute(null)) {
          ZoomOutCommand.Execute(null);
        }
      }
      else {
        UpdateViewpoint(ZoomFactor);
      }

    }
  }
}