using System;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Mapping;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Services
{
  public interface IReplicaManager
  {
    string AppFolderName { get; set; }
    double MaxScale { get; set; }
    double MinScale { get; set; }
    MobileMapPackage MobileMapPackage { get; set; }
    string ReplicaFolderName { get; }
    Task DeleteReplicaAsync();
    Task<string> DeleteReplicaAsync(Map map);
    Task DeleteReplicaFolderAsync();
    Task<DownloadReplicaResult> DownloadReplicaAsync(Map map, Viewpoint viewpoint, EventHandler<JobChangedEventArgs> jobHandler, EventHandler<ProgressChangedEventArgs> progressHandler);
    Task<Map> GetReplicaMapAsync();
    bool ReplicaExist();
    Task<SynchronizeReplicaResult> SynchronizeReplicaAsync(Map map, EventHandler<JobChangedEventArgs> jobHandler, EventHandler<ProgressChangedEventArgs> progressHandler);
    Task<ValidateReplicaResult> ValidateReplicaAsync(Map map, Viewpoint viewpoint);
  }
}