using System;
using System.Collections.Generic;
using System.Text;

namespace Imarda.Lib
{
	public static class SequentialGuid
	{
		// Note: .NET Guid byte order is 03020100-0504-0706-0809-0A0B0C0D0E0F

		/// <summary>
		/// COMB-Guid for much better performance in database tables. Better than UuidCreateSequential.
		/// http://www.informit.com/articles/article.aspx?p=25862
		/// </summary>
		/// <returns>Guid for use in database tables as primary key</returns>
		public static Guid NewDbGuid()
		{
			byte[] buf = Guid.NewGuid().ToByteArray();
			byte[] tbuf = BitConverter.GetBytes(TimeUtils.UtcNowSeq.Ticks);
			Array.Reverse(tbuf);
			Buffer.BlockCopy(tbuf, 0, buf, 10, 6);
			return new Guid(buf);
		}

		/// <summary>
		/// Create a Guid from an int.
		/// </summary>
		/// <param name="i">number to encode</param>
		/// <param name="group">byte to identify kind of number</param>
		/// <returns></returns>
		public static Guid FromInt(byte group, Int32 i)
		{
			var buf = new byte[16];
			buf[6] = group;
			byte[] ibuf = BitConverter.GetBytes(i);
			Array.Reverse(ibuf);
			Buffer.BlockCopy(ibuf, 0, buf, 12, 4);
			return new Guid(buf);
		}

		private static readonly Guid _FarFutureID = new Guid("00000000-0000-0000-0000-08f931230614"); //2050-01-01 

		public static bool IsSequential(Guid id)
		{
			return SequentialGuidComparer.Compare2(id, _FarFutureID) < 0;
		}
	}

	/// <summary>
	/// Compare 2 Guids, using SQL Server sorting order. This is different from Guid.CompareTo.
	/// Can be used in Array.Sort(array, new SequentialGuidComparer()). 
	/// </summary>
	public sealed class SequentialGuidComparer : IComparer<Guid>
	{
		public int Compare(Guid g1, Guid g2)
		{
			return Compare2(g1, g2);
		}

		public static int Compare2(Guid g1, Guid g2)
		{
			byte[] b1 = g1.ToByteArray();
			byte[] b2 = g2.ToByteArray();
			for (int i = 10; i < 16; i++)
			{
				if (b1[i] < b2[i]) return -1;
				if (b1[i] > b2[i]) return 1;
			}
			return 0;
		}
	}

	/// <summary>
	/// Miscellaneous Guid related functions
	/// </summary>
	public class GuidHelper
	{
		/// <summary>
		/// Store ASCII characters (upto 16) in a Guid.
		/// The result is non-sortable.
		/// </summary>
		/// <param name="s">text to store as Guid</param>
		/// <returns>Guid</returns>
		public static Guid FromAscii(string s)
		{
			var buf = new byte[16];
			byte[] sbuf = Encoding.ASCII.GetBytes(s);
			Buffer.BlockCopy(sbuf, 0, buf, 0, Math.Min(sbuf.Length, 16));
			return new Guid(buf);
		}

		/// <summary>
		/// Convert guid to ASCII text.
		/// </summary>
		/// <param name="g">guid containing text</param>
		/// <returns>the ASCII text</returns>
		public static string ToAscii(Guid g)
		{
			byte[] buf = g.ToByteArray();
			string s = Encoding.ASCII.GetString(buf);
			return s.TrimEnd('\0');
		}

		public static Guid FromDateTime(DateTime dt)
		{
			var buf = new byte[16];
			byte[] tbuf = BitConverter.GetBytes(dt.Ticks);
			Array.Reverse(tbuf);
			Buffer.BlockCopy(tbuf, 0, buf, 10, 6);
			return new Guid(buf);
		}

		public static DateTime ToDateTime(Guid dbGuid)
		{
			byte[] buf = dbGuid.ToByteArray();
			byte[] tbuf = new byte[8];
			Buffer.BlockCopy(buf, 10, tbuf, 0, 6);
			Array.Reverse(tbuf);
			return new DateTime(BitConverter.ToInt64(tbuf, 0));
		}

		/// <summary>
		/// Turn string to Guid if valid format, null, "null" or "0". Throw exception if otherwise.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static Guid ToGuid(string s)
		{
			if (string.IsNullOrEmpty(s) || s == "null" || s == "0") return Guid.Empty;
			return new Guid(s);
		}

		/// <summary>
		/// Try to turn string into Guid, if not valid, then return Guid.Empty. Do not throw exception.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static Guid ForceGuid(string s)
		{
			if (string.IsNullOrEmpty(s)) return Guid.Empty;
			try
			{
				return new Guid(s);
			}
			catch
			{
				return Guid.Empty;
			}
		}

		/// <summary>
		/// Fastest safe method to test if a string is a format "D" Guid. Faster than TryParse (4.0) or try-catch or Regex.
		/// </summary>
		/// <param name="g"></param>
		/// <returns></returns>
		public static bool IsGuid(string g)
		{
			if (g.Length != 36) return false;
			if (g[8] != '-' || g[13] != '-' || g[18] != '-' || g[23] != '-') return false;
			int count = 0;
			for (int i = 0; i < 36; i++) if (g[i] == '-') count++;
			if (count != 4) return false;
			for (int i = 0; i < 36; i++)
			{
				char x = g[i];
				if (!(x >= '0' && x <= '9' || x == '-' || x >= 'a' && x <= 'f' || x >= 'A' && x <= 'F')) return false;
			}
			return true;
		}
	}


}