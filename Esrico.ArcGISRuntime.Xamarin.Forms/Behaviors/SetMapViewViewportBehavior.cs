using System;
using System.Runtime.CompilerServices;

using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Xamarin.Forms;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;

using Prism.Behaviors;

using Xamarin.Forms;

/// <summary>
/// 
/// </summary>
namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors
{
  /// <summary>
  /// 
  /// </summary>
  public class SetViewpointBehavior : BehaviorBase<GeoView>
  {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ViewpointProperty = BindableProperty.Create(
      nameof(Viewpoint), 
      typeof(Viewpoint), 
      typeof(SetViewpointBehavior));

    /// <summary>
    /// 
    /// </summary>
    public Viewpoint Viewpoint
    {
      get { return (Viewpoint)GetValue(ViewpointProperty); }
      set { SetValue(ViewpointProperty, value); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      base.OnPropertyChanged(propertyName);
      if (propertyName == nameof(Viewpoint))
      {
        SetViewpoint(this.Viewpoint);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="viewpoint"></param>
    private async void SetViewpoint(Viewpoint viewpoint)
    {
      if (viewpoint != null)
      {
        var currentViewpoint = AssociatedObject.GetCurrentViewpoint(ViewpointType.BoundingGeometry);
        if (currentViewpoint == null || (currentViewpoint != null && !currentViewpoint.AreEquals(viewpoint)))
        {
          await AssociatedObject.SetViewpointAsync(viewpoint);
        }
      }
    }
  }
}
