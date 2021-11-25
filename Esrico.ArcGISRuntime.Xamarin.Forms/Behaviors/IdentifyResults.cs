using System.Collections.Generic;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors {
  public class IdentifyResults {
    /// <summary>
    /// 
    /// </summary>
    public List<IdentifyGraphicResult> GraphicsResults { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<IdentifyGeoElementResult> GeoElementResults { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IdentifyResults() {
      GraphicsResults = new List<IdentifyGraphicResult>();
      GeoElementResults = new List<IdentifyGeoElementResult>();
    }
  }
}
