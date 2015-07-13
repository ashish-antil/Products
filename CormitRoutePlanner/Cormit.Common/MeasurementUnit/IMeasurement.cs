using System;

namespace Imarda.Lib
{
	public interface IMeasurement : IFormattable
	{
		Guid NameID { get; }
		string MetricSymbol { get; }
		Measurement AsMeasurement();
		string SpecificFormat(string format, IFormatProvider formatProvider);
		//EndOfBody
	}
}