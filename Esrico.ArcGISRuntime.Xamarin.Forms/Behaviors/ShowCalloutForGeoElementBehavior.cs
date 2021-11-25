using System.Runtime.CompilerServices;

using Esri.ArcGISRuntime.Xamarin.Forms;

using Prism.Behaviors;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors {
  /// <summary>
  /// 
  /// </summary>
  public class ShowCalloutForGeoElementBehavior : BehaviorBase<MapView> {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CalloutInfoProperty = BindableProperty
      .Create(nameof(CalloutInfo), typeof(CalloutInfo), typeof(ShowCalloutForGeoElementBehavior));

    /// <summary>
    /// 
    /// </summary>
    public CalloutInfo CalloutInfo {
      get => (CalloutInfo)GetValue(CalloutInfoProperty);
      set => SetValue(CalloutInfoProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null) {
      base.OnPropertyChanged(propertyName);
      if(propertyName == nameof(CalloutInfo)) {
        ShowCallout();
      }

    }

    /// <summary>
    /// 
    /// </summary>
    private void ShowCallout() {
      if(CalloutInfo != null) {
        var punto = AssociatedObject.LocationToScreen(CalloutInfo.Point);
        AssociatedObject.ShowCalloutForGeoElement(CalloutInfo.GeoElement, punto, CalloutInfo.CalloutDefinition);
      }
      else {
        AssociatedObject.DismissCallout();
      }
    }
  }
}
