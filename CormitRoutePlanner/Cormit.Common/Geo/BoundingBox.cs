using System;

namespace Imarda.Lib
{
	public struct BoundingBox
	{
		public double Lat1;
		public double Lon1;
		public double Lat2;
		public double Lon2;

		public BoundingBox(double latdeg, double londeg, int radiusMetres)
		{
			double deltaLat = radiusMetres / 1852.0 / 60.0;
			double deltaLon = deltaLat / Math.Cos(latdeg / 180.0 * Math.PI);
			Lat1 = latdeg - deltaLat;
			Lat2 = latdeg + deltaLat;
			Lon1 = londeg - deltaLon;
			Lon2 = londeg + deltaLon;
		}

		public bool IsInside(double lat, double lon)
		{
			return lat >= Lat1 && lat <= Lat2 && lon >= Lon1 && lon <= Lon2;
		}

		public void EnsureLatitudeNorthSouthOrder()
		{
			if (Lat1 < Lat2)
			{
				double h = Lat2;
				Lat2 = Lat1;
				Lat1 = h;
			}
		}
	}
}
