using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Mapsui.Forms;
using Mapsui.Forms.iOS;
using UIKit;
using CoreGraphics;
using Mapsui.Forms.Extensions;

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
                mapControl = new Mapsui.UI.iOS.MapControl(nativeRect);

                mapControl.Map = mapView.Map;
                mapControl.Frame = nativeRect;
                mapControl.Map.NavigateTo(mapView.LastMoveToRegion.ToMapsui());

                // Get events from Map
                mapControl.Map.PropertyChanged += mapView.MapPropertyChanged;

                // Set native app
                SetNativeControl(mapControl);
			}
		}

        public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
        {
            return Control.GetSizeRequest(widthConstraint, heightConstraint);
        }

        bool _shouldUpdateRegion = true;

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_shouldUpdateRegion)
            {
                _shouldUpdateRegion = false;
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (mapControl != null)
                {
                    MessagingCenter.Unsubscribe<MapView>(this, "Refresh");
                }
            }

            base.Dispose(disposing);
        }
    }
}