using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using Esri.ArcGISRuntime.Data;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;

using Prism.Behaviors;

using Xamarin.Forms;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.Behaviors {
  /// <summary>
  /// 
  /// </summary>
  public class IdentifyBehavior : BehaviorBase<MapView> {
    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command),
        typeof(ICommand),
        typeof(IdentifyBehavior)
      );

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty GraphicsOverlaysProperty = BindableProperty.Create(
      nameof(GraphicsOverlays),
      typeof(List<GraphicsOverlay>),
      typeof(IdentifyBehavior)
      );

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty LayersProperty = BindableProperty.Create(
      nameof(Layers),
      typeof(List<Layer>),
      typeof(IdentifyBehavior)
      );

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ToleranceProperty = BindableProperty.Create(
      nameof(Tolerance),
      typeof(double),
      typeof(IdentifyBehavior)
      );

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ReturnOnlyPopupsProperty = BindableProperty.Create(
      nameof(ReturnOnlyPopups),
      typeof(bool),
      typeof(IdentifyBehavior)
      );

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MaxResultsProperty = BindableProperty.Create(
      nameof(MaxResults),
      typeof(long),
      typeof(IdentifyBehavior)
      );

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
    public List<GraphicsOverlay> GraphicsOverlays {
      get => (List<GraphicsOverlay>)GetValue(GraphicsOverlaysProperty);
      set => SetValue(GraphicsOverlaysProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public List<Layer> Layers {
      get => (List<Layer>)GetValue(LayersProperty);
      set => SetValue(LayersProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public double Tolerance {
      get => (double)GetValue(ToleranceProperty);
      set => SetValue(ToleranceProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public long MaxResults {
      get => (long)GetValue(MaxResultsProperty);
      set => SetValue(MaxResultsProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool ReturnOnlyPopups {
      get => (bool)GetValue(ReturnOnlyPopupsProperty);
      set => SetValue(ReturnOnlyPopupsProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public IdentifyBehavior() {
      Tolerance = 1;
      MaxResults = 10;
      ReturnOnlyPopups = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    protected override void OnAttachedTo(MapView bindable) {
      base.OnAttachedTo(bindable);
      bindable.GeoViewTapped += GeoViewTapped;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void GeoViewTapped(object sender, GeoViewInputEventArgs e) {
      if(Command != null && AssociatedObject.Map != null) {
        object item = null;
        if(Command.CanExecute(item)) {
          var identifyResults = new IdentifyResults();

          if(GraphicsOverlays != null && GraphicsOverlays.Count > 0) {
            var identifyGraphicOverlayResults = await AssociatedObject.IdentifyGraphicsOverlaysAsync(
              e.Position,
              Tolerance,
              ReturnOnlyPopups,
              MaxResults);

            if(identifyGraphicOverlayResults != null) {
              identifyResults
                .GraphicsResults = (from r in identifyGraphicOverlayResults
                                    from g in r.Graphics
                                    where !(g.Symbol is TextSymbol) &&
                                    GraphicsOverlays.Select(go => go.Id)
                                      .Contains(g.GraphicsOverlay.Id)
                                    select new IdentifyGraphicResult() { Graphic = g })
                                    .ToList();
            }
          }
          if(Layers != null && Layers.Count > 0) {
            var identifyLayerResults = await AssociatedObject.IdentifyLayersAsync(
              e.Position,
              Tolerance,
              ReturnOnlyPopups,
              MaxResults);
            if(identifyLayerResults != null) {
              identifyResults
                .GeoElementResults
                .AddRange(from ir in identifyLayerResults
                          from ge in ir.GeoElements
                          select new IdentifyGeoElementResult {
                            GeoElement = ge as Feature,
                            Layer = ir.LayerContent as Layer
                          });
              identifyResults.
                GeoElementResults.
                AddRange(from ir in identifyLayerResults
                         from sr in ir.SublayerResults
                         from ge in sr.GeoElements
                         select new IdentifyGeoElementResult {
                           GeoElement = ge as Feature,
                           Layer = sr.LayerContent as Layer
                         });
            }
          }
          Command.Execute(identifyResults);
        }
      }
    }
  }
}
