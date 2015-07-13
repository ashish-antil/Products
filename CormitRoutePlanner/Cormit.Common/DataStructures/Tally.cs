using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Imarda.Lib
{
	/// <summary>
	/// Checks if a submitted string has been used before by remembering the checksum of that string.
	/// Used for error logging. Normalization: digits 0..9 get removed before calculating the checksum, so date/time,
	/// counts, Guids etc that are part of the error message don't make it a different error. 
	/// To ensure a Guid gets taken into the checksum, precede it by an asterisk: *9ee5f144-7e4a-49c9-9ee4-640bd8733bce.
	/// When a message starts with a '#' then the normalization does not happen and the checksum will
	/// be calculated over the entire message including the '#'. 
	/// The normalization can also be switched on/off for all messages by setting SuppressNormalization (default false).
	/// </summary>
	public class Tally
	{
		private static readonly Regex _NumbersAndGuids = new Regex(@"(\*?)([A-Fa-f0-9]{8}(?:-[A-Fa-f0-9]{4}){4}[A-Fa-f0-9]{8})\b|\d+", RegexOptions.Compiled);
		private Dictionary<uint, int> _Map;

		public Tally()
		{
			Reset();
		}

		public void Reset()
		{
			_Map = new Dictionary<uint, int>();
		}

		public bool SuppressNormalization { get; set; }

		/// <summary>
		/// Test a string against the filter map. 
		/// </summary>
		/// <param name="data">the string to be added</param>
		/// <param name="checksum">the calculated checksum</param>
		/// <param name="removed">how many digits were removed from the data before checksum applied</param>
		/// <returns>tally</returns>
		public int Add(string data, out uint checksum, out int removed)
		{
			removed = SuppressNormalization ? 0 : Normalize(ref data);
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			checksum = NumberUtils.ComputeCrc32(bytes);
			int tally;
			if (_Map.TryGetValue(checksum, out tally))
			{
				++tally;
			}
			else
			{
				tally = 1;
			}
			_Map[checksum] = tally;
			return tally;
		}

		/// <summary>
		/// Removes Guids and decimal digits
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		private int Normalize(ref string data)
		{
			if (data.StartsWith("#")) return 0;
			int n = data.Length;
			data = _NumbersAndGuids.Replace(data, (m => m.Groups[1].Value == "*" ? m.Groups[2].Value : string.Empty));
			return n - data.Length;
		}

	}
}
