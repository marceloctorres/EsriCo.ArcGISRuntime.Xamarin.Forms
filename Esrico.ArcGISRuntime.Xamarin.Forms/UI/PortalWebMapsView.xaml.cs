using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

using Esri.ArcGISRuntime.Portal;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI {
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PortalWebMapsView : ContentView {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty WebMapItemsProperty = BindableProperty.Create(
      nameof(WebMapItems),
      typeof(IEnumerable<PortalItem>),
      typeof(PortalWebMapsView),
      defaultBindingMode: BindingMode.OneWay);

    /// <summary>
    /// 
    /// </summary>
    public IEnumerable<PortalItem> WebMapItems {
      get => (List<PortalItem>)GetValue(WebMapItemsProperty);
      set => SetValue(WebMapItemsProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty GroupNameProperty = BindableProperty.Create(
      nameof(GroupName),
      typeof(string),
      typeof(PortalWebMapsView),
      defaultBindingMode: BindingMode.OneWay);

    /// <summary>
    /// 
    /// </summary>
    public string GroupName {
      get => (string)GetValue(GroupNameProperty);
      set => SetValue(GroupNameProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
      nameof(SelectedItem),
      typeof(PortalItem),
      typeof(PortalWebMapsView),
      defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// 
    /// </summary>
    public PortalItem SelectedItem {
      get => (PortalItem)GetValue(SelectedItemProperty);
      set => SetValue(SelectedItemProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty LoadMapCommandProperty = BindableProperty.Create(
      nameof(LoadMapCommand),
      typeof(ICommand),
      typeof(PortalWebMapsView));

    /// <summary>
    /// 
    /// </summary>
    public ICommand LoadMapCommand {
      get => (ICommand)GetValue(LoadMapCommandProperty);
      set => SetValue(LoadMapCommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CloseCommandProperty = BindableProperty.Create(
      nameof(CloseCommand),
      typeof(ICommand),
      typeof(PortalWebMapsView));

    /// <summary>
    /// 
    /// </summary>
    public ICommand CloseCommand {
      get => (ICommand)GetValue(CloseCommandProperty);
      set => SetValue(CloseCommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public PortalWebMapsView() {
      try {
        InitializeComponent();
      }
      catch(Exception ex) {
        Debug.WriteLine(ex.Message);
      }
    }
  }
}