using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Prism.Mvvm;

using Xamarin.Essentials;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Services {
  /// <summary>
  /// 
  /// </summary>
  public class ConfigurationInfo : BindableBase, IConfigurationInfo {
    /// <summary>
    /// 
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Folder { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public bool IsLoaded { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task SaveAsync() {
      var text = JsonConvert.SerializeObject(this);
      await WriteTextFileAsync(text);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual async Task LoadAsync() => await Task.Delay(0);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual bool FileExist() {
      var filePath = Path.Combine(Folder, FileName);
      return File.Exists(filePath);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected virtual async Task LoadAsync<T>() where T : ConfigurationInfo {
      var text = await ReadTextFileAsync();
      if(!string.IsNullOrEmpty(text)) {
        var configuration = JsonConvert.DeserializeObject<T>(text);

        var t = configuration.GetType();
        var propertyInfos = t.GetProperties().Where(pi => pi.CanWrite).ToArray();

        foreach(var pi in propertyInfos) {
          var value = pi.GetValue(configuration);
          pi.SetValue(this, value);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    protected async Task WriteTextFileAsync(string text) => await WriteTextFileAsync(FileName, text);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    protected async Task WriteTextFileAsync(string filename, string text) {
      var filePath = Path.Combine(Folder, filename);
      using(var writer = File.CreateText(filePath)) {
        await writer.WriteAsync(text);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected async Task<string> ReadTextFileAsync() => await ReadTextFileAsync(FileName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    protected async Task<string> ReadTextFileAsync(string filename) {
      var filePath = Path.Combine(Folder, filename);
      if(File.Exists(filePath)) {
        using(var reader = File.OpenText(filePath)) {
          return await reader.ReadToEndAsync();
        }
      }
      return string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    public ConfigurationInfo() => Folder = FileSystem.AppDataDirectory;
  }
}
