using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	[DataContract]
	public struct LatLon
	{
		private decimal _Lat, _Lon;


		public LatLon(decimal lat, decimal lon)
		{
			_Lat = lat;
			_Lon = lon;
		}

		[DataMember]
		public decimal Lat
		{
			get { return _Lat; }
			set { _Lat = value; }
		}

		[DataMember]
		public decimal Lon
		{
			get { return _Lon; }
			set { _Lon = value; }
		}

		public bool IsValid()
		{
			return _Lat >= -90m && _Lat <= 90m && _Lon >= -180m && _Lon <= 180m;
		}

		/// <summary>
		/// For debugging only.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "(" + _Lat + ", " + _Lon + ")";
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			string lat = Angle.Degrees((double) _Lat).ToString(">00DN 00.000'", null);
			string lon = Angle.Degrees((double) _Lon).ToString(">000DE 00.000'", null);
			return lat + '|' + lon;
		}
	}
}