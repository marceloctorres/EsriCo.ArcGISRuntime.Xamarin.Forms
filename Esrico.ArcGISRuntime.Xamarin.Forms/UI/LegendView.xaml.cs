using System.Collections.Generic;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI {
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LegendView : LayerListPanelView {
    /// <summary>
    /// 
    /// </summary>
    public double ItemRenderHeight { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public LegendView() {
      ItemRenderHeight = 25;
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnInnerListViewItemAppearing(object sender, ItemVisibilityEventArgs e) {
      if(sender is ListView listView) {
        var list = (List<LegendImageInfo>)listView.ItemsSource;
        var height = list.Count * ItemRenderHeight;
        listView.HeightRequest = height;
      }
    }
  }
}