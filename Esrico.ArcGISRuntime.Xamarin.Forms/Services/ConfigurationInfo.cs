using System.IO;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Xamarin.Essentials;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Services
{
  /// <summary>
  /// 
  /// </summary>
  public class ConfigurationInfo
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
    public async Task Save()
    {
      string json = JsonConvert.SerializeObject(this);
      await WriteTextFile(FileName, json);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual async Task Load() => await Task.Delay(0);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected virtual async Task Load<T>() where T : ConfigurationInfo
    {
      var json = await ReadTextFile(FileName);
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
    protected async Task WriteTextFile(string filename, string text)
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
    protected async Task<string> ReadTextFile(string filename)
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
