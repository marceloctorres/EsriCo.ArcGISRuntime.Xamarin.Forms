# Esri Colombia ArcGIS Runtime Tools for Xamarin.Forms
Esri Colombia ArcGIS Runtime Tools for Xamarin.Forms contains custom ControlViews and other type of component tools that enable extend 
[Xamarin.Forms Esri ArcGIS Runtime SDK for .NET](https://developers.arcgis.com/net/latest/forms/guide/guide-home.htm)

Anyone can use these tools in any project by installing latest release of the Nuget Package:
- [EsriCo.ArcGISRuntime.Xamarin.Forms](https://www.nuget.org/packages/EsriCo.ArcGISRuntime.Xamarin.Forms)

## Features
### UI 
- **DrawingStatusView**: Shows an activity indicator while map redraws.
- **DrawingToolBarView**: Shows a Toolbar that enables draw points, polylines, polygons and rectangle on the map.
- **IndentifyView**: Shows a PanelView with the attributes of features tapped on the map.
- **LegendView**: Shows a PanelView with a list of the legend of each layer of the map.
- **LogInView**: Shows a View to get credentials to connect to a portal.
- **ModalPanelView**: Customizable modal PanelView.
- **NavigationBarView**: Toolbar to zoom in, zoom out or go the initial extend on the map.
- **PanelView**: Customizable ContentView that can be close, move around a container ContentView
- **ProgressView**: Shows a modal message while background tasks ends.
- **TableOfContentsView**: Shows a PanelView with a list of layer each of one can be switched visible on or off.
### Behaviors
- **DrawingStatusChangedBehavior**
- **IdentifyBehavior**
- **SetMapViewViewportBehavior**
- **ShowCalloutForGeoElementBehavior**
- **ViewportChangedBehavior**
### Converters
- **GeographicCoordinateConverter**
- **SymbolToImageConverter**
### Services
- **ConfigurationInfo**
- **PortalConnection**
- **ReplicaManger**

## Resources
- [API](api/EsriCo.ArcGISRuntime.Xamarin.Forms.yml)
- [List of Features](features.md)
- [System Requirements](https://esri.github.io/arcgis-toolkit-dotnet/requirements.html)
- [ArcGIS Runtime SDK for .NET](https://developers.arcgis.com/net/latest/)
