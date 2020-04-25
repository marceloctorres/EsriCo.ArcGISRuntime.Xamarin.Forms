using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Http;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Tasks;
using Esri.ArcGISRuntime.Tasks.Offline;

using Xamarin.Essentials;

using Map = Esri.ArcGISRuntime.Mapping.Map;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Services
{
  /// <summary>
  /// 
  /// </summary>
  public class ValidateReplicaResult
  {
    /// <summary>
    /// 
    /// </summary>
    public bool Valid { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int Tiles { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double Size { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class DownloadReplicaErrorResult
  {
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool SupportOffline { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Exception Error { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; }
  }
  /// <summary>
  /// 
  /// </summary>
  public class DownloadReplicaResult
  {
    /// <summary>
    /// 
    /// </summary>
    public Map Map { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string MapTitle { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<DownloadReplicaErrorResult> ResultErrors { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public DownloadReplicaResult() => ResultErrors = new List<DownloadReplicaErrorResult>();
  }

  /// <summary>
  /// 
  /// </summary>
  public class SyncReplicaErrorResult
  {
    /// <summary>
    /// 
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Exception Error { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Message { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class SynchronizeReplicaResult
  {
    /// <summary>
    /// 
    /// </summary>
    public bool Synchronized { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<SyncReplicaErrorResult> ResultErrors;

    /// <summary>
    /// 
    /// </summary>
    public SynchronizeReplicaResult() => ResultErrors = new List<SyncReplicaErrorResult>();

  }

  /// <summary>
  /// 
  /// </summary>
  public class ProgressChangedEventArgs : EventArgs
  {
    /// <summary>
    /// 
    /// </summary>
    public int Progress { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class JobChangedEventArgs : EventArgs
  {
    /// <summary>
    /// 
    /// </summary>
    public JobStatus Status { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<JobMessage> Messages { get; set; }
  }

  /// <summary>
  /// 
  /// </summary>
  public class ReplicaManager
  {
    private static event EventHandler<JobChangedEventArgs> JobChangedEventHandler;
    private static event EventHandler<ProgressChangedEventArgs> ProgressChangedEventHandler;

    /// <summary>
    /// 
    /// </summary>
    public ReplicaManager()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    public string AppFolderName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string ReplicaFolderName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double MinScale { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public double MaxScale { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public MobileMapPackage MobileMapPackage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <param name="viewpoint"></param>
    /// <returns></returns>
    [Obsolete]
    public async Task<ValidateReplicaResult> ValidateReplica(Map map, Viewpoint viewpoint)
    {
      return await ValidateReplicaAsync(map, viewpoint);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <param name="viewpoint"></param>
    /// <returns></returns>
    public async Task<ValidateReplicaResult> ValidateReplicaAsync(Map map, Viewpoint viewpoint)
    {
      var firstLayer = map.Basemap.BaseLayers.FirstOrDefault() as Layer;
      string basemapUrl = string.Empty;

      if(firstLayer is ArcGISTiledLayer)
      {
        var baseLayer = firstLayer as ArcGISTiledLayer;
        basemapUrl = baseLayer.Source.AbsoluteUri;
      }
      if(!string.IsNullOrEmpty(basemapUrl))
      {
        var newBasemapUrl = basemapUrl.Contains("services.arcgisonline") ?
          basemapUrl.Replace("services.arcgisonline", "tiledbasemaps.arcgis") :
          basemapUrl;
        var task = await ExportTileCacheTask.CreateAsync(new Uri(newBasemapUrl));

        var param = await task.CreateDefaultExportTileCacheParametersAsync(viewpoint.TargetGeometry, MinScale, MaxScale);
        var job = task.EstimateTileCacheSize(param);

        try
        {
          var result = await job.GetResultAsync();
          return new ValidateReplicaResult
          {
            Valid = result.TileCount < task.ServiceInfo.MaxExportTilesCount,
            Tiles = (int)result.TileCount,
            Size = result.FileSize
          };
        }
        catch(ArcGISRuntimeException ex)
        {
          Debug.WriteLine(ex.Message);
          return new ValidateReplicaResult();
        }

      }
      return new ValidateReplicaResult() { Valid = true };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <param name="viewpoint"></param>
    /// <param name="jobHandler"></param>
    /// <param name="progressHandler"></param>
    /// <returns></returns>
    [Obsolete]
    public async Task<DownloadReplicaResult> DownloadReplica(Map map, Viewpoint viewpoint,
      EventHandler<JobChangedEventArgs> jobHandler, EventHandler<ProgressChangedEventArgs> progressHandler)
    {
      return await DownloadReplicaAsync(map, viewpoint, jobHandler, progressHandler);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <param name="viewpoint"></param>
    /// <param name="jobHandler"></param>
    /// <param name="progressHandler"></param>
    /// <returns></returns>
    public async Task<DownloadReplicaResult> DownloadReplicaAsync(Map map, Viewpoint viewpoint,
      EventHandler<JobChangedEventArgs> jobHandler, EventHandler<ProgressChangedEventArgs> progressHandler)
    {
      ValidateReplicaFolderPath();
      var pathToOutputPackage = GetReplicaFullPath();
      var areaOfInterest = viewpoint.TargetGeometry;

      var task = await OfflineMapTask.CreateAsync(map);
      var parameters = await task.CreateDefaultGenerateOfflineMapParametersAsync(areaOfInterest);

      parameters.MinScale = MinScale;
      parameters.MaxScale = MaxScale;
      parameters.AttachmentSyncDirection = AttachmentSyncDirection.Bidirectional;
      parameters.ReturnLayerAttachmentOption = ReturnLayerAttachmentOption.AllLayers;
      parameters.ReturnSchemaOnlyForEditableLayers = false;
      parameters.ItemInfo.Title = $"{parameters.ItemInfo.Title} (Off-line)";

      var errors = new List<DownloadReplicaErrorResult>();
      var capabilitiesResults = await task.GetOfflineMapCapabilitiesAsync(parameters);
      if(capabilitiesResults.HasErrors)
      {
        errors.AddRange(capabilitiesResults.LayerCapabilities.ToList()
          .Where(l => !l.Value.SupportsOffline || l.Value.Error != null)
          .Select(l => new DownloadReplicaErrorResult
          {
            Name = l.Key.Name,
            SupportOffline = l.Value.SupportsOffline,
            Error = l.Value.Error
          }));
        errors.AddRange(capabilitiesResults.TableCapabilities.ToList()
          .Where(t => !t.Value.SupportsOffline || t.Value.Error != null)
          .Select(t => new DownloadReplicaErrorResult
          {
            Name = t.Key.TableName,
            SupportOffline = t.Value.SupportsOffline,
            Error = t.Value.Error
          }));
        return new DownloadReplicaResult
        {
          ResultErrors = errors
        };
      }
      else
      {
        var job = task.GenerateOfflineMap(parameters, pathToOutputPackage);
        if(jobHandler != null)
        {
          JobChangedEventHandler += jobHandler;
          job.JobChanged += (o, e) =>
          {
            JobChangedEventHandler?.Invoke(job, new JobChangedEventArgs() { Messages = job.Messages, Status = job.Status });
          };
        }
        if(progressHandler != null)
        {
          ProgressChangedEventHandler += progressHandler;
          job.ProgressChanged += (o, e) =>
          {
            ProgressChangedEventHandler?.Invoke(job, new ProgressChangedEventArgs() { Progress = job.Progress });
          };
        }
        var generateOfflineMapResults = await job.GetResultAsync();
        if(!generateOfflineMapResults.HasErrors)
        {
          var okMessage = $"{AppResources.DownloadReplicaOkMessageMap} " +
            $"{generateOfflineMapResults.MobileMapPackage.Item.Title} " +
            $"{AppResources.DownloadReplicaOkMessageSaved}.";
          MobileMapPackage = generateOfflineMapResults.MobileMapPackage;
          return new DownloadReplicaResult
          {
            Map = generateOfflineMapResults.OfflineMap,
            MapTitle = okMessage
          };
        }
        else
        {
          errors.AddRange(generateOfflineMapResults.LayerErrors.ToList()
            .Select(l => new DownloadReplicaErrorResult
            {
              Name = l.Key.Name,
              Message = l.Value.Message
            }));
          errors.AddRange(generateOfflineMapResults.TableErrors.ToList()
            .Select(t => new DownloadReplicaErrorResult
            {
              Name = t.Key.TableName,
              Message = t.Value.Message
            }));
          return new DownloadReplicaResult
          {
            ResultErrors = errors
          };
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <param name="jobHandler"></param>
    /// <param name="progressHandler"></param>
    /// <returns></returns>
    [Obsolete]
    public async Task<SynchronizeReplicaResult> SynchronizeReplica(Map map, EventHandler<JobChangedEventArgs> jobHandler,
      EventHandler<ProgressChangedEventArgs> progressHandler)
    {
      return await SynchronizeReplicaAsync(map, jobHandler, progressHandler);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <param name="jobHandler"></param>
    /// <param name="progressHandler"></param>
    /// 
    /// <returns></returns>
    public async Task<SynchronizeReplicaResult> SynchronizeReplicaAsync(Map map, EventHandler<JobChangedEventArgs> jobHandler,
      EventHandler<ProgressChangedEventArgs> progressHandler)
    {
      var errors = new List<SyncReplicaErrorResult>();
      var task = await OfflineMapSyncTask.CreateAsync(map);
      var param = new OfflineMapSyncParameters()
      {
        RollbackOnFailure = true,
        SyncDirection = SyncDirection.Bidirectional
      };

      var job = task.SyncOfflineMap(param);
      if(jobHandler != null)
      {
        JobChangedEventHandler += jobHandler;
        job.JobChanged += (o, e) =>
        {
          if(JobChangedEventHandler != null)
          {
            JobChangedEventHandler.Invoke(job, new JobChangedEventArgs() { Messages = job.Messages, Status = job.Status });
          }
        };
      }
      if(progressHandler != null)
      {
        ProgressChangedEventHandler += progressHandler;
        job.ProgressChanged += (o, e) =>
        {
          ProgressChangedEventHandler?.Invoke(job, new ProgressChangedEventArgs() { Progress = job.Progress });
        };
      }

      var result = await job.GetResultAsync();
      if(result.HasErrors)
      {
        errors.AddRange(result.LayerResults
          .Select(l => new SyncReplicaErrorResult() { Name = l.Key.Name, Error = l.Value.Error }));
        errors.AddRange(result.TableResults
          .Select(t => new SyncReplicaErrorResult() { Name = t.Key.TableName, Error = t.Value.Error }));

        return new SynchronizeReplicaResult { Synchronized = false, ResultErrors = errors };
      }
      else
      {
        return new SynchronizeReplicaResult { Synchronized = true };
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    [Obsolete]
    public async Task<string> DeleteReplica(Map map)
    {
      return await DeleteReplicaAsync(map);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public async Task<string> DeleteReplicaAsync(Map map)
    {
      var sb = new StringBuilder();
      var geodatabases = GetGeodatabases(map);
      if(geodatabases.Any())
      {
        foreach(var gdb in geodatabases)
        {
          var syncId = Guid.Empty;
          try
          {
            syncId = gdb.SyncId;

            var task = await GeodatabaseSyncTask.CreateAsync(gdb.Source);
            await task.UnregisterGeodatabaseAsync(gdb);

            gdb.Close();
            sb.Append($"{AppResources.DeleteReplicaMessageDeleted} {syncId}.");
          }
          catch(ArcGISWebException ex)
          {
            Debug.WriteLine(ex.Message);
            sb.Append($"{AppResources.DeleteReplicaMessageCantDelete} {syncId}.");
          }
          catch
          {
            throw;
          }
          finally
          {
            gdb.Close();
            sb.Append($"{AppResources.DeleteReplicaMessageDone}.");
          }
        }
      }

      await DeleteReplicaFolderAsync();
      return sb.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public async Task DeleteReplica()
    {
      await DeleteReplicaAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task DeleteReplicaAsync() => await DeleteReplicaFolderAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public async Task DeleteReplicaFolder()
    {
      await DeleteReplicaFolderAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task DeleteReplicaFolderAsync()
    {
      var folderPath = GetReplicaFullPath();
      try
      {
        var folders = Directory.GetDirectories(folderPath);
        foreach(var f in folders)
        {
          if(f.Contains("p13"))
          {
            var gdbs = Directory.GetFiles(f);
            foreach(var file in gdbs)
            {
              try
              {
                if(file.Contains(".geodatabase"))
                {
                  var gdb = await Geodatabase.OpenAsync(file);
                  gdb.Close();
                }
              }
              catch(Exception ex)
              {
                Debug.WriteLine(ex.Message);
              }
            }
          }
        }
        if(MobileMapPackage != null)
        {
          MobileMapPackage.Close();
        }
        Directory.Delete(folderPath, true);
      }
      catch(Exception ex)
      {
        Debug.WriteLine(ex.Message);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool ReplicaExist()
    {
      if(!string.IsNullOrEmpty(ReplicaFolderName))
      {
        var filePath = GetReplicaFullPath();
        return !string.IsNullOrEmpty(ReplicaFolderName) && Directory.Exists(filePath);
      }
      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public async Task<Map> GetReplicaMap()
    {
      return await GetReplicaMapAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Map> GetReplicaMapAsync()
    {
      var pathToOutputPackage = GetReplicaFullPath();
      MobileMapPackage = await MobileMapPackage.OpenAsync(pathToOutputPackage);

      return MobileMapPackage.Maps.FirstOrDefault();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private string GetReplicaFullPath() => Path.Combine(FileSystem.AppDataDirectory, ReplicaFolderName);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private void ValidateReplicaFolderPath()
    {
      var blockedFolders = new List<string>();
      var basePath = FileSystem.AppDataDirectory;
      var folders = Directory.GetDirectories(basePath);
      foreach(var f in folders)
      {
        if(f.Contains(AppFolderName))
        {
          try
          {
            Directory.Delete(f, true);
          }
          catch(Exception ex)
          {
            Debug.WriteLine(ex.Message);
            blockedFolders.Add(f);
          }
        }
      }
      if(blockedFolders.Count > 0)
      {
        var max = blockedFolders.Select(bf =>
        {
          var sufix = bf.Replace(AppFolderName, string.Empty);
          if(int.TryParse(sufix, out int index))
          {
            return index;
          }
          else
          {
            return -1;
          }
        }).Max();
        ReplicaFolderName = $"{AppFolderName}{max + 1}";
      }
      else
      {
        ReplicaFolderName = AppFolderName;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    private IEnumerable<Geodatabase> GetGeodatabases(Map map)
    {
      var query = map.AllLayers
        .Where(l => (l is FeatureLayer) && (l as FeatureLayer).FeatureTable is GeodatabaseFeatureTable)
        .Select(l => ((l as FeatureLayer).FeatureTable as GeodatabaseFeatureTable).Geodatabase)
        .Distinct();
      return query;
    }
  }


}
