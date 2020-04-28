using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;
using EsriCo.ArcGISRuntime.Xamarin.Forms.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class PortalConnectionsView : ContentView
  {
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
    public List<PortalConnection> PortalConnections
    {
      get => (List<PortalConnection>)GetValue(PortalConnectionsProperty);
      set => SetValue(PortalConnectionsProperty, value);
    }

    private ImageSource _logedInImage;

    /// <summary>
    /// 
    /// </summary>
    public ImageSource LoginImage
    {
      get => _logedInImage;
      set => _logedInImage = value;
    }

    private ImageSource _activeImage;

    /// <summary>
    /// 
    /// </summary>
    public ImageSource ActiveImage
    {
      get => _activeImage;
      set => _activeImage = value;
    }


    public PortalConnectionsView()
    {
      InitializeComponent();
      var asm = GetType().Assembly;

      LoginImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_key"));
      ActiveImage = ImageSource.FromStream(() => asm.GetStreamEmbeddedResource(@"ic_checked"));

    }
  }
}