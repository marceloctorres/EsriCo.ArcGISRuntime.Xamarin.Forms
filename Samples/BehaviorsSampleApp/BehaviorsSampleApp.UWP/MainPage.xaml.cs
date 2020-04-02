using Prism;
using Prism.Ioc;

namespace BehaviorsSampleApp.UWP
{
  public sealed partial class MainPage
  {
    public MainPage()
    {
      InitializeComponent();

      LoadApplication(new BehaviorsSampleApp.App(new UwpInitializer()));
    }
  }

  public class UwpInitializer : IPlatformInitializer
  {
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      // Register any platform specific implementations
    }
  }
}
