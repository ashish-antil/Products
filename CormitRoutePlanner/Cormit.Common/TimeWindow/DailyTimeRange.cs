using System;
using System.Runtime.Serialization;

namespace Imarda.Lib
{
	/// <summary>
	/// Stores a time range within a day. 
	/// </summary>
	[DataContract]
	public struct DailyTimeRange
	{
		[DataMember(Name = "After")] 
		private readonly int _AfterSeconds;

		[DataMember(Name = "Before")] 
		private readonly int _BeforeSeconds;

		/// <summary>
		/// Creates a DailyTimeRange; it defines a after and before time in seconds since midnight.
		/// If secAfter later than secBefore then the meaning of the range is inverted: the range is then from 00:00..before and after..24:00
		/// </summary>
		/// <param name="secAfter">after: seconds since midnight</param>
		/// <param name="secBefore">before: seconds since midnight</param>
		public DailyTimeRange(int secAfter, int secBefore)
		{
			_AfterSeconds = secAfter;
			_BeforeSeconds = secBefore;
		}

		/// <summary>
		/// Creates a DailyTimeRange; it defines a after and before time in seconds since midnight.
		/// If after later than before then the meaning of the range is inverted: the range is then from 00:00..before and after..24:00
		/// </summary>
		/// <param name="after">time since midnight, after time</param>
		/// <param name="before">time since midnight, before time</param>
		public DailyTimeRange(TimeSpan after, TimeSpan before)
		{
			_AfterSeconds = (int) after.TotalSeconds;
			_BeforeSeconds = (int) before.TotalSeconds;
		}

		public TimeSpan After
		{
			get { return TimeSpan.FromSeconds(_AfterSeconds); }
		}

		public TimeSpan Before
		{
			get { return TimeSpan.FromSeconds(_BeforeSeconds); }
		}

		/// <summary>
		/// Is the time in this range? Boundaries values are included in the range.
		/// </summary>
		/// <param name="t"></param>
		/// <returns></returns>
		public bool InRange(TimeSpan t)
		{
			var u = t.TotalSeconds;
			return _AfterSeconds <= _BeforeSeconds ? u >= _AfterSeconds && u <= _BeforeSeconds : u >= _AfterSeconds || u <= _BeforeSeconds;
		}


		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() != typeof (DailyTimeRange)) return false;
			var range = (DailyTimeRange) obj;
			return range._AfterSeconds == _AfterSeconds && range._BeforeSeconds == _BeforeSeconds;
		}

		public override int GetHashCode()
		{
			return _AfterSeconds ^ _BeforeSeconds;
		}


		public override string ToString()
		{
			return string.Format("DailyTimeRange({0}~{1})", After, Before);
		}
	}
}