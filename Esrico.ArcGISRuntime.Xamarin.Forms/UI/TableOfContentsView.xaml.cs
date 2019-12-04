using System;
using System.Collections.Generic;
using System.Linq;

using Esri.ArcGISRuntime.Mapping;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

/// <summary>
/// 
/// </summary>
namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class TableOfContentsView : LayerListPanelView
  {
    public TableOfContentsView()
    {
      InitializeComponent();
    }
  }
}