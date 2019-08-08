using System;
using System.Windows.Input;

using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Xamarin.Forms;

using Prism.Behaviors;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors
{
  public class ViewportChangedBehavior : BehaviorBase<GeoView>
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
      bindable.ViewpointChanged += Bindable_ViewpointChanged;
    }

    protected override void OnDetachingFrom(GeoView bindable)
    {
      base.OnDetachingFrom(bindable);
      bindable.ViewpointChanged -= Bindable_ViewpointChanged;
    }

    private void Bindable_ViewpointChanged(object sender, EventArgs e)
    {
      if (Command != null)
      {
        if (AssociatedObject != null)
        {
          var currentViewpoint = AssociatedObject.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
          if (this.Command.CanExecute(currentViewpoint))
          {
            this.Command.Execute(currentViewpoint);
          }
        }
      }
    }

  }
}
