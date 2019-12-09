using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BehaviorsSampleApp.Resx;
using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors;

using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

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

    private bool _isLoginVisible;

    public bool IsLoginVisible
    {
      get => _isLoginVisible;
      set => SetProperty(ref _isLoginVisible, value);
    }

    private bool _processing; 
    
    public bool IsProcessing
    {
      get => _processing;
      set => SetProperty(ref _processing, value);
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

    public IdentifyResults IdentifyResults
    {
      get { return _identifyResults; }
      set { SetProperty(ref _identifyResults, value); }
    }

    private string _user;

    public string User
    {
      get => _user;
      set => SetProperty(ref _user, value);
    }

    private string _password;

    public string Password
    {
      get => _password;
      set => SetProperty(ref _password, value);
    }

    public ICommand LogInCommand { get; private set; }

    public ICommand CancelCommand { get; private set; }

    public ICommand LegendCommand { get; private set; }

    public ICommand TOCCommand { get; private set; }

    public ICommand IdentificarCommand { get; private set; }

    public ICommand LoadMapCommand { get; private set; }

    public ICommand GeoViewTappedCommand { get; private set; }

    public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        : base(navigationService, pageDialogService)
    {
      Title = "Map Page";
      IsLegendVisible = false;
      IsProcessing = false;
      IsTOCVisible = false;
      IsIdentifyVisible = false;
      IsLoginVisible = false;
      User = "mtorres";
      Password = "qwertyuiop54321$%&";

      GeoViewTappedCommand = new DelegateCommand(() =>
      {
        IsProcessing = true;
      });

      LegendCommand = new DelegateCommand(() =>
        {
          IsLegendVisible = !IsLegendVisible;
        }, () => { 
          return Map != null;
        }).ObservesProperty(() => Map);
      TOCCommand = new DelegateCommand(() =>
        {
          IsTOCVisible = !IsTOCVisible;
        }, () => {
          return Map != null;
        }).ObservesProperty(() => Map);
      IdentificarCommand = new DelegateCommand<IdentifyResults>((o) =>
        {
          if (o.GeoElementResults.Count > 0)
          {
            IdentifyResults = o;
            IsIdentifyVisible = true;
          }
          IsProcessing = false;
        });
      LoadMapCommand = new DelegateCommand(() =>
      {
        if(Map!= null)
        {
          Map = null;
          IsLoginVisible = false;
        }
        else
        {
          IsLoginVisible = true;
        }
      });
      LogInCommand = new DelegateCommand(() =>
      {
        IsLoginVisible = false;
        Map = new Map(Basemap.CreateTopographicVector())
        {
          InitialViewpoint = new Viewpoint(new MapPoint(-74.042492, 4.660555, SpatialReferences.Wgs84), 5000)
        };
        Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/3")));
        Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/2")));
        Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/1")));
        Map.OperationalLayers.Add(new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/0")));

        Layers = Map.OperationalLayers.ToList();
      });
      CancelCommand = new DelegateCommand(async () =>
       {
         IsLoginVisible = false;
         await PageDialogService.DisplayAlertAsync(AppResources.DialogTitle, 
           AppResources.CancelLoginText, 
           AppResources.CloseButtonText);
       });
    }

  }
}
