namespace Mapsui.Forms.Extensions
{
	/// <summary>
	/// Color conversion extensions
	/// </summary>
	static class ColorExtensions
	{
		/// <summary>
		/// Convert Mapsui Color to Xamarin.Color
		/// </summary>
		/// <param name="color">Color in Mapsui format</param>
		/// <returns>Color in Xamarin.Forms format</returns>
		public static Xamarin.Forms.Color ToForms(this Mapsui.Styles.Color color)
		{
			return new Xamarin.Forms.Color(color.R / 255, color.G / 255, color.B / 255, color.A / 255);
		}

		/// <summary>
		/// Convert Xamarin.Forms to Mapsui Color
		/// </summary>
		/// <param name="color">Color in Xamarin.Forms format</param>
		/// <returns>Color in Mapsui format</returns>
		public static Mapsui.Styles.Color ToMapsui(this Xamarin.Forms.Color color)
		{
			return new Mapsui.Styles.Color((int) (color.R * 255), (int) (color.G * 255), (int) (color.B * 255), (int) (color.A * 255));
		}
	}
}