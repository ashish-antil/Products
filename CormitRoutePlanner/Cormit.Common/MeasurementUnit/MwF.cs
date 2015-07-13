namespace Imarda.Lib
{
	/// <summary>
	/// Object stores a measurement with a format.
	/// </summary>
	public class MwF
	{
		/// <summary>
		/// Parse the pstring and store along with the fmt. No compatibility checks are done.
		/// </summary>
		/// <param name="pstring"></param>
		/// <param name="fmt"></param>
		public MwF(string pstring, string fmt)
		{
			Value = Measurement.Parse(pstring);
			Format = fmt;
		}

		/// <summary>
		/// Store the given measurement with the format. No checks are done.
		/// </summary>
		/// <param name="m"></param>
		/// <param name="fmt"></param>
		public MwF(IMeasurement m, string fmt)
		{
			Value = m;
			Format = fmt;
		}

		private IMeasurement Value { get; set; }
		private string Format { get; set; }


		public override bool Equals(object obj)
		{
			var mwf = obj as MwF;
			return mwf != null && mwf.Value == Value && mwf.Format == Format;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode() ^ Format.GetHashCode();
		}

		/// <summary>
		/// Return an FString format, e.g. "Volume:10 ~bottle", "Length:5.4 km".
		/// First space separates the PString (parseable measurement string) from the formatting info.
		/// </summary>
		/// <returns>FString</returns>
		public override string ToString()
		{
			return Measurement.FString(Value, Format);
		}

		/// <summary>
		/// Format the value using the stored format information.
		/// </summary>
		/// <param name="mfi">contains context information required for formatting</param>
		/// <returns></returns>
		public string ToString(MeasurementFormatInfo mfi)
		{
			return Measurement.Format(ToString(), mfi);
		}
	}
}