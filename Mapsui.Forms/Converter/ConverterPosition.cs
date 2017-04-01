using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Mapsui.Forms.Converter
{
	public class ConverterPosition : IValueConverter
	{
		struct Details
		{
			public string Format;
			public string FormatLat;
			public string FormatLon;
			public string North;
			public string East;
			public string South;
			public string West;
		}

		static Details FormatDetails = new Details();

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Position pos = (Position)value;

			// If value isn't a Position than return an empty string
			if (pos == null || parameter == null)
			{
				return string.Empty;
			}

			if (parameter is string && FormatDetails.Format != (string)parameter)
			{
				// Calc new format strings

				// Set default format for Position
				var formats = new string[] { "P DD° MM.mmm'", "P DDD° MM.mmm", "N", "E", "S", "W" };

				// If there is a string for paramter, than use this string as format
				if (parameter is string)
				{
					var formatParts = ((string)parameter).Split('|');

					for (int i = 0; i < Math.Min(formatParts.Length, 6); i++)
						formats[i] = formatParts[i];
				}

				FormatDetails.Format = (string)parameter;
				FormatDetails.FormatLat = formats[0];
				FormatDetails.FormatLon = formats[1];
				FormatDetails.North = formats[2];
				FormatDetails.East = formats[3];
				FormatDetails.South = formats[4];
				FormatDetails.West = formats[5];
			}

			var result = new StringBuilder();

			result.Append(ConvertNumber(pos.Latitude, FormatDetails.FormatLat, FormatDetails.North, FormatDetails.South));
			result.Append(" ");
			result.Append(ConvertNumber(pos.Longitude, FormatDetails.FormatLon, FormatDetails.East, FormatDetails.West));

			return result.ToString();
		}

		private string ConvertNumber(double number, string format, string textPos, string textNeg)
		{
			var result = format;

			result = number > 0 ? result.Replace("P", textPos) : result.Replace("P", textNeg);

			var textD = Regex.Matches(format, "([D]+)");
			var textd = Regex.Matches(format, "([d]+)");
			var textM = Regex.Matches(format, "([M]+)");
			var textm = Regex.Matches(format, "([m]+)");
			var textS = Regex.Matches(format, "([S]+)");
			var texts = Regex.Matches(format, "([s]+)");

			var textD1 = Regex.Matches(format, @"([Dd\.]+)");
			var textM1 = Regex.Matches(format, @"([M\.m]+)");
			var textS1 = Regex.Matches(format, @"([S\.s]+)");

			var degrees = number;
			var minutes = (number - Math.Floor(number)) * 60.0;
			var seconds = (minutes - Math.Floor(minutes)) * 60.0;

			var decDegrees = number - degrees;
			var fullMinutes = Math.Floor(minutes);
			var decMinutes = minutes - fullMinutes;
			var fullSeconds = Math.Floor(seconds);
			var decSeconds = seconds - fullSeconds;

			var temp1 = textD[0].ToString();

			result = textD.Count > 0 ? result.Replace(textD[0].ToString(), string.Format("{0:" + new string('0', textD[0].ToString().Length) + "}", degrees)) : result;
			result = textd.Count > 0 ? result.Replace(textd[0].ToString(), string.Format("{0:" + new string('0', textd[0].ToString().Length) + "}", decDegrees * Math.Pow(10, textd[0].ToString().Length))) : result;
			result = textM.Count > 0 ? result.Replace(textM[0].ToString(), string.Format("{0:" + new string('0', textM[0].ToString().Length) + "}", fullMinutes)) : result;
			result = textm.Count > 0 ? result.Replace(textm[0].ToString(), string.Format("{0:" + new string('0', textm[0].ToString().Length) + "}", decMinutes * Math.Pow(10, textm[0].ToString().Length))) : result;
			result = textS.Count > 0 ? result.Replace(textS[0].ToString(), string.Format("{0:" + new string('0', textS[0].ToString().Length) + "}", fullSeconds)) : result;
			result = texts.Count > 0 ? result.Replace(texts[0].ToString(), string.Format("{0:" + new string('0', texts[0].ToString().Length) + "}", decSeconds * Math.Pow(10, texts[0].ToString().Length))) : result;

			result = result.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
