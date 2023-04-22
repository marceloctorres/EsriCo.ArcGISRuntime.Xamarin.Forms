using System;
using System.Globalization;
using System.Threading.Tasks;

using Esri.ArcGISRuntime.Symbology;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Converters {
  /// <summary>
  /// 
  /// </summary>
  public class SymbolToImageConverter : IValueConverter {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if(value is Symbol symbol1 && targetType == typeof(ImageSource)) {
        var symbol = symbol1;
        var task = GetImageAsync(symbol);
        var awaiter = task.GetAwaiter();
        return awaiter.GetResult();
      }
      return value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="symbol"></param>
    /// <returns></returns>
    private static async Task<ImageSource> GetImageAsync(Symbol symbol) {
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
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      throw new NotImplementedException();
    }
  }
}
