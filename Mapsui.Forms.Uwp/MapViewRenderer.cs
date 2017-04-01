using Xamarin.Forms.Platform.UWP;
using Xamarin.Forms;
using Mapsui.Forms;
using Mapsui.Forms.Uwp;

// Export the rendererer, and associate it with out MapsUI Forms Control
[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]
namespace Mapsui.Forms.Uwp
{
	// Extend ViewRenderer and link it to our Forms Control, and to the MapsUI implementation for UWP
	public class MapViewRenderer : ViewRenderer<MapView, Mapsui.UI.Uwp.MapControl>
	{
		// MapsUI Native UWP implementation
		Mapsui.UI.Uwp.MapControl mapNativeControl;

		// Our MapsUI Forms Control
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
				// Set Native UWP implementation
				mapNativeControl = new Mapsui.UI.Uwp.MapControl();

				// Link our Forms control to the Native control
				mapNativeControl.Map = mapViewControl.Map;

				// Get events from Map
				mapNativeControl.Map.PropertyChanged += mapViewControl.MapPropertyChanged;

				// Set native app
				SetNativeControl(mapNativeControl);
			}
		}
	}
}