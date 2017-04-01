using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Mapsui.Forms;
using Mapsui.Forms.Android;

// Export the rendererer, and associate it with our Mapsui Forms Control
[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]
namespace Mapsui.Forms.Android
{
	// Extend ViewRenderer and link it to our Forms Control, and to the MapsUI implementation for Android
	public class MapViewRenderer : ViewRenderer<MapView, Mapsui.UI.Android.MapControl>
	{
		// Mapsui Native Android implementation
		Mapsui.UI.Android.MapControl mapNativeControl;

		// Our Mapsui Forms Control
		MapView mapViewControl;

		protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				MessagingCenter.Unsubscribe<MapView>(this, "Refresh");
			}

			if (mapViewControl == null && e.NewElement != null)
			{
				// Get the MapsUI Forms control
				mapViewControl = e.NewElement as MapView;
				
				// Subscribe messages for refreshing the map control
				MessagingCenter.Subscribe<MapView>(this, "Refresh", (sender) => {
					mapNativeControl?.RefreshGraphics();
				});
			}

			if (mapNativeControl == null)
			{
				// Set Native Android implementation
				mapNativeControl = new Mapsui.UI.Android.MapControl(Context, null);

				// Link our Forms control to the native control
				mapNativeControl.Map = mapViewControl.Map;

				// Get events from Map
				mapNativeControl.Map.PropertyChanged += mapViewControl.MapPropertyChanged;

				// Set native app
				SetNativeControl(mapNativeControl);
			}
		}
	}
}