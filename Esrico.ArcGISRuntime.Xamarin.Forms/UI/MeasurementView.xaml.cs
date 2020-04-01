using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime.Xamarin.Forms;

using EsriCo.ArcGISRuntime.Xamarin.Forms.Extensions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EsriGeometry = Esri.ArcGISRuntime.Geometry;

namespace EsriCo.ArcGISRuntime.Xamarin.Forms.UI
{
  /// <summary>
  /// 
  /// </summary>
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class MeasurementBarView : PanelView
  {
    public class UnitItem
    {
      /// <summary>
      /// 
      /// </summary>
      public string DisplayName { get; set; }

      /// <summary>
      /// 
      /// </summary>
      public string Key { get; set; }


      public object Value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(
      nameof(Color),
      typeof(Color),
      typeof(MeasurementBarView),
      defaultValue: Color.DarkCyan,
      propertyChanged: OnColorPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    public Color Color
    {
      get => (Color)GetValue(ColorProperty);
      set => SetValue(ColorProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue != null)
      {
        view.Color = (Color)newValue;
        view.DrawingProcess.Color = view.Color;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private const string DrawGrapichsOverlayId = "MeasurementGraphicsOverlay";

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty PointMeasurementToolImageProperty = BindableProperty.Create(
      nameof(PointMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnPointMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource PointMeasurementToolImage
    {
      get => (ImageSource)GetValue(PointMeasurementToolImageProperty);
      set => SetValue(PointMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnPointMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue == null)
      {
        view.PointMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_coord"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty LineMeasurementToolImageProperty = BindableProperty.Create(
      nameof(LineMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnLineMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource LineMeasurementToolImage
    {
      get => (ImageSource)GetValue(LineMeasurementToolImageProperty);
      set => SetValue(LineMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnLineMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue == null)
      {
        view.LineMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_distance"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty AreaMeasurementToolImageProperty = BindableProperty.Create(
      nameof(AreaMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnAreaMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource AreaMeasurementToolImage
    {
      get => (ImageSource)GetValue(AreaMeasurementToolImageProperty);
      set => SetValue(AreaMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnAreaMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue == null)
      {
        view.AreaMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_area"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty NoneMeasurementToolImageProperty = BindableProperty.Create(
      nameof(NoneMeasurementToolImage),
      typeof(ImageSource),
      typeof(MeasurementBarView),
      propertyChanged: OnNoneMeasurementToolImageChanged);

    /// <summary>
    /// 
    /// </summary>
    public ImageSource NoneMeasurementToolImage
    {
      get => (ImageSource)GetValue(NoneMeasurementToolImageProperty);
      set => SetValue(NoneMeasurementToolImageProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnNoneMeasurementToolImageChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var view = bindable as MeasurementBarView;
      if(newValue == null)
      {
        view.NoneMeasurementToolImage = ImageSource.FromStream(() =>
          typeof(MeasurementBarView).Assembly.GetStreamEmbeddedResource(@"ic_cancel"));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty MapViewProperty = BindableProperty.Create(
      nameof(MapView),
      typeof(MapView),
      typeof(MeasurementBarView),
      propertyChanged: OnMapViewPropertyChanged);

    /// <summary>
    /// 
    /// </summary>
    public MapView MapView
    {
      get => (MapView)GetValue(MapViewProperty);
      set => SetValue(MapViewProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bindable"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private static void OnMapViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
      var panelView = bindable as MeasurementBarView;
      if(newValue is MapView newMapView)
      {
        if(newMapView.Map != null)
        {
          panelView.CheckMap(newMapView);
        }
        else
        {
          newMapView.PropertyChanged += (s, e) =>
          {
            if(e.PropertyName == nameof(newMapView.Map))
            {
              panelView.CheckMap(newMapView);
            }
          };
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="map"></param>
    public void CheckMap(MapView mapView)
    {
      IsVisible = mapView != null && mapView.Map != null;
      if(IsVisible)
      {
        if(mapView.GraphicsOverlays == null)
        {
          mapView.GraphicsOverlays = new GraphicsOverlayCollection();
        }
        var graphicsOverlay = mapView.GraphicsOverlays.Where(g => g.Id == DrawGrapichsOverlayId).FirstOrDefault();
        if(graphicsOverlay == null)
        {
          DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
          mapView.GraphicsOverlays.Add(DrawingProcess.DrawGraphicsOverlay);
        }
        DrawingProcess.MapView = mapView;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly BindableProperty IsDrawingProperty = BindableProperty.Create(
      nameof(IsDrawing),
      typeof(bool),
      typeof(MeasurementBarView),
      defaultValue: false,
      defaultBindingMode: BindingMode.OneWayToSource);

    /// <summary>
    /// 
    /// </summary>
    public bool IsDrawing
    {
      get => (bool)GetValue(IsDrawingProperty);
      set => SetValue(IsDrawingProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    private DrawingProcess DrawingProcess { get; set; }

    private string _text;

    /// <summary>
    /// 
    /// </summary>
    public string ResultText
    {
      get => _text;
      set
      {
        _text = value;
        OnPropertyChanged(nameof(ResultText));
      }
    }

    private UnitItem _selectedItem;

    /// <summary>
    /// 
    /// </summary>
    public UnitItem SelectedUnit
    {
      get => _selectedItem;
      set
      {
        _selectedItem = value;
        OnPropertyChanged(nameof(SelectedUnit));
      }
    }

    private List<UnitItem> _units;

    /// <summary>
    /// 
    /// </summary>
    public List<UnitItem> Units
    {
      get => _units;
      set
      {
        _units = value;
        OnPropertyChanged(nameof(Units));
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public UnitItem SelectedAngularUnit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UnitItem SelectedLinearUnit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public UnitItem SelectedAreaUnit { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<UnitItem> AngularUnits { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public List<UnitItem> LinearUnits { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public List<UnitItem> AreaUnits { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    private Geometry Geometry { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public MeasurementBarView()
    {
      InitializeComponent();
      IsVisible = false;

      PointMeasurementToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_coord"));
      LineMeasurementToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_distance"));
      AreaMeasurementToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_area"));
      NoneMeasurementToolImage = ImageSource.FromStream(() => GetType().Assembly.GetStreamEmbeddedResource(@"ic_cancel"));

      AngularUnits = new List<UnitItem>()
      {
        new UnitItem() { DisplayName = AppResources.DecimalDegrees, Key = "DecimalDegrees", Value = LatitudeLongitudeFormat.DecimalDegrees},
        new UnitItem() { DisplayName = AppResources.DegreesDecimalMinutes, Key = "DegreesDecimalMinutes", Value = LatitudeLongitudeFormat.DegreesDecimalMinutes},
        new UnitItem() { DisplayName = AppResources.DegreesMinutesSeconds, Key = "DegreesMinutesSeconds", Value = LatitudeLongitudeFormat.DegreesMinutesSeconds }
      };

      LinearUnits = new List<UnitItem>()
      {
        new UnitItem() { DisplayName = AppResources.Meters, Key = "Meters", Value = EsriGeometry.LinearUnits.Meters},
        new UnitItem() { DisplayName = AppResources.Kilometers, Key = "Kilometers", Value = EsriGeometry.LinearUnits.Kilometers},
        new UnitItem() { DisplayName = AppResources.Feet, Key = "Feet", Value = EsriGeometry.LinearUnits.Feet},
        new UnitItem() { DisplayName = AppResources.Yards, Key = "Yards", Value = EsriGeometry.LinearUnits.Yards},
        new UnitItem() { DisplayName = AppResources.Miles, Key = "Miles", Value = EsriGeometry.LinearUnits.Miles}

      };
      AreaUnits = new List<UnitItem>()
      {
        new UnitItem() { DisplayName = AppResources.SquareMeters, Key = "SquareMeters", Value = EsriGeometry.AreaUnits.SquareMeters },
        new UnitItem() { DisplayName = AppResources.Hectares, Key = "Hectares", Value = EsriGeometry.AreaUnits.Hectares },
        new UnitItem() { DisplayName = AppResources.SquareKilometers, Key = "SquareKilometers", Value = EsriGeometry.AreaUnits.SquareKilometers },
        new UnitItem() { DisplayName = AppResources.SquareFeet, Key = "SquareFeet", Value = EsriGeometry.AreaUnits.SquareFeet },
        new UnitItem() { DisplayName = AppResources.SquareYards, Key = "SquareYards", Value = EsriGeometry.AreaUnits.SquareYards },
        new UnitItem() { DisplayName = AppResources.Acres, Key = "Acres", Value = EsriGeometry.AreaUnits.Acres },
        new UnitItem() { DisplayName = AppResources.SquareMiles, Key = "SquareMiles", Value = EsriGeometry.AreaUnits.SquareMiles }
      };

      DrawingProcess = new DrawingProcess()
      {
        SketchEditor = new SketchEditor(),
        Color = Color,
        DrawGraphicsOverlay = new GraphicsOverlay() { Id = DrawGrapichsOverlayId }
      };
      DrawingProcess.SketchEditor.GeometryChanged += SketchEditor_GeometryChanged;
      DrawingProcess.PropertyChanged += DrawingProcess_PropertyChanged;

      DrawingProcess.PointCreated = g =>
      {
        Geometry = g;
        PointCreated(g);
      };
      DrawingProcess.PolylineCreated = g =>
      {
        Geometry = g;
        PolylineCreated(g);
      };
      DrawingProcess.PolygonCreated = g =>
      {
        PolygonCreated(g);
      };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SketchEditor_GeometryChanged(object sender, GeometryChangedEventArgs e) 
    {
      Geometry = e.NewGeometry;
      if(Geometry is Polyline) 
      {
        PolylineCreated(Geometry);
      }
      else if(Geometry is Polygon)
      {
        PolygonCreated(Geometry);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="geometry"></param>
    private void PolygonCreated(Geometry geometry)
    {
      Geometry = geometry;

      var unit = (AreaUnit)SelectedAreaUnit.Value;
      var result = GeometryEngine.AreaGeodetic(geometry, unit, GeodeticCurveType.Geodesic);
      ResultText = $"{result:0.00} {unit.Abbreviation}";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="geometry"></param>
    private void PolylineCreated(Geometry geometry)
    {
      Geometry = geometry;

      var unit = (LinearUnit)SelectedLinearUnit.Value;
      var result = GeometryEngine.LengthGeodetic(geometry, unit, GeodeticCurveType.Geodesic);
      ResultText = $"{result:0.00} {unit.Abbreviation}";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="geometry"></param>
    private void PointCreated(Geometry geometry)
    {
      Geometry = geometry;

      var result = CoordinateFormatter
        .ToLatitudeLongitude((MapPoint)geometry, (LatitudeLongitudeFormat)SelectedAngularUnit.Value, 3);
      ResultText = result;
    }

    /// <summary>
    /// 
    /// </summary>
    private void ClearMeasurement()
    {
      if(DrawingProcess.SketchEditor.CancelCommand != null && DrawingProcess.SketchEditor.CancelCommand.CanExecute(null))
      {
        DrawingProcess.SketchEditor.CancelCommand.Execute(null);
      }
      DrawingProcess.SketchEditor.Stop();
      DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
      Geometry = null;
      ResultText = string.Empty;
      Units = null;
      SelectedUnit = null;
      IsDrawing = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DrawingProcess_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if(e.PropertyName == nameof(DrawingProcess.IsDrawing) && DrawingProcess.IsDrawing)
      {
        IsDrawing = DrawingProcess.IsDrawing;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void PointMeasurementToolClicked(object sender, EventArgs e)
    {
      DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
      Geometry = null;
      ResultText = string.Empty;

      Units = AngularUnits;
      if(SelectedAngularUnit == null)
      {
        SelectedAngularUnit = AngularUnits.Where(u => u.Key == nameof(LatitudeLongitudeFormat.DegreesMinutesSeconds)).FirstOrDefault();
      }
      SelectedUnit = SelectedAngularUnit;

      await DrawingProcess.DrawGeometry(SketchCreationMode.Point);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void LineMeasurementToolClicked(object sender, EventArgs e)
    {
      DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
      Geometry = null;
      ResultText = string.Empty;

      Units = LinearUnits;
      if(SelectedLinearUnit == null)
      {
        SelectedLinearUnit = LinearUnits.Where(u => u.Key == nameof(EsriGeometry.LinearUnits.Kilometers)).FirstOrDefault();
      }
      SelectedUnit = SelectedLinearUnit;
      await DrawingProcess.DrawGeometry(SketchCreationMode.Polyline);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void AreaMeasurementToolClicked(object sender, EventArgs e)
    {
      DrawingProcess.DrawGraphicsOverlay.Graphics.Clear();
      Geometry = null;
      ResultText = string.Empty;

      Units = AreaUnits;
      if(SelectedAreaUnit == null)
      {
        SelectedAreaUnit = AreaUnits.Where(u => u.Key == nameof(EsriGeometry.AreaUnits.SquareKilometers)).FirstOrDefault();
      }
      SelectedUnit = SelectedAreaUnit;

      await DrawingProcess.DrawGeometry(SketchCreationMode.Polygon);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PickerSelectedIndexChanged(object sender, EventArgs e)
    {
      var value = SelectedUnit?.Value;
      if(value is LatitudeLongitudeFormat)
      {
        SelectedAngularUnit = SelectedUnit;
        if(Geometry != null)
        {
          PointCreated(Geometry);
        }
      }
      else if(value is LinearUnit)
      {
        SelectedLinearUnit = SelectedUnit;
        if(Geometry != null)
        {
          PolylineCreated(Geometry);
        }
      }
      else if(value is AreaUnit)
      {
        SelectedAreaUnit = SelectedUnit;
        if(Geometry != null)
        {
          PolygonCreated(Geometry);
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MeasureViewClosed(object sender, EventArgs e)
    {
      ClearMeasurement();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NoneMeasurementToolClicked(object sender, EventArgs e)
    {
      ClearMeasurement();
    }
  }
}