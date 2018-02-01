using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Mapsui.Forms;
using Mapsui.Forms.Android;
using Android.Content;

// Export the rendererer, and associate it with our Mapsui Forms Control
[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]
namespace Mapsui.Forms.Android
{
	// Extend ViewRenderer and link it to our Forms Control, and to the MapsUI implementation for Android
	public class MapViewRenderer : ViewRenderer<MapView, Mapsui.UI.Android.MapControl>
	{
        // Mapsui Native Android implementation
        Mapsui.UI.Android.MapControl mapControl;

		// Our Mapsui Forms Control
		MapView mapView;

        public MapViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				MessagingCenter.Unsubscribe<MapView>(this, "Refresh");
			}

			if (mapView == null && e.NewElement != null)
			{
				// Get the MapsUI Forms control
				mapView = e.NewElement as MapView;
				
				// Subscribe messages for refreshing the map control
				MessagingCenter.Subscribe<MapView>(this, "Refresh", (sender) => {
					mapControl?.RefreshGraphics();
				});
			}

			if (mapControl == null)
			{
				// Set Native Android implementation
				mapControl = new Mapsui.UI.Android.MapControl(Context, null);

				// Link our Forms control to the native control
				mapControl.Map = mapView.Map;

				// Get events from Map
				mapControl.Map.PropertyChanged += mapView.MapPropertyChanged;

				// Set native app
				SetNativeControl(mapControl);
			}
		}
	}
}