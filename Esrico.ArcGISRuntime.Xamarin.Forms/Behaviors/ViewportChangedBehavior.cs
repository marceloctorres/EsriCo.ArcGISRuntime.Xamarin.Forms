using System;
using System.Windows.Input;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Xamarin.Forms;

using Prism.Behaviors;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors {
  /// <summary>
  /// 
  /// </summary>
  public class ViewportChangedBehavior : BehaviorBase<MapView> {

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty VisibleAreaProperty = BindableProperty.Create(
      nameof(VisibleArea),
      typeof(Polygon),
      typeof(ViewportChangedBehavior),
      defaultBindingMode: BindingMode.OneWayToSource);

    /// <summary>
    /// 
    /// </summary>
    public Polygon VisibleArea {
      get => (Polygon)GetValue(VisibleAreaProperty);
      set => SetValue(VisibleAreaProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty UnitsPerPixelProperty = BindableProperty.Create(
      nameof(UnitsPerPixel),
      typeof(double),
      typeof(ViewportChangedBehavior),
      defaultBindingMode: BindingMode.OneWayToSource);

    /// <summary>
    /// 
    /// </summary>
    public double UnitsPerPixel {
      get => (double)GetValue(UnitsPerPixelProperty);
      set => SetValue(UnitsPerPixelProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapScaleProperty = BindableProperty.Create(
      nameof(MapScale),
      typeof(double),
      typeof(ViewportChangedBehavior),
      defaultBindingMode: BindingMode.OneWayToSource);

    /// <summary>
    /// 
    /// </summary>
    public double MapScale {
      get => (double)GetValue(MapScaleProperty);
      set => SetValue(MapScaleProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
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
    protected override void OnAttachedTo(MapView bindable) {
      base.OnAttachedTo(bindable);
      bindable.ViewpointChanged += Bindable_ViewpointChanged;
      UnitsPerPixel = bindable.UnitsPerPixel;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    protected override void OnDetachingFrom(MapView bindable) {
      base.OnDetachingFrom(bindable);
      bindable.ViewpointChanged -= Bindable_ViewpointChanged;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Bindable_ViewpointChanged(object sender, EventArgs e) {
      MapScale = AssociatedObject.MapScale;
      VisibleArea = AssociatedObject.VisibleArea;

      if(Command != null && AssociatedObject != null) {
        var currentViewpoint = AssociatedObject.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
        if(Command.CanExecute(currentViewpoint)) {
          Command.Execute(currentViewpoint);
        }
      }
    }
  }
}
