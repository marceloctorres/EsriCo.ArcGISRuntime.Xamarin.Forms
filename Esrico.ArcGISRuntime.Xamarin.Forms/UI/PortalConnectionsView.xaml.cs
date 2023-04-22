using System.Collections.Generic;
using System.Windows.Input;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;
using EsriCo.ArcGISRuntime.Xamarin.Forms.Services;

using Prism.Commands;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI {
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PortalConnectionsView : ContentView {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty PortalConnectionsProperty = BindableProperty.Create(
      nameof(PortalConnections),
      typeof(List<PortalConnection>),
      typeof(PortalConnectionsView),
      defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// 
    /// </summary>
    public List<PortalConnection> PortalConnections {
      get => (List<PortalConnection>)GetValue(PortalConnectionsProperty);
      set => SetValue(PortalConnectionsProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public ImageSource LoginImage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ImageSource ActiveImage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public ICommand CloseCommand { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public PortalConnectionsView() {
      InitializeComponent();
      var asm = GetType().Assembly;

      LoginImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_key"));
      ActiveImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_checked"));
      CloseCommand = new DelegateCommand(() => {
        IsVisible = false;
      }
      );

    }

    private void CloseButton_Clicked(object sender, System.EventArgs e) {
      IsVisible = false;
    }
  }
}