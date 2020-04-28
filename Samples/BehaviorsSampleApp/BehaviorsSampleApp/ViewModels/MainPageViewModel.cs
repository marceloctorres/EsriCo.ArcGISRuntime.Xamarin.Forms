using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

using BehaviorsSampleApp.Resx;

using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors;
using EsriCo.ArcGISRuntime.Xamarin.Forms.Services;

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
      get => _map;
      set => SetProperty(ref _map, value);
    }

    private bool _isDrawing;

    /// <summary>
    /// 
    /// </summary>
    public bool IsDrawing
    {
      get => _isDrawing;
      set => SetProperty(ref _isDrawing, value);
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
      get => _identifyResults;
      set => SetProperty(ref _identifyResults, value);
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

    private bool _isIdentifyMenuVisible;
    public bool IsIdentifyMenuVisible
    {
      get => _isIdentifyMenuVisible;
      set => SetProperty(ref _isIdentifyMenuVisible, value);
    }

    private bool _isApprovalPanelVisible;

    public bool IsApprovalPanelVisible
    {
      get => _isApprovalPanelVisible;
      set => SetProperty(ref _isApprovalPanelVisible, value);
    }

    private bool _isMeasurementViewVisible;

    public bool IsMeasurementViewVisible
    {
      get => _isMeasurementViewVisible;
      set => SetProperty(ref _isMeasurementViewVisible, value);
    }

    private int _currentElementIndex;
    public int CurrentElementIndex
    {
      get => _currentElementIndex;
      set => SetProperty(ref _currentElementIndex, value);
    }

    private bool _isApprovalActivityVisible;

    /// <summary>
    /// 
    /// </summary>
    public bool IsApprovalActivityVisible
    {
      get => _isApprovalActivityVisible;
      set => SetProperty(ref _isApprovalActivityVisible, value);
    }

    private bool _isSignatureVisible;

    /// <summary>
    /// 
    /// </summary>
    public bool IsSignatureVisible
    {
      get => _isSignatureVisible;
      set => SetProperty(ref _isSignatureVisible, value);
    }

    private bool _isPortalsPanelVisible;

    /// <summary>
    /// 
    /// </summary>
    public bool IsPortalPanelVisible
    {
      get => _isPortalsPanelVisible;
      set => SetProperty(ref _isPortalsPanelVisible, value);
    }

    private List<PortalConnection> _portals;

    /// <summary>
    /// 
    /// </summary>
    public List<PortalConnection> Portals
    {
      get => _portals;
      set => SetProperty(ref _portals, value);
    }

    public ICommand LogInCommand { get; private set; }

    public ICommand CancelCommand { get; private set; }

    public ICommand LegendCommand { get; private set; }

    public ICommand TOCCommand { get; private set; }

    public ICommand IdentificarCommand { get; private set; }

    public ICommand LoadMapCommand { get; private set; }

    public ICommand GeoViewTappedCommand { get; private set; }

    public ICommand ShowIdentifyMenuCommand { get; private set; }

    public ICommand ApprovalCommand { get; private set; }

    public ICommand CloseCommand { get; private set; }

    public ICommand SignatureCommand { get; private set; }

    public ICommand MeasurementCommand { get; private set; }

    public ICommand BusquedaCommand { get; private set; }

    public ICommand PortalsCommand { get; private set; }

    public PortalConnection Portal { get; private set; }

    public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        : base(navigationService, pageDialogService)
    {
      IsLegendVisible = false;
      IsProcessing = false;
      IsTOCVisible = false;
      IsIdentifyVisible = false;
      IsLoginVisible = false;
      IsSignatureVisible = false;
      IsPortalPanelVisible = false;

      User = "mctorres";
      Password = "m4rc3l025202$$";
      IsMeasurementViewVisible = false;

      PortalsCommand = new DelegateCommand(() =>
      {
        IsPortalPanelVisible = !IsPortalPanelVisible;
      });

      MeasurementCommand = new DelegateCommand(() =>
      {
        IsMeasurementViewVisible = !IsMeasurementViewVisible;
      }, () =>
      {
        return Map != null;
      }).ObservesProperty(() => Map);

      ApprovalCommand = new DelegateCommand(async () =>
      {
        IsIdentifyMenuVisible = false;
        IsApprovalPanelVisible = true;
        IsApprovalActivityVisible = true;

        await PageDialogService.DisplayAlertAsync(AppResources.DialogTitle,
          $"{AppResources.ApprovalMessage} {CurrentElementIndex}",
          AppResources.CloseButtonText);

        IsApprovalActivityVisible = false;

      });
      SignatureCommand = new DelegateCommand(() =>
      {
        IsSignatureVisible = true;
      });

      CloseCommand = new DelegateCommand(() =>
        {
          IsApprovalPanelVisible = false;
        });

      ShowIdentifyMenuCommand = new DelegateCommand(() =>
        {
          IsIdentifyMenuVisible = !IsIdentifyMenuVisible;
        });

      GeoViewTappedCommand = new DelegateCommand(() =>
      {
        if(!IsDrawing)
        {
          IsProcessing = true;
        }
      });

      LegendCommand = new DelegateCommand(() =>
        {
          IsLegendVisible = !IsLegendVisible;
        }, () =>
        {
          return Map != null;
        }).ObservesProperty(() => Map);

      TOCCommand = new DelegateCommand(() =>
        {
          IsTOCVisible = !IsTOCVisible;
        }, () =>
        {
          return Map != null;
        }).ObservesProperty(() => Map);

      IdentificarCommand = new DelegateCommand<IdentifyResults>((o) =>
        {
          if(IsDrawing)
            return;

          if(o.GeoElementResults.Count > 0)
          {
            IdentifyResults = o;
            IsIdentifyVisible = true;
          }
          IsProcessing = false;
        });

      LoadMapCommand = new DelegateCommand(() =>
      {
        if(Map != null)
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
        InitMap();
        //Map = new Map(Basemap.CreateTopographicVector())
        //{
        //  InitialViewpoint = new Viewpoint(new MapPoint(-74.042492, 4.660555, SpatialReferences.Wgs84), 5000)
        //};
        //FeatureLayer[] layers =
        //{
        //  new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/3")),
        //  new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/2")),
        //  new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/1")),
        //  new FeatureLayer(new Uri("https://services1.arcgis.com/7S16A7PAFcmSmqJA/ArcGIS/rest/services/InspeccionPublica/FeatureServer/0"))
        //};
        //Map.OperationalLayers.AddRange(layers);
      });
      CancelCommand = new DelegateCommand(() =>
       {
         IsLoginVisible = false;
         //await PageDialogService.DisplayAlertAsync(AppResources.DialogTitle, 
         //  AppResources.CancelLoginText, 
         //  AppResources.CloseButtonText);
       });
      BusquedaCommand = new DelegateCommand(async () =>
      {
        var featureLayer = Map.OperationalLayers
          .Where(l =>
          {
            if(l is FeatureLayer)
            {
              var lyr = l as FeatureLayer;
              return lyr.FeatureTable.TableName == "Predio";
            }
            return false;
          })
          .Select(l => l as FeatureLayer)
          .FirstOrDefault();
        var resultados = new List<Feature>();
        if(featureLayer != null)
        {
          var queryParameters = new QueryParameters
          {
            WhereClause = $"nombe_unidad_espacial LIKE '%Argentina%'"
          };

          var results = await featureLayer.FeatureTable.QueryFeaturesAsync(queryParameters);
          foreach(var r in results)
          {
            var agsFeature = (ArcGISFeature)r;
            if(agsFeature.LoadStatus != Esri.ArcGISRuntime.LoadStatus.Loaded)
            {
              await agsFeature.RetryLoadAsync();
            }
            resultados.Add(r);
          }
        }

        var identifyResults = new IdentifyResults
        {
          GeoElementResults = resultados
          .Select(r => new IdentifyGeoElementResult
          {
            GeoElement = r,
            Layer = featureLayer
          })
          .ToList()
        };
        IdentifyResults = identifyResults;
        IsIdentifyVisible = true;
      });

      Portals = new List<PortalConnection>()
      {
        new PortalConnection
        {
          User = "mctorres",
          Password = "m4rc3l025202$$",
          BaseUrl = "https://www.arcgis.com",
        },
        new PortalConnection
        {
          User = "mctorres",
          Password = "m4rc3l025202$$",
          BaseUrl = "https://project-esri-co.maps.arcgis.com",
        },
        new PortalConnection
        {
          User = "mtorres",
          Password = "m4rc3l025202$$",
          BaseUrl = "https://age1071.eastus.cloudapp.azure.com/portal",
        },
      };
      Portal = Portals[0];

      InitPortal();
    }

    private async void InitPortal() 
    {
      foreach(var p in Portals)
      {
        await p.SingInAsync();
      }
    }

    private async void InitMap()
    {
      Portal.WebMapId = "d847253ac7384eb4a41308c6c0edf3e6";
      var map = await Portal.GetMapAsync();
      Map = map;
      Map.Loaded += (o, e) =>
      {
        Layers = Map.OperationalLayers.ToList();
      };
      if(Map.LoadStatus == Esri.ArcGISRuntime.LoadStatus.NotLoaded)
      {
        await Map.LoadAsync();
      }
    }

  }
}
