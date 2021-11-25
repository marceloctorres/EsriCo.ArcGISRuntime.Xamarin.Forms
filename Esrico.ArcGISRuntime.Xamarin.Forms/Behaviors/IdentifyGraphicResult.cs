using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;

using Prism.Mvvm;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors {
  /// <summary>
  /// 
  /// </summary>
  public class IdentifyGraphicResult : BindableBase {
    /// <summary>
    /// 
    /// </summary>
    private Graphic _graphic;

    /// <summary>
    ///
    /// </summary>
    public Graphic Graphic {
      get => _graphic;
      set => SetProperty(ref _graphic, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public MapPoint Center => Graphic != null && Graphic.Geometry != null ? Graphic.Geometry.Extent.GetCenter() : null;

    /// <summary>
    /// 
    /// </summary>
    public string GraphicsOverlayId => Graphic != null ? Graphic.GraphicsOverlay.Id : string.Empty;
  }
}
