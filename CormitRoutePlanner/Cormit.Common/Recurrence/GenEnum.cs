using System;
using System.Collections.Generic;

namespace Imarda.Lib
{
	/// <summary>
	/// Iterates over dates that were passed into the constructor.
	/// </summary>
	public class EnumDateGenerator : DateGenerator
	{
		private readonly List<DateTime> _dates;
		private int _index;

		public EnumDateGenerator(IEnumerable<DateTime> dates)
		{
			_dates = new List<DateTime>(dates);
			_dates.Sort();
		}

		public DateTime[] Dates
		{
			get { return _dates.ToArray(); }
		}

		protected override DateTime First(DateTime date)
		{
			for (_index = 0; _index < _dates.Count; _index++)
			{
				DateTime dt = _dates[_index];
				if (dt >= date) return dt;
			}
			return DateTime.MaxValue;
		}

		protected override DateTime Next(DateTime date)
		{
			while (++_index < _dates.Count)
			{
				DateTime dt = _dates[_index];
				if (dt >= date) return dt;
			}
			return DateTime.MaxValue;
		}
	}
}