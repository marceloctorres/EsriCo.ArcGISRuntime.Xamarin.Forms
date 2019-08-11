using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;
using Prism.Behaviors;
using System.Windows.Input;
using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors
{
  public class DrawingStatusChangedBehavior : BehaviorBase<GeoView>
  {
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
      nameof(Command),
      typeof(ICommand),
      typeof(ViewportChangedBehavior));

    public ICommand Command
    {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }

    protected override void OnAttachedTo(GeoView bindable)
    {
      base.OnAttachedTo(bindable);
      bindable.DrawStatusChanged += Bindable_DrawStatusChanged;
    }

    protected override void OnDetachingFrom(GeoView bindable)
    {
      base.OnDetachingFrom(bindable);
      bindable.DrawStatusChanged -= Bindable_DrawStatusChanged;
    }

    private void Bindable_DrawStatusChanged(object sender, DrawStatusChangedEventArgs e)
    {
      if (Command != null)
      {
        bool inProgress = e.Status == DrawStatus.InProgress;
        if (Command.CanExecute(inProgress))
        {
          Command.Execute(inProgress);
        }
      }
    }

  }
}
