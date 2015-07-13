using System;

namespace Imarda.Lib
{
	/// <summary>
	/// Interface for events. An event is a name with a date. It is a level on top of
	/// DateTime.
	/// Typically used to represent events in classes such as "EventCalendar" or "TaskManager"
	/// </summary>
	public interface ICalEvent : IComparable<ICalEvent>
	{
		DateTime Date { get; set; }
		string Name { get; set; }
	}
}