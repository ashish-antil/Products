using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Imarda.Lib
{
	public static class GeoUtils
	{
		public const double EarthRadius = 6371009; // For Earth, the mean radius is 6,371.009 km (?3,958.761 mi; ?3,440.069 nmi).
		public const double MetresPerDegreeLatitude = Math.PI * EarthRadius / 180.0; // ?111,195 m/deg

		/// <summary>
		/// Calculate bounding box, given lat & lon of centre and a radius
		/// </summary>
		/// <param name="centreLat">centre latitude (degrees)</param>
		/// <param name="centreLon">centre longitude (degrees)</param>
		/// <param name="radiusMetres"></param>
		/// <param name="lat1">latitude (degrees) nearest to (0,0)</param>
		/// <param name="lon1">longitude (degrees) nearest to (0,0)</param>
		/// <param name="lat2">latitude (degrees) furthest from (0,0)</param>
		/// <param name="lon2">longitude (degrees) furthest from (0,0)</param>
		public static void CalculateBoundingBox(double centreLat, double centreLon, int radiusMetres,
		                                        out double lat1, out double lon1, out double lat2, out double lon2)
		{
			double deltaLat = radiusMetres / 1852.0 / 60.0;
			lat1 = centreLat - deltaLat;
			lat2 = centreLat + deltaLat;
			double deltaLon = deltaLat / Math.Cos(ToRad(centreLat));
			lon1 = centreLon - deltaLon;
			lon2 = centreLon + deltaLon;
		}

		/// <summary>
		/// Pass in any two latitudes and return them in the order that lat1 is the most north and lat2 is the one most south
		/// </summary>
		/// <param name="lat1"></param>
		/// <param name="lat2"></param>
		public static void EnsureLatitudeNorthSouthOrder(ref double lat1, ref double lat2)
		{
			if (lat1 < lat2)
			{
				double h = lat2;
				lat2 = lat1;
				lat1 = h;
			}
		}

		/// <summary>
		/// Get the degree equivalent of the metres latitude direction
		/// </summary>
		/// <param name="metres">metres in north/south direction</param>
		/// <returns>non-negative number degrees</returns>
		public static double ToLatDeg(double metres)
		{
			return Math.Abs(metres / MetresPerDegreeLatitude);
		}

		/// <summary>
		/// Get the degree equivalent of the metres in longitude direction, measured at the given latitude
		/// </summary>
		/// <param name="latitudeInDeg">latitude where we are measuring</param>
		/// <param name="metres">metres in east/west direction </param>
		/// <returns>non-negative number degrees</returns>
		public static double ToLonDeg(double latitudeInDeg, double metres)
		{
			return Math.Abs(ToLatDeg(metres) / Math.Cos(latitudeInDeg));
		}

	    /// <summary>
	    ///     Calculate greatcircle distance between two points
	    /// </summary>
	    /// <param name="lat1rad">latitude point 1, in radians</param>
	    /// <param name="lon1rad">longitude point 1, in radians</param>
	    /// <param name="lat2rad">latitude point 2, in radians</param>
	    /// <param name="lon2rad">longitude point 2, in radians</param>
	    /// <returns>metres</returns>
	    public static double Distance(double lat1rad, double lon1rad, double lat2rad, double lon2rad)
		{
			double dLat = lat2rad - lat1rad;
			double dLon = lon2rad - lon1rad;
			double hLat = Math.Sin(dLat / 2);
			double hLon = Math.Sin(dLon / 2);
			double a = hLat * hLat + Math.Cos(lat1rad) * Math.Cos(lat2rad) * hLon * hLon;
			double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
			double d = EarthRadius * c;
			return d;
		}

		public static double DistanceDeg(double lat1deg, double lon1deg, double lat2deg, double lon2deg)
		{
			return Distance(ToRad(lat1deg), ToRad(lon1deg), ToRad(lat2deg), ToRad(lon2deg));
		}

		public static double ToRad(double deg)
		{
			return deg / 180.0 * Math.PI;
		}

		/// <summary>
		/// Calculate direction from point 1 to point 2.
		/// </summary>
		/// <param name="lat1">latitude (radians) point 1</param>
		/// <param name="lon1">longitude (radians) point 1</param>
		/// <param name="lat2">latitude (radians) point 2</param>
		/// <param name="lon2">longitude (radians) point 2</param>
		/// <returns></returns>
		public static double Bearing(double lat1, double lon1, double lat2, double lon2)
		{
			var diff = lon2 - lon1;
			var y = Math.Sin(diff) * Math.Cos(lat2);
			var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(diff);
			var a = Math.Atan2(y, x);
			return a;
		}
	}
}
