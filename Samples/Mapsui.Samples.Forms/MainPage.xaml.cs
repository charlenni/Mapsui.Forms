using BruTile.Predefined;
using Mapsui.Forms;
using Mapsui.Layers;
using Mapsui.Projection;
using System;
using Xamarin.Forms;

namespace Mapsui.Samples.Forms
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

			mapView.PropertyChanged += (s, e) => System.Diagnostics.Debug.WriteLine(e.PropertyName);
			mapView.Map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.BingAerial)) { Name = "Bing Aerial" });
			mapView.Map.Viewport.ViewportChanged += (s, e) => System.Diagnostics.Debug.WriteLine(e.PropertyName);
		}

		void OnButtonClicked(object sender, EventArgs e)
		{
			// Get the lon lat coordinates from somewhere (Mapsui can not help you there)
			var centerOfLondonOntario = new Point(-81.2497, 42.9837);
			// OSM uses spherical mercator coordinates. So transform the lon lat coordinates to spherical mercator
			var sphericalMercatorCoordinate = SphericalMercator.FromLonLat(centerOfLondonOntario.X, centerOfLondonOntario.Y);
			// Set the center of the viewport to the coordinate. The UI will refresh automatically
			mapView.Map.Viewport.Center = sphericalMercatorCoordinate;
			// Additionally you might want to set the resolution, this could depend on your specific purpose
			mapView.Map.Viewport.Resolution = mapView.Map.Resolutions[9];
			mapView.BackgroundColor = Color.Red;
			mapView.MoveToCenter(new Xamarin.Forms.Maps.Position(48.4789167, 9.2706));
//			mapView.MoveToRegion(new Xamarin.Forms.Maps.MapSpan(new Xamarin.Forms.Maps.Position(48.4789167, 9.2706), 0.01, 0.01));
			var test = mapView.VisibleRegion;
		}
	}
}