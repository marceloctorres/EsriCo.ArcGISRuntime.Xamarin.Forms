using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;

using Prism.Mvvm;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors
{
  /// <summary>
  /// 
  /// </summary>
  public class IdentifyGraphicResult : BindableBase
  {
    /// <summary>
    /// 
    /// </summary>
    private Graphic _graphic;

    /// <summary>
    ///
    /// </summary>
    public Graphic Graphic
    {
      get => _graphic;
      set => SetProperty(ref _graphic, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public MapPoint Center
    {
      get
      {
        if(Graphic != null && Graphic.Geometry != null)
        {
          return Graphic.Geometry.Extent.GetCenter();
        }
        return null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public string GraphicsOverlayId
    {
      get
      {
        if(Graphic != null)
        {
          return Graphic.GraphicsOverlay.Id;
        }
        return string.Empty;
      }
    }
  }
}
