
using System;
using System.Linq;
using System.Windows.Input;
using Esri.ArcGISRuntime.Mapping.Popups;
using EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors;
using Prism.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class IdentifyView : ContentView
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IdentifyResultsProperty = BindableProperty.Create(
      nameof(IdentifyResults),
      typeof(IdentifyResults),
      typeof(IdentifyView),
      propertyChanged:OnIdentifyResultsChanged);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBorderColorProperty = BindableProperty.Create(
      nameof(TitleBorderColor),
      typeof(Color),
      typeof(IdentifyView), 
      defaultValue: null);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleBackgroundColorProperty = BindableProperty.Create(
      nameof(TitleBackgroundColor),
      typeof(Color),
      typeof(IdentifyView),
      defaultValue: Color.White);

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(
      nameof(TitleTextColor),
      typeof(Color),
      typeof(IdentifyView),
      defaultValue: Color.Black);

    /// <summary>
    /// 
    /// </summary>
    public IdentifyResults IdentifyResults
    {
      get => (IdentifyResults)GetValue(IdentifyResultsProperty);
      set => SetValue(IdentifyResultsProperty, value);
    }

    private PopupManager _popupManager;
    /// <summary>
    /// 
    /// </summary>
    public PopupManager PopupManager {
      get => _popupManager;
      set
      {
        _popupManager = value;
        OnPropertyChanged(nameof(PopupManager));
      }
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

    /// <summary>
    /// 
    /// </summary>
    public ICommand PreviousResultCommand { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public ICommand NextResultCommand { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public ICommand HideViewCommand { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    private int CurrentElementIndex { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IdentifyView()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnIdentifyResultsChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var identifyView = bindable as IdentifyView;
      if (newValue is IdentifyResults identifyResults && identifyResults.GeoElementResults.Count > 0)
      {
        Popup popup;
        var geoElementResult = identifyResults.GeoElementResults.First();
        if (geoElementResult.Layer is IPopupSource)
        {
          var popupSource = geoElementResult.Layer as IPopupSource;
          var popupDefinition = popupSource.PopupDefinition;
          popup = new Popup(geoElementResult.GeoElement, popupDefinition);
        }
        else
        {
          popup = Popup.FromGeoElement(geoElementResult.GeoElement);
        }
        if (popup != null)
        {
          identifyView.PopupManager = new PopupManager(popup as Popup);
        }
      }
    }

    private static void GetPopupManager (IdentifyView identifyView, IdentifyResults identifyResults)
    {
      Popup popup;
      var geoElementResult = identifyResults.GeoElementResults.First();
      if (geoElementResult.Layer is IPopupSource)
      {
        var popupSource = geoElementResult.Layer as IPopupSource;
        var popupDefinition = popupSource.PopupDefinition;
        popup = new Popup(geoElementResult.GeoElement, popupDefinition);
      }
      else
      {
        popup = Popup.FromGeoElement(geoElementResult.GeoElement);
      }
      if (popup != null)
      {
        identifyView.PopupManager = new PopupManager(popup as Popup);
      }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PreviousResulteClicked(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NextResultClicked(object sender, EventArgs e)
    {

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