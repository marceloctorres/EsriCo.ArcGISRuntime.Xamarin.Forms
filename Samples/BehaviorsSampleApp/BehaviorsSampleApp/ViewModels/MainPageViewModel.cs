using Esri.ArcGISRuntime.Mapping;
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

    public bool IsLegendVisible
    {
      get => _visible;
      set => SetProperty(ref _visible, value);
    }

    private List<Layer> _layers;
    public List<Layer> Layers { 
      get => _layers; 
      set => SetProperty(ref _layers, value); 
    }

    public ICommand LegendCommand { get; private set; }

    public ICommand TOCCommand { get; private set; }

    public MainPageViewModel(INavigationService navigationService)
        : base(navigationService)
    {
      Title = "Main Page";
      IsLegendVisible = false;
      Map = new Map(Basemap.CreateTopographicVector());
      Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/3")));
      Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/2")));
      Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/1")));
      Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/0")));

      Layers = Map.OperationalLayers.ToList();

      TOCCommand = new DelegateCommand(() =>
      {
        IsLegendVisible = !IsLegendVisible;
      });

    }

  }
}
