using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Xamarin.Forms;
using EsriCo.ArcGISRuntime.Xamarin.Forms.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class LayerListPanelView : ListPanelView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(LayerListPanelView),
      propertyChanged: OnMapViewPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    public MapView MapView
    {
      get => (MapView)GetValue(MapViewProperty);
      set => SetValue(MapViewProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnMapViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var panelView = bindable as LayerListPanelView;
      if (newValue is MapView newMapView)
      {
        if (newMapView.Map != null)
        {
          panelView.SetMap(newMapView.Map);
        }
        else
        {
          newMapView.PropertyChanged += (s, e) =>
          {
            if (e.PropertyName == nameof(newMapView.Map) && newMapView.Map != null)
            {
              panelView.SetMap(newMapView.Map);
            }
          };
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private Map Map { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
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
    /// <param name="map"></param>
    public async void SetMap(Map map)
    {
      Map = map;
      var layerInfos = await Device.InvokeOnMainThreadAsync(() => GetLayerInfos());
      LayerInfosList = layerInfos != null ?
        new ObservableCollection<LayerInfos>(layerInfos) :
        null;
    }

    /// <summary>
    /// 
    /// </summary>
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

    public LayerListPanelView()
    {
      InitializeComponent();
    }
  }
}