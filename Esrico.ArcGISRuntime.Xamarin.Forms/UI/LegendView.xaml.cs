﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Mapping;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Model;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LegendView : LayerListPanelView
  {
    public double ItemRenderHeight { get; set; }

    public LegendView()
    {
      ItemRenderHeight = 25;
      InitializeComponent();
    }

    private void OnInnerListViewItemAppearing(object sender, ItemVisibilityEventArgs e)
    {
      if (sender is ListView listView)
      {
        var list = (List<LegendImageInfo>)listView.ItemsSource;
        var template = listView.ItemTemplate;
        var height = list.Count * ItemRenderHeight;
        listView.HeightRequest = height;
      }
    }
  }
}