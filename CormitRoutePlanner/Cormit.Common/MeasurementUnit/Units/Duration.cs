using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Imarda.Lib
{
	[DataContract]
	public struct Duration : IMeasurement
	{
		public const double SecondsPerMinute = 60.0;
		public const double SecondsPerHour = SecondsPerMinute*60.0;
		public const double SecondsPerDay = SecondsPerHour*24.0;

		public static readonly Duration Zero = new Duration(0.0);
		public static readonly MUnit Unit = new MUnit(0, 0, 1, 0, 0, 0, 0);
		private static readonly Regex _RxDays = new Regex(@"((?:0|#)+)(.*)", RegexOptions.Compiled);

		[DataMember] private double _SI;

		public Duration(double seconds)
		{
			_SI = seconds;
		}

		public Duration(int days, int hours, int minutes, int seconds)
		{
			_SI = new TimeSpan(days, hours, minutes, seconds).TotalSeconds;
		}

		[Unit("s")]
		public double InSeconds
		{
			get { return _SI; }
		}

		[Unit("h")]
		public double InHours
		{
			get { return _SI/SecondsPerHour; }
		}

		[Unit("min")]
		public double InMinutes
		{
			get { return _SI/SecondsPerMinute; }
		}

		[Unit("d")]
		public double InDays
		{
			get { return _SI/SecondsPerDay; }
		}

		#region IMeasurement Members

		public Guid NameID
		{
			get { return new Guid("0f983ae4-91b7-4f16-bf58-0388a3c5dc14"); }
		}

		public string MetricSymbol
		{
			get { return "s"; }
		}


		public string ToString(string format, IFormatProvider formatProvider)
		{
			return Measurement.DefaultFormatHandler(format, this, formatProvider);
		}

		/// <summary>
		/// Format a duration, using special format
		/// </summary>
		/// <remars>
		/// string[] exampleFormats = {
		///  "00d ;H:m:ss", 
		///  "#.;H:m:s",
		///  "# days ;H'h'm'm'",
		///  "0 days ;HH 'hrs' mm 'm' ss 's'",
		///  "H:m:s", // time-only format, do not use ';'
		///  "0 d;", // day-only format, terminate with ';'
		///  "# d;",
		///  "^# d;" // round up the day if hours >= 12, only applies to day-only format
		/// };
		/// </remars>
		/// <param name="format">e.g. "0d ;h:mm:ss"</param>
		/// <param name="formatProvider"></param>
		/// <returns></returns>
		public string SpecificFormat(string format, IFormatProvider formatProvider)
		{
			if (format == "") throw new FormatException("empty string not valid for Duration specific format");
			bool roundDayUp = format.StartsWith("^");
			if (roundDayUp) format = format.Substring(1);
			TimeSpan ts = TimeSpan.FromSeconds(_SI);
			string[] parts = format.Split(';');
			string days;
			string timeFmt;
			if (parts.Length > 1)
			{
				timeFmt = parts[1];
				if (timeFmt.Trim() == "")
				{
					// no day part
					int d = ts.Days;
					if (roundDayUp && ts.Hours >= 12) d++;
					return FormatDayPart(d, parts[0]); // return day part only
				}
				else
				{
					days = FormatDayPart(ts.Days, parts[0]);
				}
			}
			else
			{
				days = string.Empty;
				timeFmt = parts[0];
			}
			// use DateTime just to format the time part, only use time formatting chars, not date formatting!
			bool neg;
			if (ts.TotalSeconds < 0.0)
			{
				ts = ts.Negate();
				neg = true;
			}
			else neg = false;
			string time = new DateTime(1, 1, 1, ts.Hours, ts.Minutes, ts.Seconds).ToString(timeFmt);
			string s = days + time;
			if (neg && days == string.Empty) s = "-" + s;
			return s;
		}


		public Measurement AsMeasurement()
		{
			return this;
		}

		#endregion

		[Parse("s")]
		public static Duration Seconds(double seconds)
		{
			return new Duration(seconds);
		}

		[Parse("min")]
		public static Duration Minutes(double minutes)
		{
			return new Duration(minutes*SecondsPerMinute);
		}

		[Parse("h")]
		public static Duration Hours(double hours)
		{
			return new Duration(hours*SecondsPerHour);
		}

		[Parse("d")]
		public static Duration Days(double days)
		{
			return new Duration(days*SecondsPerDay);
		}

		public static Duration operator +(Duration n1, Duration n2)
		{
			return new Duration(n1._SI + n2._SI);
		}

		public static Duration operator -(Duration n1, Duration n2)
		{
			return new Duration(n1._SI - n2._SI);
		}

		public static implicit operator Measurement(Duration x)
		{
			return new Measurement(x._SI, Unit);
		}

		public static implicit operator Duration(Measurement x)
		{
			if (x.Unit != Unit) throw new ArgumentException();
			return new Duration(x.Value);
		}

		public static implicit operator TimeSpan(Duration x)
		{
			return TimeSpan.FromSeconds(x._SI);
		}

		public static implicit operator Duration(TimeSpan x)
		{
			return new Duration(x.TotalSeconds);
		}

		private static string FormatDayPart(int days, string dayfmt)
		{
			Match m = _RxDays.Match(dayfmt);
			string s = days.ToString(m.Groups[1].Value);
			if (s.Trim() != "") s += m.Groups[2].Value;
			return s;
		}

		public Duration Add(TimeSpan ts)
		{
			_SI += ts.TotalSeconds;
			return this;
		}

		public Duration Subtract(TimeSpan ts)
		{
			_SI -= ts.TotalSeconds;
			return this;
		}


		
		public static bool operator >(Duration x1, Duration x2)
		{
			return x1._SI > x2._SI;
		}

		public static bool operator <(Duration x1, Duration x2)
		{
			return x1._SI < x2._SI;
		}

		public static bool operator >=(Duration x1, Duration x2)
		{
			return x1._SI >= x2._SI;
		}

		public static bool operator <=(Duration x1, Duration x2)
		{
			return x1._SI <= x2._SI;
		}


		public static bool operator ==(Duration x1, Duration x2)
		{
			return x1._SI == x2._SI;
		}

		public static bool operator !=(Duration x1, Duration x2)
		{
			return x1._SI != x2._SI;
		}

		public override int GetHashCode()
		{
			return _SI.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != typeof(Duration)) return false;
			return ((Duration)obj)._SI == _SI;
		}

		//EndOfBody Duration


	}
}