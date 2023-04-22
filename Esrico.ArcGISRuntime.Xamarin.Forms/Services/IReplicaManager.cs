using System;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Mapping;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Services {
  /// <summary>
  /// 
  /// </summary>
  public interface IReplicaManager {
    /// <summary>
    /// 
    /// </summary>
    string AppFolderName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    double MaxScale { get; set; }

    /// <summary>
    /// 
    /// </summary>
    double MinScale { get; set; }

    /// <summary>
    /// 
    /// </summary>
    MobileMapPackage MobileMapPackage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    string ReplicaFolderName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task DeleteReplicaAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    Task<string> DeleteReplicaAsync(Map map);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task DeleteReplicaFolderAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <param name="viewpoint"></param>
    /// <param name="jobHandler"></param>
    /// <param name="progressHandler"></param>
    /// <returns></returns>
    Task<DownloadReplicaResult> DownloadReplicaAsync(Map map, Viewpoint viewpoint, EventHandler<JobChangedEventArgs> jobHandler, EventHandler<ProgressChangedEventArgs> progressHandler);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<Map> GetReplicaMapAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool ReplicaExist();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <param name="jobHandler"></param>
    /// <param name="progressHandler"></param>
    /// <returns></returns>
    Task<SynchronizeReplicaResult> SynchronizeReplicaAsync(Map map, EventHandler<JobChangedEventArgs> jobHandler, EventHandler<ProgressChangedEventArgs> progressHandler);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <param name="viewpoint"></param>
    /// <returns></returns>
    Task<ValidateReplicaResult> ValidateReplicaAsync(Map map, Viewpoint viewpoint);
  }
}