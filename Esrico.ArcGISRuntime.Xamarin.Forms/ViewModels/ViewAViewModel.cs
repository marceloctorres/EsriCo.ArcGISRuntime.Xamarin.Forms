using Prism.Mvvm;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.ViewModels
{
  public class ViewAViewModel : BindableBase
  {
    private string _title;
    public string Title
    {
      get { return _title; }
      set { SetProperty(ref _title, value); }
    }

    public ViewAViewModel()
    {
      Title = "View A";
    }
  }
}
