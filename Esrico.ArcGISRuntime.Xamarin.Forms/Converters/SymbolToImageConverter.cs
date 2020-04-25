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
      if(value is Symbol && targetType == typeof(ImageSource))
      {
        var symbol = (Symbol)value;
        var task = GetImageAsync(symbol);
        var awaiter = task.GetAwaiter();
        awaiter.OnCompleted(() =>
        {
          var image = task.Result;
        });
        return awaiter.GetResult();
      }
      return value;
    }

    private async Task<ImageSource> GetImageAsync(Symbol symbol)
    {
      var imageData = await symbol.CreateSwatchAsync();
      var stream = await imageData.GetEncodedBufferAsync();
      var imageSource = ImageSource.FromStream(() => stream);
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
