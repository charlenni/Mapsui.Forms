using NUnit.Framework;
using Mapsui.Forms.Converter;
using Xamarin.Forms.Maps;
using System.Globalization;
using System;

namespace Mapsui.Forms.Tests.Converter
{
	[TestFixture]
	public class ConverterPosition
	{
		Position positionNE = new Position(23.12345, 3.12345);

		[Test]
		public void ConvertToString()
		{
			var converter = new Forms.Converter.ConverterPosition();

			var result = converter.Convert(positionNE, typeof(string), "P DD° MM.mmm'|P DDD° MM.mmm'", CultureInfo.InvariantCulture);
			var should = string.Format("N {0:00}° {1:00.000}' E {2:000}° {3:00.000}'", Math.Floor(positionNE.Latitude), (positionNE.Latitude - Math.Floor(positionNE.Latitude)) * 60.0, Math.Floor(positionNE.Longitude), (positionNE.Longitude - Math.Floor(positionNE.Longitude)) * 60.0);

			Assert.AreEqual(should, result);
		}

		[Test]
		public void TestMethod()
		{
			// TODO: Add your test code here
			Assert.Pass("Your first passing test");
		}
	}
}
