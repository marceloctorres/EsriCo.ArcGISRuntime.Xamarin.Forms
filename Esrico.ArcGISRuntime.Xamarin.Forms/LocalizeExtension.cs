using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms {
  /// <summary>
  /// 
  /// </summary>
  [ContentProperty("Text")]
  public class LocalizeExtension : IMarkupExtension {
    /// <summary>
    /// 
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns></returns>
    public object ProvideValue(IServiceProvider serviceProvider) {
      if(Text != null) {
        var resourceManager = new ResourceManager(
          typeof(AppResources).FullName,
          typeof(LocalizeExtension).GetTypeInfo().Assembly);
        return resourceManager.GetString(Text, CultureInfo.CurrentCulture);
      }
      return null;
    }
  }
}
