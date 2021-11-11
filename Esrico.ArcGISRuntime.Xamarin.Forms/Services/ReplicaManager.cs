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
  public class ReplicaManager : IReplicaManager
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
    public async Task<ValidateReplicaResult> ValidateReplicaAsync(Map map, Viewpoint viewpoint)
    {
      Layer firstLayer = map.Basemap.BaseLayers.FirstOrDefault() as Layer;
      string basemapUrl = string.Empty;

      if (firstLayer is ArcGISTiledLayer)
      {
        ArcGISTiledLayer baseLayer = firstLayer as ArcGISTiledLayer;
        basemapUrl = baseLayer.Source.AbsoluteUri;
      }
      if (!string.IsNullOrEmpty(basemapUrl))
      {
        string newBasemapUrl = basemapUrl.Contains("services.arcgisonline") ?
          basemapUrl.Replace("services.arcgisonline", "tiledbasemaps.arcgis") :
          basemapUrl;
        ExportTileCacheTask task = await ExportTileCacheTask.CreateAsync(new Uri(newBasemapUrl));

        ExportTileCacheParameters param = await task.CreateDefaultExportTileCacheParametersAsync(viewpoint.TargetGeometry, MinScale, MaxScale);
        EstimateTileCacheSizeJob job = task.EstimateTileCacheSize(param);

        try
        {
          EstimateTileCacheSizeResult result = await job.GetResultAsync();
          return new ValidateReplicaResult
          {
            Valid = result.TileCount < task.ServiceInfo.MaxExportTilesCount,
            Tiles = (int)result.TileCount,
            Size = result.FileSize
          };
        }
        catch (ArcGISRuntimeException ex)
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
    public async Task<DownloadReplicaResult> DownloadReplicaAsync(Map map, Viewpoint viewpoint,
      EventHandler<JobChangedEventArgs> jobHandler, EventHandler<ProgressChangedEventArgs> progressHandler)
    {
      ValidateReplicaFolderPath();
      string pathToOutputPackage = GetReplicaFullPath();
      Esri.ArcGISRuntime.Geometry.Geometry areaOfInterest = viewpoint.TargetGeometry;

      OfflineMapTask task = await OfflineMapTask.CreateAsync(map);
      GenerateOfflineMapParameters parameters = await task.CreateDefaultGenerateOfflineMapParametersAsync(areaOfInterest);

      parameters.MinScale = MinScale;
      parameters.MaxScale = MaxScale;
      parameters.AttachmentSyncDirection = AttachmentSyncDirection.Bidirectional;
      parameters.ReturnLayerAttachmentOption = ReturnLayerAttachmentOption.AllLayers;
      parameters.ReturnSchemaOnlyForEditableLayers = false;
      parameters.ItemInfo.Title = $"{parameters.ItemInfo.Title} (Off-line)";

      List<DownloadReplicaErrorResult> errors = new List<DownloadReplicaErrorResult>();
      OfflineMapCapabilities capabilitiesResults = await task.GetOfflineMapCapabilitiesAsync(parameters);
      if (capabilitiesResults.HasErrors)
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
        GenerateOfflineMapJob job = task.GenerateOfflineMap(parameters, pathToOutputPackage);
        if (jobHandler != null)
        {
          JobChangedEventHandler += jobHandler;
          job.JobChanged += (o, e) =>
          {
            JobChangedEventHandler?.Invoke(job, new JobChangedEventArgs() { Messages = job.Messages, Status = job.Status });
          };
        }
        if (progressHandler != null)
        {
          ProgressChangedEventHandler += progressHandler;
          job.ProgressChanged += (o, e) =>
          {
            ProgressChangedEventHandler?.Invoke(job, new ProgressChangedEventArgs() { Progress = job.Progress });
          };
        }
        GenerateOfflineMapResult generateOfflineMapResults = await job.GetResultAsync();
        if (!generateOfflineMapResults.HasErrors)
        {
          string okMessage = $"{AppResources.DownloadReplicaOkMessageMap} " +
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
    /// 
    /// <returns></returns>
    public async Task<SynchronizeReplicaResult> SynchronizeReplicaAsync(Map map, EventHandler<JobChangedEventArgs> jobHandler,
      EventHandler<ProgressChangedEventArgs> progressHandler)
    {
      List<SyncReplicaErrorResult> errors = new List<SyncReplicaErrorResult>();
      OfflineMapSyncTask task = await OfflineMapSyncTask.CreateAsync(map);
      OfflineMapSyncParameters param = new OfflineMapSyncParameters()
      {
        RollbackOnFailure = true,
        SyncDirection = SyncDirection.Bidirectional
      };

      OfflineMapSyncJob job = task.SyncOfflineMap(param);
      if (jobHandler != null)
      {
        JobChangedEventHandler += jobHandler;
        job.JobChanged += (o, e) =>
        {
          if (JobChangedEventHandler != null)
          {
            JobChangedEventHandler.Invoke(job, new JobChangedEventArgs() { Messages = job.Messages, Status = job.Status });
          }
        };
      }
      if (progressHandler != null)
      {
        ProgressChangedEventHandler += progressHandler;
        job.ProgressChanged += (o, e) =>
        {
          ProgressChangedEventHandler?.Invoke(job, new ProgressChangedEventArgs() { Progress = job.Progress });
        };
      }

      OfflineMapSyncResult result = await job.GetResultAsync();
      if (result.HasErrors)
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
    public async Task<string> DeleteReplicaAsync(Map map)
    {
      StringBuilder sb = new StringBuilder();
      IEnumerable<Geodatabase> geodatabases = GetGeodatabases(map);
      if (geodatabases.Any())
      {
        foreach (Geodatabase gdb in geodatabases)
        {
          Guid syncId = Guid.Empty;
          try
          {
            syncId = gdb.SyncId;

            GeodatabaseSyncTask task = await GeodatabaseSyncTask.CreateAsync(gdb.Source);
            await task.UnregisterGeodatabaseAsync(gdb);

            gdb.Close();
            sb.Append($"{AppResources.DeleteReplicaMessageDeleted} {syncId}.");
          }
          catch (ArcGISWebException ex)
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
    public async Task DeleteReplicaAsync() => await DeleteReplicaFolderAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task DeleteReplicaFolderAsync()
    {
      string folderPath = GetReplicaFullPath();
      try
      {
        string[] folders = Directory.GetDirectories(folderPath);
        foreach (string f in folders)
        {
          if (f.Contains("p13"))
          {
            string[] gdbs = Directory.GetFiles(f);
            foreach (string file in gdbs)
            {
              try
              {
                if (file.Contains(".geodatabase"))
                {
                  Geodatabase gdb = await Geodatabase.OpenAsync(file);
                  gdb.Close();
                }
              }
              catch (Exception ex)
              {
                Debug.WriteLine(ex.Message);
              }
            }
          }
        }
        if (MobileMapPackage != null)
        {
          MobileMapPackage.Close();
        }
        Directory.Delete(folderPath, true);
      }
      catch (Exception ex)
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
      if (!string.IsNullOrEmpty(ReplicaFolderName))
      {
        string filePath = GetReplicaFullPath();
        return !string.IsNullOrEmpty(ReplicaFolderName) && Directory.Exists(filePath);
      }
      return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task<Map> GetReplicaMapAsync()
    {
      string pathToOutputPackage = GetReplicaFullPath();
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
      List<string> blockedFolders = new List<string>();
      string basePath = FileSystem.AppDataDirectory;
      string[] folders = Directory.GetDirectories(basePath);
      foreach (string f in folders)
      {
        if (f.Contains(AppFolderName))
        {
          try
          {
            Directory.Delete(f, true);
          }
          catch (Exception ex)
          {
            Debug.WriteLine(ex.Message);
            blockedFolders.Add(f);
          }
        }
      }
      if (blockedFolders.Count > 0)
      {
        int max = blockedFolders.Select(bf =>
        {
          string sufix = bf.Replace(AppFolderName, string.Empty);
          if (int.TryParse(sufix, out int index))
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
      IEnumerable<Geodatabase> query = map.AllLayers
        .Where(l => (l is FeatureLayer) && (l as FeatureLayer).FeatureTable is GeodatabaseFeatureTable)
        .Select(l => ((l as FeatureLayer).FeatureTable as GeodatabaseFeatureTable).Geodatabase)
        .Distinct();
      return query;
    }
  }


}
