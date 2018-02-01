using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Mapsui.Forms;
using Mapsui.Forms.iOS;
using UIKit;
using CoreGraphics;

// Export the rendererer, and associate it with our Mapsui Forms Control
[assembly: ExportRenderer(typeof(MapView), typeof(MapViewRenderer))]
namespace Mapsui.Forms.iOS
{
	// Extend ViewRenderer and link it to our Forms Control, and to the Mapsui implementation for iOS
	[Foundation.Preserve(AllMembers = true)]
	public class MapViewRenderer : ViewRenderer<MapView, Mapsui.UI.iOS.MapControl>
	{
		// Mapsui Native iOS implementation
		Mapsui.UI.iOS.MapControl mapControl;

		// Our Mapsui Forms Control
		MapView mapView;

		protected override void OnElementChanged(ElementChangedEventArgs<MapView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				MessagingCenter.Unsubscribe<MapView>(this, "Refresh");
			}

			if (mapView == null && e.NewElement != null)
			{
				// Get the Mapsui Forms control
				mapView = e.NewElement as MapView;
				// Subscribe messages for refreshing the map control
				MessagingCenter.Subscribe<MapView>(this, "Refresh", (sender) => {
					mapControl?.RefreshGraphics();
				});
			}

			if (mapControl == null)
			{
                var formsRect = mapView.Bounds;
                var nativeRect = new CGRect(formsRect.X, formsRect.Y, formsRect.Width, formsRect.Height);

                // Set Native iOS implementation
                mapControl = new Mapsui.UI.iOS.MapControl(nativeRect)
                {
                    Map = mapView.Map,
                    Frame = nativeRect
                };

				// Set native app
				SetNativeControl(mapControl);
			}
		}
    }
}