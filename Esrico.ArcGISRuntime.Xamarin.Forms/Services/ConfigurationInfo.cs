using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Prism.Mvvm;

using Xamarin.Essentials;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Services
{
  /// <summary>
  /// 
  /// </summary>
  public class ConfigurationInfo : BindableBase, IConfigurationInfo
  {
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
    /// <returns></returns>
    public async Task SaveAsync()
    {
      string json = JsonConvert.SerializeObject(this);
      await WriteTextFileAsync(FileName, json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual async Task LoadAsync() => await Task.Delay(0);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected virtual async Task LoadAsync<T>() where T : ConfigurationInfo
    {
      var json = await ReadTextFileAsync(FileName);
      if(!string.IsNullOrEmpty(json))
      {
        T configuration = JsonConvert.DeserializeObject<T>(json);

        var t = configuration.GetType();
        var propertyInfos = t.GetProperties();

        foreach(var pi in propertyInfos)
        {
          object value = pi.GetValue(configuration);
          pi.SetValue(this, value);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    protected async Task WriteTextFileAsync(string filename, string text)
    {
      var filePath = Path.Combine(Folder, filename);
      using(StreamWriter writer = File.CreateText(filePath))
      {
        await writer.WriteAsync(text);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    protected async Task<string> ReadTextFileAsync(string filename)
    {
      var filePath = Path.Combine(Folder, filename);
      if(File.Exists(filePath))
      {
        using(StreamReader reader = File.OpenText(filePath))
        {
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
