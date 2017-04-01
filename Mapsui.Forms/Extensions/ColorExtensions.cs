namespace Mapsui.Forms.Extensions
{
	static class ColorExtensions
	{
		public static Xamarin.Forms.Color ToFormsColor(this Mapsui.Styles.Color color)
		{
			return new Xamarin.Forms.Color(color.R / 255, color.G / 255, color.B / 255, color.A / 255);
		}

		public static Mapsui.Styles.Color ToMapsuiColor(this Xamarin.Forms.Color color)
		{
			return new Mapsui.Styles.Color((int) (color.R * 255), (int) (color.G * 255), (int) (color.B * 255), (int) (color.A * 255));
		}
	}
}