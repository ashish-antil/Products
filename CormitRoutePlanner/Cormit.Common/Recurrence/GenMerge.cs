using System;
using System.Collections.Generic;
using System.Linq;

namespace Imarda.Lib
{
	/// <summary>
	/// This DateGenerator contains a list of DateGenerators and delivers the next DateTime
	/// by peforming a merge-sort without duplicates on the DateTime values produced by all the
	/// contained DateGenerators. This can be used to model more complex DateTime generators.
	/// </summary>
	public class MultiplexGenerator : DateGenerator
	{
		/// <summary>
		/// The Enumerators of the DateGenerators.
		/// </summary>
		private readonly IEnumerator<DateTime>[] _enumerators;

		/// <summary>
		/// List of generators that produce DateTime objects to be merged into a sequence.
		/// </summary>
		/// <remarks>
		/// Array is kept here for administrative purpose, the algorithm actually works
		/// on the _enumerators.
		/// </remarks>
		private readonly DateGenerator[] _generators;

		/// <summary>
		/// _hasMore[i] is true if _enumerators[i] still has DateTime values to deliver
		/// and becomes false after _enumerators[i] has delivered its last DateTime
		/// </summary>
		private readonly bool[] _hasMore;

		/// <summary>
		/// Create a DateGenerator based on the list of DateGenerators.
		/// </summary>
		/// <param name="array"></param>
		public MultiplexGenerator(DateGenerator[] array)
		{
			_generators = array;
			_enumerators = new IEnumerator<DateTime>[array.Length];
			_hasMore = new bool[array.Length];
		}

		public DateGenerator[] Generators
		{
			get { return _generators; }
		}

		protected override DateTime First(DateTime date)
		{
			date = date.AddDays(-1);
			for (int i = 0; i < _generators.Length; i++)
			{
				_enumerators[i] = _generators[i].GetEnumerator(_Start, false);
				_hasMore[i] = _enumerators[i].MoveNext();
			}
			return Next(date);
		}

		protected override void AfterSetRangeEnd()
		{
			foreach (DateGenerator gen in _generators.Where(gen => !gen.HasRange))
			{
				gen.SetRange(Start, End);
			}
		}

		protected override void AfterSetRangeOcc()
		{
			foreach (DateGenerator gen in _generators.Where(gen => !gen.HasRange))
			{
				gen.SetRange(Start, Occurrences);
			}
		}

		/// <summary>
		/// Here is where the magic happens: iterate over the enumerators and determine
		/// the next date to be delivered.
		/// </summary>
		/// <param name="date">earliest allowed date</param>
		/// <returns></returns>
		protected override DateTime Next(DateTime date)
		{
			int indexOfNextIterator = 0;
			DateTime nextDate = DateTime.MaxValue;
			for (int i = 0; i < _enumerators.Length; i++)
			{
				if (Progress(i, date))
				{
					IEnumerator<DateTime> iter = _enumerators[i];
					if (iter.Current < nextDate)
					{
						nextDate = iter.Current;
						indexOfNextIterator = i;
					}
				}
			}
			IEnumerator<DateTime> iterDue = _enumerators[indexOfNextIterator];
			_hasMore[indexOfNextIterator] = iterDue.MoveNext();
			return nextDate;
		}

		/// <summary>
		/// Run the iterator with the given index to a date 
		/// greater than the given date.
		/// </summary>
		/// <param name="i">identifies iterator</param>
		/// <param name="date"></param>
		/// <returns></returns>
		private bool Progress(int i, DateTime date)
		{
			IEnumerator<DateTime> iter = _enumerators[i];
			bool hasMore = _hasMore[i];
			while (hasMore && iter.Current <= date)
			{
				hasMore = iter.MoveNext();
			}
			_hasMore[i] = hasMore;
			return hasMore;
		}
	}
}