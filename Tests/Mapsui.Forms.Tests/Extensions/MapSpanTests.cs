using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;
using Mapsui.Forms.Extensions;

namespace Mapsui.Forms.Tests.Extensions
{
    [TestFixture]
    public class MapSpanTests
    {
        [Test]
        public void MapSpanToBoundingBox()
        {
            var position1 = new MapSpan(new Position(30, 120), 1, 1).ToMapsui();

            Assert.AreEqual(3567983.5, position1.Top);
            Assert.AreEqual(13302679.0, position1.Left);
            Assert.AreEqual(3439440.0, position1.Bottom);
            Assert.AreEqual(13413999.0, position1.Right);
        }
    }
}
