using System;
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
  public partial class LayerListView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsTitleVisibleProperty = BindableProperty.Create(
      nameof(IsTitleVisible),
      typeof(bool),
      typeof(LayerListView),
      defaultValue: false);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapProperty = BindableProperty.Create(
      nameof(Map),
      typeof(Map),
      typeof(LayerListView),
      propertyChanged: OnMapChanged,
      defaultValue: null);

    /// <summary>
    /// 
    /// </summary>
    protected static readonly BindableProperty TitleProperty = BindableProperty.Create(
      nameof(Title),
      typeof(string),
      typeof(LayerListView),
      defaultValue: "Layer List View");

    /// <summary>
    /// 
    /// </summary>
    protected static readonly BindableProperty TitleBorderColorProperty = BindableProperty.Create(
      nameof(TitleBorderColor),
      typeof(Color),
      typeof(LayerListView),
      defaultValue: null);

    /// <summary>
    /// 
    /// </summary>
    protected static readonly BindableProperty TitleBackgroundColorProperty = BindableProperty.Create(
      nameof(TitleBackgroundColor),
      typeof(Color),
      typeof(LayerListView),
      defaultValue: Color.White);

    /// <summary>
    /// 
    /// </summary>
    protected static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(
      nameof(TitleTextColor),
      typeof(Color),
      typeof(LayerListView),
      defaultValue: Color.Black);

    /// <summary>
    /// 
    /// </summary>
    public static BindableProperty LayerItemTemplateProperty = BindableProperty.Create(
      nameof(LayerItemTemplate), 
      typeof(View), 
      typeof(LayerListView), 
      propertyChanged: null);

    /// <summary>
    /// 
    /// </summary>
    public DataTemplate LayerItemTemplate
    {
      get { return (DataTemplate)GetValue(LayerItemTemplateProperty); }
      set { SetValue(LayerItemTemplateProperty, value); }
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual string Title
    {
      get => (string)GetValue(TitleProperty);
      set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual bool IsTitleVisible
    {
      get => (bool)GetValue(IsTitleVisibleProperty);
      set => SetValue(IsTitleVisibleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual Map Map
    {
      get => (Map)GetValue(MapProperty);
      set => SetValue(MapProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual Color TitleBorderColor
    {
      get => (Color)GetValue(TitleBorderColorProperty);
      set => SetValue(TitleBorderColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual Color TitleBackgroundColor
    {
      get => (Color)GetValue(TitleBackgroundColorProperty);
      set => SetValue(TitleBackgroundColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual Color TitleTextColor
    {
      get => (Color)GetValue(TitleTextColorProperty);
      set => SetValue(TitleTextColorProperty, value);
    }

    private ObservableCollection<LayerInfos> _layerInfosList;

    /// <summary>
    /// 
    /// </summary>
    public virtual ObservableCollection<LayerInfos> LayerInfosList
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
      var view = bindable as LayerListView;
      GetLayers(view);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    protected virtual async Task<List<LayerInfos>> GetLayerInfos()
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
    protected async static void GetLayers(LayerListView view)
    {
      try
      {
        var layerInfos = await Device.InvokeOnMainThreadAsync(() => view.GetLayerInfos());
        if (layerInfos != null)
        {
          view.LayerInfosList = new ObservableCollection<LayerInfos>(layerInfos);
        }
        else
        {
          view.LayerInfosList = null;
        }
      }
      catch
      {
        view.LayerInfosList = null;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public LayerListView()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void HideViewClicked(object sender, EventArgs e)
    {
      IsVisible = false;
    }
  }
}
