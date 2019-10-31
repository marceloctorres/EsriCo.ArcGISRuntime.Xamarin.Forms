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
  public partial class TableOfContentsView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsTitleVisibleProperty = BindableProperty.Create(
      nameof(IsTitleVisible),
      typeof(bool),
      typeof(TableOfContentsView),
      defaultValue: false);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapProperty = BindableProperty.Create(
      nameof(Map),
      typeof(Map),
      typeof(TableOfContentsView),
      propertyChanged: OnMapChanged,
      defaultValue: null);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
      nameof(Title),
      typeof(string),
      typeof(TableOfContentsView),
      defaultValue: "Table of Contents");

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBorderColorProperty = BindableProperty.Create(
      nameof(TitleBorderColor),
      typeof(Color),
      typeof(TableOfContentsView),
      defaultValue: null);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBackgroundColorProperty = BindableProperty.Create(
      nameof(TitleBackgroundColor),
      typeof(Color),
      typeof(TableOfContentsView),
      defaultValue: Color.White);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(
      nameof(TitleTextColor),
      typeof(Color),
      typeof(TableOfContentsView),
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

    private List<LayerInfos> _layerInfosList;

    /// <summary>
    /// 
    /// </summary>
    public List<LayerInfos> LayerInfosList
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
    private static void OnMapChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var tocView = bindable as TableOfContentsView;
      var map = newValue as Map;
      GetLayers(tocView, map);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tocView"></param>
    /// <param name="map"></param>
    private static void GetLayers(TableOfContentsView tocView, Map map)
    {
      var listLayerInfos = new List<LayerInfos>();
      if (map == null)
      {
        tocView.LayerInfosList = null;
      }
      else
      {
        map.Loaded += (o, e) =>
        {
          map.OperationalLayers
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
          tocView.LayerInfosList = listLayerInfos;
        };
      }
    }

    public TableOfContentsView()
    {
      InitializeComponent();
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

  }
}