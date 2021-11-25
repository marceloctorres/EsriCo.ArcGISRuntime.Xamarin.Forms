using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;

using Prism.Mvvm;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors {
  /// <summary>
  /// 
  /// </summary>
  public class IdentifyGeoElementResult : BindableBase {
    private GeoElement _geoElement;

    /// <summary>
    /// 
    /// </summary>
    public GeoElement GeoElement {
      get => _geoElement;
      set => SetProperty(ref _geoElement, value);
    }

    private Layer _layer;

    /// <summary>
    /// 
    /// </summary>
    public Layer Layer {
      get => _layer;
      set => SetProperty(ref _layer, value);
    }

    public IdentifyGeoElementResult() {

    }

  }
}
