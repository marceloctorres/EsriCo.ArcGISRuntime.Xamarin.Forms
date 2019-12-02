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
  public partial class LegendView : ContentView
  {

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsTitleVisibleProperty = BindableProperty.Create(
      nameof(IsTitleVisible),
      typeof(bool),
      typeof(LegendView),
      defaultValue: false);

    public new static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
      nameof(IsVisible),
      typeof(bool),
      typeof(LegendView), 
      defaultBindingMode:BindingMode.TwoWay,
      propertyChanged: OnIsVisibleChanged,
      defaultValue: false);


    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapProperty = BindableProperty.Create(
      nameof(Map),
      typeof(Map),
      typeof(LegendView),
      propertyChanged: OnMapChanged,
      defaultValue: null);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
      nameof(Title),
      typeof(string),
      typeof(LegendView),
      defaultValue: "Table of Contents");

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBorderColorProperty = BindableProperty.Create(
      nameof(TitleBorderColor),
      typeof(Color),
      typeof(LegendView),
      defaultValue: null);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBackgroundColorProperty = BindableProperty.Create(
      nameof(TitleBackgroundColor),
      typeof(Color),
      typeof(LegendView),
      defaultValue: Color.White);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(
      nameof(TitleTextColor),
      typeof(Color),
      typeof(LegendView),
      defaultValue: Color.Black);

    /// <summary>
    /// 
    /// </summary>
    public string Title
    {
      get => (string)GetValue(TitleProperty);
      set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool IsTitleVisible
    {
      get => (bool)GetValue(IsTitleVisibleProperty);
      set => SetValue(IsTitleVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public new bool IsVisible
    {
      get => (bool)GetValue(IsVisibleProperty);
      set => SetValue(IsVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public Map Map
    {
      get => (Map)GetValue(MapProperty);
      set => SetValue(MapProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBorderColor
    {
      get => (Color)GetValue(TitleBorderColorProperty);
      set => SetValue(TitleBorderColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleBackgroundColor
    {
      get => (Color)GetValue(TitleBackgroundColorProperty);
      set => SetValue(TitleBackgroundColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public Color TitleTextColor
    {
      get => (Color)GetValue(TitleTextColorProperty);
      set => SetValue(TitleTextColorProperty, value);
    }

    private ObservableCollection<LayerInfos> _layerInfosList;

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<LayerInfos> LayerInfosList
    {
      get => _layerInfosList;
      set
      {
        _layerInfosList = value;
        OnPropertyChanged(nameof(LayerInfosList));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var legendView = bindable as LegendView;
      var newVisible = (bool)newValue;

      legendView.SetIsVisible(newVisible);
    }

    private void SetIsVisible(bool visible)
    {
      base.IsVisible = visible;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnMapChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var legendView = bindable as LegendView;
      var map = newValue as Map;
      GetLayers(legendView, map);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    private async Task<List<LayerInfos>> GetLayerInfos()
    {
      if (Map == null)
      {
        return null;
      }
      else
      {
        var listLayerInfos = new List<LayerInfos>();
        Map.Loaded += (o, e) =>
        {
          Map.OperationalLayers
            .ToList()
            .ForEach(ol =>
            {
              ol.LoadAsync();
              var layerInfos = new LayerInfos()
              {
                GroupLayerInfo = new LayerInfo { Layer = ol }
              };
              ol.SublayerContents
              .ToList()
              .ForEach(sl =>
              {
                layerInfos.SubLayerInfos.Add(new LayerInfo()
                {
                  ParentInfo = layerInfos.GroupLayerInfo,
                  Layer = sl as Layer
                });
              });
              listLayerInfos.Add(layerInfos);
            });
        };
        await Map.LoadAsync();
        listLayerInfos
          .ForEach(async li =>
          {
            var legendInfos = await li.GroupLayerInfo.Layer.GetLegendInfosAsync();
            li.GroupLayerInfo.SetLegendInfos(legendInfos);
            li.SubLayerInfos
              .ForEach(async sli =>
              {
                var subLegendInfos = await sli.Layer.GetLegendInfosAsync();
                sli.SetLegendInfos(subLegendInfos);
              });
          });
        return listLayerInfos;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="legendView"></param>
    /// <param name="map"></param>
    private async static void GetLayers(LegendView legendView, Map map)
    {
      // var layerInfos = await legendView.GetLayerInfos();
      var layerInfos = await Device.InvokeOnMainThreadAsync(() => legendView.GetLayerInfos());
      if (layerInfos != null)
      {
        legendView.LayerInfosList = new ObservableCollection<LayerInfos>(layerInfos);
      }
      else
      {
        legendView.LayerInfosList = null;
      }
    }

    public LegendView()
    {
      InitializeComponent();
      base.IsVisible = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HideViewClicked(object sender, EventArgs e)
    {
      IsVisible = false;
    }

    private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
    {
      if (sender is ListView listView)
      {
        var list = (List<LegendImageInfo>)listView.ItemsSource;
        var height = list.Count * 32;
        listView.HeightRequest = height;
      }
    }
  }
}