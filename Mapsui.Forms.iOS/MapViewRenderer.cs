using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Mapsui.Forms;
using Mapsui.Forms.iOS;
using UIKit;

// Export the rendererer, and associate it with our Mapsui Forms Control
[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]
namespace Mapsui.Forms.iOS
{
	// Extend ViewRenderer and link it to our Forms Control, and to the Mapsui implementation for iOS
	[Foundation.Preserve(AllMembers = true)]
	public class MapViewRenderer : ViewRenderer<MapView, Mapsui.UI.iOS.MapControl>
	{
		// Mapsui Native iOS implementation
		Mapsui.UI.iOS.MapControl mapNativeControl;

		// Our Mapsui Forms Control
		MapView mapViewControl;

		public MapViewRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				MessagingCenter.Unsubscribe<MapView>(this, "Refresh");
			}

			if (mapViewControl == null && e.NewElement != null)
			{
				// Get the Mapsui Forms control
				mapViewControl = e.NewElement as MapView;
				// Subscribe messages for refreshing the map control
				MessagingCenter.Subscribe<MapView>(this, "Refresh", (sender) => {
					mapNativeControl?.RefreshGraphics();
				});
			}

			if (mapNativeControl == null)
			{
				// Set Native iOS implementation
				mapNativeControl = new Mapsui.UI.iOS.MapControl(UIScreen.MainScreen.NativeBounds);

				// Link our Forms Control to the Native control
				mapNativeControl.Map = mapViewControl.Map;

				// Not sure what this is for, but necessary for Mapsui on iOS
				//mapControl.Run(60.0);
				mapNativeControl.Frame = UIScreen.MainScreen.NativeBounds;

				// Set native app
				SetNativeControl(mapNativeControl);
			}
		}
	}
}