namespace Mapsui.Forms.Converter
{
	using System;

	/// <summary>
	/// Converter from length in meters to a string.
	/// </summary>
	public static class Converter
	{
		#region Length

		///// <summary>
		///// Convert a number into a string according to the preferences.
		///// </summary>
		///// <returns>The length as string with unit.</returns>
		///// <param name="length">Length in meters.</param>
		///// <param name="format">Format of string.</param>
		//public static string NumberToLength(double length, string format = "0.0")
		//{
		//	string unit = "m";

		//	switch (Settings.UnitLength)
		//	{
		//		case UnitLength.Feet:
		//			unit = "ft";
		//			break;
		//		case UnitLength.NauticalMiles:
		//			unit = "nm";
		//			break;
		//	}

		//	return string.Format("{0:" + format + "} {1}", NumberToConvertedLength(length), unit);
		//}

		///// <summary>
		///// Convert a number into a string according to the preferences.
		///// </summary>
		///// <remarks>
		///// The returned string has never more than 4 digits.
		///// </remarks>
		///// <returns>Length as string with unit.</returns>
		///// <param name="length">Length in meters.</param>
		//public static string NumberToBestLength(double length)
		//{
		//	double lengthInUnits = NumberToConvertedLength(length);
		//	string result = string.Empty;

		//	switch (Settings.UnitLength)
		//	{
		//		case UnitLength.Meter:
		//			if (length < 1000.0)
		//			{
		//				result = string.Format("{0:0} m", NumberToConvertedLength(length));
		//			}
		//			else if (length < 10000.0)
		//			{
		//				result = string.Format("{0:0.00} km", NumberToConvertedLength(length) / 1000.0);
		//			}
		//			else if (length < 100000.0)
		//			{
		//				result = string.Format("{0:0.0} km", NumberToConvertedLength(length) / 1000.0);
		//			}
		//			else
		//			{
		//				result = string.Format("{0:0} km", NumberToConvertedLength(length) / 1000.0);
		//			}

		//			break;
		//		case UnitLength.Feet:
		//			if (lengthInUnits < 1000.0)
		//			{
		//				result = string.Format("{0:0} ft", lengthInUnits);
		//			}
		//			else if (lengthInUnits < 5280.0)
		//			{
		//				result = string.Format("{0:0} ft", lengthInUnits);
		//			}
		//			else if (lengthInUnits < 52800.0)
		//			{
		//				result = string.Format("{0:0.00} mi", lengthInUnits / 5280.0);
		//			}
		//			else if (lengthInUnits < 528000.0)
		//			{
		//				result = string.Format("{0:0.0} mi", lengthInUnits / 5280.0);
		//			}
		//			else
		//			{
		//				result = string.Format("{0:0} mi", lengthInUnits / 5280.0);
		//			}

		//			break;
		//		case UnitLength.NauticalMiles:
		//			if (lengthInUnits < 10.0)
		//			{
		//				result = string.Format("{0:0.0} nm", lengthInUnits);
		//			}
		//			else if (lengthInUnits < 100.0)
		//			{
		//				result = string.Format("{0:0.0} nm", lengthInUnits);
		//			}
		//			else if (lengthInUnits < 1000.0)
		//			{
		//				result = string.Format("{0:0.0} nm", lengthInUnits);
		//			}
		//			else
		//			{
		//				result = string.Format("{0:0} nm", lengthInUnits);
		//			}

		//			break;
		//	}

		//	return result;
		//}

		#endregion

		#region Coordinates

		/// <summary>
		/// Convert a latitude to a valid string according to the preferences.
		/// </summary>
		/// <returns>The latitude as string.</returns>
		/// <param name="lat">Latitude in degrees.</param>
		public static string NumberToLatitude(double lat)
		{
			return (lat >= 0.0 ? "N" : "S") + " " + NumberToCoordinat(Math.Abs(lat), "00");
		}

		/// <summary>
		/// Convert a longitude to a valid string according to the preferences.
		/// </summary>
		/// <returns>The longitude as string.</returns>
		/// <param name="lon">Longitude in degrees.</param>
		public static string NumberToLongitude(double lon)
		{
			return (lon >= 0.0 ? "E" : "W") + " " + NumberToCoordinat(Math.Abs(lon), "000");
		}

		#endregion

		#region Private Functions

		///// <summary>
		///// Convert a number into a double according to the preferences.
		///// </summary>
		///// <remarks>
		///// The length is always in meter.
		///// </remarks>
		///// <returns>The length as double for preferenced format.</returns>
		///// <param name="length">Length in meter.</param>
		//private static double NumberToConvertedLength(double length)
		//{
		//	double value;

		//	switch (Settings.UnitLength) 
		//	{
		//		case UnitLength.Feet:
		//			value = length * 3.2808399;
		//			break;
		//		case UnitLength.NauticalMiles:
		//			value = length * 0.000539956803;
		//			break;
		//		case UnitLength.Meter:
		//		default:
		//			value = length;
		//			break;
		//	}

		//	return value;
		//}

		/// <summary>
		/// Convert the number part of a coordinate to a string according to the preferences.
		/// </summary>
		/// <returns>The to coordinat.</returns>
		/// <param name="coord">Coordinate in degrees.</param>
		/// <param name="format">Format of the degrees part of the string.</param>
		private static string NumberToCoordinat(double coord, string format)
		{
			int intDeg, intMin;
			double doubleMin, doubleSec;
			string result = string.Empty;

			//switch (Settings.FormatCoordinates)
			//{
			//	case FormatCoordinates.Decimal:
			//		result = string.Format("{0:" + format + ".00000}°", coord);
			//		break;
			//	case FormatCoordinates.DecimalMinutesSeconds:
			//		intDeg = (int)coord;
			//		doubleMin = (coord - (double)intDeg) * 60.0;
			//		intMin = (int)doubleMin;
			//		doubleSec = (doubleMin - (double)intMin) * 60.0;
			//		result = string.Format("{0:" + format + "}° {1:00}' {2:00.00}\"", intDeg, intMin, doubleSec);
			//		break;
			//	case FormatCoordinates.DecimalMinutes:
			//	default:
					intDeg = (int)coord;
					doubleMin = (coord - (double)intDeg) * 60.0;
					result = string.Format("{0:" + format + "}° {1:00.000}'", intDeg, doubleMin);
					//break;
			//}

			return result;
		}

		#endregion
	}
}
