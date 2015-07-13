using System;

namespace Imarda.Lib
{
	/// <summary>
	/// Base class for date and event generators.
	/// </summary>
	public abstract class Generator
	{
		protected DateTime _End;
		protected int _Occurrences;
		protected DateTime _Start;


		public bool HasRange
		{
			get { return _Start != DateTime.MinValue && (_End != DateTime.MinValue || _Occurrences > 0); }
		}

		public DateTime Start
		{
			get { return _Start; }
		}

		public DateTime End
		{
			get { return _End; }
		}

		public int Occurrences
		{
			get { return _Occurrences; }
		}

		public void SetRange(DateTime start, DateTime end)
		{
			_Start = start.Date;
			_End = end.Date;
			_Occurrences = int.MaxValue;
			AfterSetRangeEnd();
		}

		protected virtual void AfterSetRangeEnd()
		{
		}

		public void SetRange(DateTime start, int occurrences)
		{
			_Start = start.Date;
			_End = DateTime.MaxValue;
			_Occurrences = occurrences;
			AfterSetRangeOcc();
		}

		protected virtual void AfterSetRangeOcc()
		{
		}
	}
}