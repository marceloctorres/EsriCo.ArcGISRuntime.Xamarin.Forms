using Prism.Ioc;
using Prism.Modularity;
using EsriCo.ArcGISRuntime.Xamarin.Forms.Views;
using EsriCo.ArcGISRuntime.Xamarin.Forms.ViewModels;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms
{
  public class FormsModule : IModule
  {
    public void OnInitialized(IContainerProvider containerProvider)
    {

    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
    }
  }
}
