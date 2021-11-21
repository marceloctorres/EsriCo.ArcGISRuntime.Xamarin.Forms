using System.IO;
using System.Linq;
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
    public bool IsLoaded { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task SaveAsync()
    {
      string text = JsonConvert.SerializeObject(this);
      await WriteTextFileAsync(text);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual async Task LoadAsync()
    {
      await Task.Delay(0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual bool FileExist()
    {
      string filePath = Path.Combine(Folder, FileName);
      return File.Exists(filePath);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected virtual async Task LoadAsync<T>() where T : ConfigurationInfo
    {
      string text = await ReadTextFileAsync();
      if (!string.IsNullOrEmpty(text))
      {
        T configuration = JsonConvert.DeserializeObject<T>(text);

        System.Type t = configuration.GetType();
        System.Reflection.PropertyInfo[] propertyInfos = t.GetProperties().Where(pi => pi.CanWrite).ToArray();

        foreach (System.Reflection.PropertyInfo pi in propertyInfos)
        {
          object value = pi.GetValue(configuration);
          pi.SetValue(this, value);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    protected async Task WriteTextFileAsync(string text)
    {
      await WriteTextFileAsync(FileName, text);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    protected async Task WriteTextFileAsync(string filename, string text)
    {
      string filePath = Path.Combine(Folder, filename);
      using (StreamWriter writer = File.CreateText(filePath))
      {
        await writer.WriteAsync(text);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected async Task<string> ReadTextFileAsync()
    {
      return await ReadTextFileAsync(FileName);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    protected async Task<string> ReadTextFileAsync(string filename)
    {
      string filePath = Path.Combine(Folder, filename);
      if (File.Exists(filePath))
      {
        using (StreamReader reader = File.OpenText(filePath))
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
 