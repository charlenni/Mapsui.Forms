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
		internal Map map;

		public MapView() : this(new MapSpan(new Position(51.4813, -0.00405), 0.01, 0.01))
		{
		}

		public MapView(MapSpan startPosition = null)
		{
			Map = new Map();

			if (startPosition != null)
			{
                LastMoveToRegion = startPosition;
                map.Viewport.Center = startPosition.Center.ToMapsui();
                map.Viewport.Resolution = 50;
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
				return map;
			}
			set
			{
				if (map == value)
					return;

				if (map != null && map.Viewport != null)
				{
					// Remove listener for Viewport events
					map.Viewport.ViewportChanged -= ViewportPropertyChanged;
				}

				map = value;

				// Add listener for Viewport events
				map.Viewport.ViewportChanged += ViewportPropertyChanged;

				// Get values
				Center = map.Viewport.Center.ToForms();
				// Set values
				map.BackColor = BackgroundColor.ToMapsui();
			}
		}

		public MapSpan LastMoveToRegion { get; private set; }

		public MapSpan VisibleRegion
		{
			get {
				if (map == null)
					return null;

                return map.Viewport.Extent.ToForms();
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
            LastMoveToRegion = mapSpan ?? throw new ArgumentNullException(nameof(mapSpan));
			map.NavigateTo(LastMoveToRegion.ToMapsui());
		}

		/// Change Viewport 
		public void MoveToCenter(Position pos)
		{
			if (pos == null)
				throw new ArgumentNullException(nameof(pos));
			LastMoveToRegion = new MapSpan(pos, LastMoveToRegion.LatitudeDegrees, LastMoveToRegion.LongitudeDegrees);
            map.NavigateTo(LastMoveToRegion.ToMapsui());
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
				map.BackColor = BackgroundColor.ToMapsui();
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

        bool init = true;

		/// <summary>
		/// Get updates from Viewport
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ViewportPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Only check, if the Viewport is correct
			if (map.Viewport.Width == 0 || map.Viewport.Height == 0)
				return;

			// Did Center changed?
			if (e.PropertyName.Equals("Center"))
			{
                var centerPosition = map.Viewport.Center.ToForms();

				if (Center.Equals(centerPosition))
					return;

				Center = centerPosition;
                LastMoveToRegion = new MapSpan(Center, LastMoveToRegion.LatitudeDegrees, LastMoveToRegion.LongitudeDegrees);

				// We don't need to resend event again
				return;
			}

			// Did Viewport dimensions changed
			if (e.PropertyName.Equals("Width") | e.PropertyName.Equals("Height"))
			{
                // Do this the first time after Viewport has correct size
                if (init)
                {
                    map.NavigateTo(LastMoveToRegion.ToMapsui());
                    init = false;
                }

                // Raise event, that VisibleRegion has changed
                RaisePropertyChanged(nameof(VisibleRegion));

                return;
			}

            // Did Viewport resolution changed
            if (e.PropertyName.Equals("Resolution"))
            {
                var bottomLeft = map.Viewport.Extent.BottomLeft.ToForms();
                var topRight = map.Viewport.Extent.TopRight.ToForms();

                LastMoveToRegion = new MapSpan(Center, Math.Abs(bottomLeft.Latitude - topRight.Latitude), Math.Abs(bottomLeft.Longitude - topRight.Longitude));
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
	}
}