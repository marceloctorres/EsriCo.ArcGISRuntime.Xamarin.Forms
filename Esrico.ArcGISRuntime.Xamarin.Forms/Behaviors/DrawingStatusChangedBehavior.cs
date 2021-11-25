using System.Windows.Input;

using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;

using Prism.Behaviors;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors {
  /// <summary>
  /// 
  /// </summary>
  public class DrawingStatusChangedBehavior : BehaviorBase<GeoView> {
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
      nameof(Command),
      typeof(ICommand),
      typeof(ViewportChangedBehavior));

    /// <summary>
    /// 
    /// </summary>
    public ICommand Command {
      get => (ICommand)GetValue(CommandProperty);
      set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    protected override void OnAttachedTo(GeoView bindable) {
      base.OnAttachedTo(bindable);
      bindable.DrawStatusChanged += Bindable_DrawStatusChanged;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    protected override void OnDetachingFrom(GeoView bindable) {
      base.OnDetachingFrom(bindable);
      bindable.DrawStatusChanged -= Bindable_DrawStatusChanged;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Bindable_DrawStatusChanged(object sender, DrawStatusChangedEventArgs e) {
      if(Command != null) {
        var inProgress = e.Status == DrawStatus.InProgress;
        if(Command.CanExecute(inProgress)) {
          Command.Execute(inProgress);
        }
      }
    }
  }
}
