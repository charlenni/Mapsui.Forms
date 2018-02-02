namespace Mapsui.Forms.Extensions
{
    public static class MapSpanExtensions
    {
        /// <summary>
        /// Convert Mapsui.Geometries.BoundingBox to Xamarin.Forms.Maps.MapSpan
        /// </summary>
        /// <param name="rect">Rectangle in Mapsui.Geometries.BoundingBox format</param>
        /// <returns>MapSpan in Xamarin.Forms.Maps format</returns>
        public static Xamarin.Forms.Maps.MapSpan ToForms(this Mapsui.Geometries.BoundingBox rect)
        {
            var center = rect.GetCentroid().ToForms();
            var bottomLeft = rect.BottomLeft.ToForms();
            var topRight = rect.TopRight.ToForms();

            return new Xamarin.Forms.Maps.MapSpan(center, (topRight.Latitude - bottomLeft.Latitude) / 2, (topRight.Longitude - bottomLeft.Longitude) / 2);
        }

        /// <summary>
        /// Convert Xamarin.Forms.Maps.MapSpan to Mapsui.Geometries.BoundingBox
        /// </summary>
        /// <param name="rect">Rectangle in Xamarin.Forms.Maps.MapSpan </param>
        /// <returns>BoundingBox format</returns>
        public static Mapsui.Geometries.BoundingBox ToMapsui(this Xamarin.Forms.Maps.MapSpan rect)
        {
            // Found at https://forums.xamarin.com/discussion/22493/maps-visibleregion-bounds

            var center = rect.Center;
            var halfheightDegrees = rect.LatitudeDegrees / 2;
            var halfwidthDegrees = rect.LongitudeDegrees / 2;

            var left = center.Longitude - halfwidthDegrees;
            var right = center.Longitude + halfwidthDegrees;
            var top = center.Latitude + halfheightDegrees;
            var bottom = center.Latitude - halfheightDegrees;

            // Adjust for Internation Date Line (+/- 180 degrees longitude)
            if (left < -180) left = 180 + (180 + left);
            if (right > 180) right = (right - 180) - 180;
            // I don't wrap around north or south; I don't think the map control allows this anyway

            var bottomLeft = new Xamarin.Forms.Maps.Position(bottom, left).ToMapsui();
            var topRight = new Xamarin.Forms.Maps.Position(top, right).ToMapsui();

            return new Geometries.BoundingBox(bottomLeft, topRight);
        }
    }
}
