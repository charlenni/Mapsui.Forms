using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Mapsui.Forms.Tests.Xamarin
{
	[TestFixture]
	public class PositionToString
	{
		[Test]
		public void DefaultString()
		{
			var position1 = new Position(30.12345, 120.12345);

			Assert.AreEqual("N 30° 07.407' E 120° 07.407'", position1.ToString());

			var position2 = new Position(-30.12345, -120.12345);

			Assert.AreEqual("S 30° 07.407' W 120° 07.407'", position2.ToString());
		}

		[Test]
		public void DecimalString()
		{
			var position1 = new Position(30.12345, 120.12345);

			Assert.AreEqual("N 30.1234° E 120.1235°", position1.ToString("P DD.dddd°|P DDD.dddd°|N|S|E|W"));

			var position2 = new Position(-30.12345, -120.12345);

			Assert.AreEqual("S 30.1234° W 120.1235°", position2.ToString("P DD.dddd°|P DDD.dddd°|N|S|E|W"));
		}

		[Test]
		public void DMSString()
		{
			var position1 = new Position(30.12345, 120.12345);

			Assert.AreEqual("N 30° 07' 24.4\" E 120° 07' 24.4\"", position1.ToString("P DD° MM' SS.s\"|P DD° MM' SS.s\"|N|S|E|W"));

			var position2 = new Position(-30.12345, -120.12345);

			Assert.AreEqual("S 30° 07' 24.4\" W 120° 07' 24.4\"", position2.ToString("P DD° MM' SS.s\"|P DD° MM' SS.s\"|N|S|E|W"));
		}
	}
}
