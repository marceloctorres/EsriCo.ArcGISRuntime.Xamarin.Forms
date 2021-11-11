using System;
using System.Globalization;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Symbology;

using Xamarin.Forms;

/// <summary>
/// 
/// </summary>
namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Converters
{
  /// <summary>
  /// 
  /// </summary>
  public class SymbolToImageConverter : IValueConverter
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (value is Symbol && targetType == typeof(ImageSource))
      {
        Symbol symbol = (Symbol)value;
        Task<ImageSource> task = GetImageAsync(symbol);
        System.Runtime.CompilerServices.TaskAwaiter<ImageSource> awaiter = task.GetAwaiter();
        awaiter.OnCompleted(() =>
        {
          ImageSource image = task.Result;
        });
        return awaiter.GetResult();
      }
      return value;
    }

    private async Task<ImageSource> GetImageAsync(Symbol symbol)
    {
      Esri.ArcGISRuntime.UI.RuntimeImage imageData = await symbol.CreateSwatchAsync();
      System.IO.Stream stream = await imageData.GetEncodedBufferAsync();
      ImageSource imageSource = ImageSource.FromStream(() => stream);
      return imageSource;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
  }
}
