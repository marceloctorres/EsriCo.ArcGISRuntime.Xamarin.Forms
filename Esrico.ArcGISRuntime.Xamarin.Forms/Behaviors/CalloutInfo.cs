using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors
{
  public class CalloutInfo
  {
    /// <summary>
    /// 
    /// </summary>
    public GeoElement GeoElement { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public MapPoint Point { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public CalloutDefinition CalloutDefinition { get; set; }
  }
}
