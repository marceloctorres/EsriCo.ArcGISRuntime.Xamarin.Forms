using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace BehaviorsSampleApp.ViewModels
{
  public class MainPageViewModel : ViewModelBase
  {
    private Map _map;
    public Map Map
    {
      get { return _map; }
      set { SetProperty(ref _map, value); }
    }

    private bool _visible;

    public bool IsIdentifyVisible
    {
      get => _visible;
      set => SetProperty(ref _visible, value);
    }

    private bool _tocVisible;

    public bool IsTOCVisible
    {
      get => _tocVisible;
      set => SetProperty(ref _tocVisible, value);
    }

    private bool _legendVisible;

    public bool IsLegendVisible
    {
      get => _legendVisible;
      set => SetProperty(ref _legendVisible, value);
    }


    private List<Layer> _layers;
    public List<Layer> Layers
    {
      get => _layers;
      set => SetProperty(ref _layers, value);
    }

    private IdentifyResults _identifyResults;

    /// <summary>
    /// 
    /// </summary>
    public IdentifyResults IdentifyResults
    {
      get { return _identifyResults; }
      set { SetProperty(ref _identifyResults, value); }
    }

    public ICommand LegendCommand { get; private set; }

    public ICommand TOCCommand { get; private set; }

    public ICommand IdentificarCommand { get; private set; }

    public MainPageViewModel(INavigationService navigationService)
        : base(navigationService)
    {
      Title = "Main Page";
      IsLegendVisible = false;
      Map = new Map(Basemap.CreateTopographicVector())
      {
        InitialViewpoint = new Viewpoint(new MapPoint(-74.042492, 4.660555, SpatialReferences.Wgs84), 5000)
      };
      Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/3")));
      Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/2")));
      Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/1")));
      Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/0")));

      Layers = Map.OperationalLayers.ToList();

      LegendCommand = new DelegateCommand(() =>
        {
          IsLegendVisible = !IsLegendVisible;
        });
      TOCCommand = new DelegateCommand(() =>
        {
          IsTOCVisible = !IsTOCVisible;
        });
      IdentificarCommand = new DelegateCommand<IdentifyResults>((o) =>
        {
          if (o.GeoElementResults.Count > 0)
          {
            IdentifyResults = o;
            IsIdentifyVisible = true;
          }
        });
    }

  }
}
