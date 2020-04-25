using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Mapping;

using Prism.Mvvm;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Model
{
  /// <summary>
  /// 
  /// </summary>
  public class LegendImageInfo : BindableBase
  {
    private ImageSource _imageSource;
    private string _name;

    /// <summary>
    /// 
    /// </summary>
    public ImageSource ImageSource
    {
      get => _imageSource;
      set => SetProperty(ref _imageSource, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public string Name
    {
      get => _name;
      set => SetProperty(ref _name, value);
    }
  }


  /// <summary>
  /// 
  /// </summary>
  public class LayerInfo : BindableBase
  {
    private Layer _layer;
    private LayerInfo _parentInfoLayer;
    private List<LegendImageInfo> _legendInfos;


    /// <summary>
    /// 
    /// </summary>
    public Layer Layer
    {
      get => _layer;
      set => SetProperty(ref _layer, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public LayerInfo ParentInfo
    {
      get => _parentInfoLayer;
      set => SetProperty(ref _parentInfoLayer, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public List<LegendImageInfo> LegendImageInfos
    {
      get => _legendInfos;
      set => SetProperty(ref _legendInfos, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="legendInfos"></param>
    public async Task SetLegendInfosAsync(IEnumerable<LegendInfo> legendInfos)
    {
      if(LegendImageInfos == null)
      {
        LegendImageInfos = new List<LegendImageInfo>();
      }
      else
      {
        LegendImageInfos.Clear();
      }

      foreach(var li in legendInfos.ToList())
      {
        var imageData = await li.Symbol.CreateSwatchAsync();
        var stream = await imageData.GetEncodedBufferAsync();
        LegendImageInfos.Add(new LegendImageInfo()
        {
          Name = li.Name,
          ImageSource = ImageSource.FromStream(() => stream)
        });
      }
    }
  }

  /// <summary>
  /// 
  /// </summary>
  public class LayerInfos : List<LayerInfo>
  {
    /// <summary>
    /// 
    /// </summary>
    public LayerInfo GroupLayerInfo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<LayerInfo> SubLayerInfos => this;
  }
}
