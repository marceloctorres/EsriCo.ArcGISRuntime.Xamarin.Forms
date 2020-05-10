using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Services
{
  /// <summary>
  /// 
  /// </summary>
  public interface IConfigurationInfo
  {
    /// <summary>
    /// 
    /// </summary>
    string FileName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    string Folder { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task LoadAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
  }
}