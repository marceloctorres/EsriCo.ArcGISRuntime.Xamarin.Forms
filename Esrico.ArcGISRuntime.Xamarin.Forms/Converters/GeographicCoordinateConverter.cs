using System;
using System.Globalization;

using Esri.ArcGISRuntime.Geometry;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Converters {
  /// <summary>
  /// 
  /// </summary>
  public class GeographicCoordinateConverter : IValueConverter {
    /// <summary>
    /// Convert a double value that is a decimal degree angle to its degree, minutes and 
    /// seconds display
    /// </summary>
    /// <param name="value">Input value</param>
    /// <param name="targetType">Target Type</param>
    /// <param name="parameter">Parameter</param>
    /// <param name="culture">Culture Info</param>
    /// <returns>Converted value</returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      if((value is double @double) && targetType == typeof(string)) {
        return @double.ToDms((string)parameter);
      }
      else if(value.GetType() == typeof(MapPoint) && targetType == typeof(string)) {
        return ((MapPoint)value).ToDms();
      }
      return value;
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
