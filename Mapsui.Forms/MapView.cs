using Mapsui.Forms.Extensions;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using System;
using System.ComponentModel;
using Xamarin.Forms.Maps;

namespace Mapsui.Forms
{
	/// <summary>
	/// This is the Mapsui Forms Control that will be used within the Forms PCL project
	/// </summary>
	public class MapView : View, INotifyPropertyChanged
	{
		/// <summary>
		/// Privates
		/// </summary>
		internal Map nativeMap;

		public MapView() : this(new MapSpan(new Position(41.890202, 12.492049), 0.1, 0.1))
		{
		}

		public MapView(MapSpan startPosition = null)
		{
			Map = new Map();

			if (startPosition != null)
			{
				UpdateVisibleRegion(startPosition);
			}
		}

		/// <summary>
		/// Events
		/// </summary>

		public new PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Properties
		/// </summary>

		public Map Map
		{
			get
			{
				return nativeMap;
			}
			set
			{
				if (nativeMap == value)
					return;

				nativeMap = value;

				// Add listener for Viewport events
				nativeMap.Viewport.ViewportChanged += ViewportPropertyChanged;

				// Get values
				//Center = nativeMap.Viewport.Center;
				// Set values
				UpdateVisibleRegion(LastMoveToRegion);
				nativeMap.BackColor = Color.Red.ToMapsuiColor(); // BackgroundColor.ToMapsuiColor();
			}
		}

		internal MapSpan LastMoveToRegion { get; private set; }

		public MapSpan VisibleRegion
		{
			get {
				if (nativeMap == null)
					return null;

				var leftBottom = Projection.SphericalMercator.ToLonLat(nativeMap.Viewport.Extent.BottomLeft.X, nativeMap.Viewport.Extent.BottomLeft.X);
				var rightTop = Projection.SphericalMercator.ToLonLat(nativeMap.Viewport.Extent.TopRight.X, nativeMap.Viewport.Extent.TopRight.X);
				var center = Projection.SphericalMercator.ToLonLat(nativeMap.Viewport.Center.X, nativeMap.Viewport.Center.Y);

				return new MapSpan(new Position(center.Y, center.X), Math.Abs(rightTop.Y - leftBottom.Y) / 2, Math.Abs(leftBottom.X - rightTop.X) / 2);
			}
		}

		public Position Center
		{
			get { return (Position)GetValue(CenterProperty); }
			private set { SetValue(CenterProperty, value); }
		}

		/// <summary>
		/// Bindings
		/// </summary>
		 
		public static readonly BindableProperty CenterProperty = BindableProperty.Create(
										propertyName: nameof(Center),
										returnType: typeof(Position),
										declaringType: typeof(MapView),
										defaultValue: default(Position),
										defaultBindingMode: BindingMode.OneWay,
										propertyChanged: null);

		/// <summary>
		/// Methods
		/// </summary>
		
		/// Change Viewport 
		public void MoveToRegion(MapSpan mapSpan)
		{
			if (mapSpan == null)
				throw new ArgumentNullException(nameof(mapSpan));
			LastMoveToRegion = mapSpan;
			UpdateVisibleRegion(mapSpan);
		}

		/// Change Viewport 
		public void MoveToCenter(Position pos)
		{
			if (pos == null)
				throw new ArgumentNullException(nameof(pos));
			var mapSpan = new MapSpan(pos, LastMoveToRegion.LatitudeDegrees, LastMoveToRegion.LongitudeDegrees);
			UpdateVisibleRegion(mapSpan);
		}

		/// <summary>
		/// Refresh the graphics of the map
		/// </summary>
		public void RefreshGraphics()
		{
			MessagingCenter.Send<MapView>(this, "Refresh");
		}

		/// <summary>
		/// Check if something important for Map changed
		/// </summary>
		/// <param name="propertyName">Name of property which changed</param>
		protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			// Set new BackgroundColor to nativeMap
			if (propertyName.Equals(nameof(BackgroundColor)))
			{
				nativeMap.BackColor = BackgroundColor.ToMapsuiColor();
			}

			if (propertyName.Equals(nameof(Center)))
			{
				// Center changed via property
				return;
			}

			RaisePropertyChanged(propertyName);
		}

		/// <summary>
		/// Get updates from Map
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void MapPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			RaisePropertyChanged(e.PropertyName);
		}

		/// <summary>
		/// Get updates from Viewport
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ViewportPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Only check, if the Viewport is correct
			if (nativeMap.Viewport.Width == 0 || nativeMap.Viewport.Height == 0)
				return;

			// Did Center changed?
			if (e.PropertyName.Equals("Center"))
			{
				var centerPoint = Projection.SphericalMercator.ToLonLat(nativeMap.Viewport.Center.X, nativeMap.Viewport.Center.Y);
				var centerPosition = new Position(centerPoint.Y, centerPoint.X);

				if (Center.Equals(centerPosition))
					return;

				Center = centerPosition;

				// We don't need to resend event again
				return;
			}

			// Did Viewport dimensions changed
			if (e.PropertyName.Equals("Width") | e.PropertyName.Equals("Height"))
			{
				// Viewport changed size, so recalculate VisibleRegion
				UpdateVisibleRegion(LastMoveToRegion);
			}

			RaisePropertyChanged(e.PropertyName);
		}

		/// <summary>
		/// Raise event for PropertyChanged of MapView
		/// </summary>
		/// <param name="propertyName"></param>
		void RaisePropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		void UpdateVisibleRegion(MapSpan newMapSpan)
		{
			if (newMapSpan == null || VisibleRegion.Equals(newMapSpan) || nativeMap == null)
				return;

			LastMoveToRegion = newMapSpan;

			var top = newMapSpan.Center.Latitude + newMapSpan.LatitudeDegrees;
			var bottom = newMapSpan.Center.Latitude - newMapSpan.LatitudeDegrees;
			var left = newMapSpan.Center.Longitude - newMapSpan.LongitudeDegrees;
			var right = newMapSpan.Center.Longitude + newMapSpan.LongitudeDegrees;

			var leftBottom = Projection.SphericalMercator.FromLonLat(left, bottom);
			var rightTop = Projection.SphericalMercator.FromLonLat(right, top);

			nativeMap.NavigateTo(new Geometries.BoundingBox(leftBottom, rightTop));
		}
	}
}