using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Imarda.Lib
{
	public static class NumberUtils
	{
		/// <summary>
		/// Fast way to check if a type is a kind of number.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsNumberType(Type type)
		{
			return (type == typeof (Int32) ||
			        type == typeof (Double) ||
			        type == typeof (Decimal) ||
			        type == typeof (Int16) ||
			        type == typeof (Int64) ||
			        type == typeof (Byte) ||
			        type == typeof (SByte) ||
			        type == typeof (Single) ||
			        type == typeof (UInt16) ||
			        type == typeof (UInt32) ||
			        type == typeof (UInt64));
		}

		/// <summary>
		/// Fast way to check if an object is a kind of number.
		/// </summary>
		/// <returns></returns>
		public static bool IsNumber(object obj)
		{
			if (obj is IConvertible)
			{
				TypeCode tc = ((IConvertible) obj).GetTypeCode();
				return (tc >= TypeCode.SByte && tc <= TypeCode.Decimal);
			}
			else return false;
		}

		/// <summary>
		/// Xor all the bytes and return the result
		/// </summary>
		/// <param name="data">input data for checksum</param>
		/// <param name="start">where to start in the data</param>
		/// <param name="end">0 or positive: absolute index; negative: -1 is last char, -2 last but one etc.</param>
		/// <returns></returns>
		public static byte XorChecksum(byte[] data, int start, int end)
		{
			byte checksum = 0;
			int n = end >= 0 ? end : data.Length + end;
			for (int i = start; i <= n; i++) checksum ^= data[i];
			return checksum;
		}

		//& IM-3518
		public static byte BasicChecksum(byte[] data, int start, int end)
		{
			byte checksum = 0;
			int n = end >= 0 ? end : data.Length + end;
			for (int i = start; i <= n; i++) checksum += data[i];
			return checksum;
		}
		//. IM-3518

		public static byte TwosComplimentChecksum(byte[] numbers, bool overall, int startPosn, int endPosn)
		{
			if (numbers.Length == 0) return 0;
			if (!overall && (startPosn > numbers.Length - 1 || endPosn > numbers.Length - 1 || startPosn > endPosn)) return 0;

			// Seems data is ok to proceed
			if (overall)
			{
				startPosn = 0;
				endPosn = numbers.Length - 1;
			}

			uint checksum = 0;
			for (int i = startPosn; i <= endPosn; i++)
			{
				checksum += numbers[i];
			}
			checksum = (uint)-checksum;			// make it negative, dropping overflow
			return (Byte)(checksum & 0xff);		// invert the bits
		}

		/// <summary>
		/// With some communication protocols e.g. Garmin messaging, there is a certain byte vaue used as an escape character.
		/// If that value is present in the payload, then it must be escaped. This method handles the escaping of the escape character.
		/// </summary>
		/// <param name="byteArray"></param>
		/// <param name="stuffByte"></param>
		/// <param name="overall"></param>
		/// <param name="startPosn"></param>
		/// <param name="endPosn"></param>
		/// <returns></returns>
		public static byte[] ByteStuffing(byte[] byteArray, byte stuffByte, bool overall, int startPosn, int endPosn)
		{
			if (byteArray.Length == 0) return new byte[] { 0 };
			if (!overall && (startPosn > byteArray.Length - 1 || endPosn > byteArray.Length - 1 || startPosn > endPosn)) return new byte[]{0};

			// Seems data is ok to proceed
			if (overall)
			{
				startPosn = 0;
				endPosn = byteArray.Length - 1;
			}

			// Move byte array to a new working array, adding in stuff bytes as needed
			int newByteArrayLength = byteArray.Length;
			byte[] workingArray = new byte[byteArray.Length*2];		// twice as long as byte array provided, to ensure room to expand
			int wap = 0;
			bool stuffByteFound = false;
			for (int i = 0; i < byteArray.Length; i++)
			{
				if (i >= startPosn && i <= endPosn && byteArray[i] == stuffByte)
				{
					stuffByteFound = true;
					workingArray[wap++] = stuffByte;		// escape the stuff byte with another stuff byte
					newByteArrayLength++;
				}
				workingArray[wap++] = byteArray[i];
			}

			if (!stuffByteFound) return byteArray;	// array unchanged, as no stuff bytes found that needed escaping

			// Move working array to a new fixed length array
			var newByteArray = new byte[newByteArrayLength];
			for (int x = 0; x < newByteArrayLength; x++)
			{
				newByteArray[x] = workingArray[x];
			}
			return newByteArray; 
		}

		/// <summary>
		/// Turn a string like "1,2,5..20,23..50+2" int an array, expand the ranges.
		/// A range can be defined by using "..", and optionally add +n to define a step n within the range.
		/// between two numbers (no spacing), e.g. 1,5..10+2,18,-4..-2 = 1,5,7,9,18,-4,-3,-2
		/// Use space or comma as separators.
		/// </summary>
		/// <param name="s">string that contains the numbers and range</param>
		/// <param name="sort">true to sort the result in ascending order</param>
		/// <returns></returns>
		public static int[] ToIntArray(string s, bool sort)
		{
			if (s == null) return null;
			string[] ss = s.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var tt = new List<int>();
			foreach (string item in ss)
			{
				try
				{
					int p = item.IndexOf("..");
					if (p != -1)
					{
						int step = 1;
						int q = item.IndexOf("+", p);
						if (q != -1) int.TryParse(item.Substring(q + 1), out step);
						else q = item.Length;
						int lo, hi;
						int.TryParse(item.Substring(0, p), out lo);
						int.TryParse(item.Substring(p + 2, q - p - 2), out hi);
						for (int k = lo; k <= hi; k += step) tt.Add(k);
					}
					else
					{
						tt.Add(Convert.ToInt32(item));
					}
				}
				catch (Exception)
				{
					tt.Add(0);
				}
			}
			int[] array = tt.ToArray();
			if (sort) Array.Sort(array);
			return array;
		}
	
		#region CRC32

		private static Crc32 _Crc32;

		public static uint ComputeCrc32(byte[] data)
		{
			if (_Crc32 == null) _Crc32 = new Crc32();
			return _Crc32.Compute(data);
		}


		/// <summary>
		/// http://www.sanity-free.com/12/crc32_implementation_in_csharp.html
		/// </summary>
		private class Crc32
		{
			private readonly uint[] _Table;


			public Crc32()
			{
				uint poly = 0xedb88320;
				_Table = new uint[256];
				for (uint i = 0; i < _Table.Length; ++i)
				{
					uint a = i;
					for (int j = 8; j > 0; --j)
					{
						if ((a & 1) == 1)
						{
							a = ((a >> 1) ^ poly);
						}
						else
						{
							a >>= 1;
						}
					}
					_Table[i] = a;
				}
			}

			public uint Compute(byte[] bytes)
			{
				uint crc = 0xffffffff;
				foreach (byte t in bytes)
				{
					var index = (byte) (crc & 0xff ^ t);
					crc = ((crc >> 8) ^ _Table[index]);
				}
				return ~crc;
			}
		}

		#endregion CRC32


        //public static string CalcCRC16(string strInput)
        public static uint  CalcCRC16(byte[] data)
        {
            var crc16Kermit = new Crc16(Crc16Mode.CcittKermit);
            var checksumtwoBytes = crc16Kermit.ComputeChecksumBytes(data);
            var checksumfourBytes = new byte[4];
            checksumfourBytes[0] = 0;
            checksumfourBytes[1] = 1;
            checksumfourBytes[2] = checksumtwoBytes[0];
            checksumfourBytes[3] = checksumtwoBytes[1];
            return BitConverter.ToUInt32(checksumfourBytes, 0);//.ToString("X4");
        }

        //public static string CalcCRC16(string strInput)
        public static byte[] CalcCRC16InBytes(byte[] data)
        {
            var crc16Kermit = new Crc16(Crc16Mode.CcittKermit);
            var checksum = crc16Kermit.ComputeChecksumBytes(data);
            return checksum;
            //return BitConverter.ToUInt32(checksum, 0);//.ToString("X4");
        }
        public static Byte[] GetBytesFromHexString(string strInput)
        {
            Byte[] bytArOutput = new Byte[] { };
            if (!string.IsNullOrEmpty(strInput) && strInput.Length % 2 == 0)
            {
                SoapHexBinary hexBinary = null;
                try
                {
                    hexBinary = SoapHexBinary.Parse(strInput);
                    if (hexBinary != null)
                    {
                        bytArOutput = hexBinary.Value;
                    }
                }
                catch (Exception)
                {
                }
            }
            return bytArOutput;
        }

		private static readonly Random _Random = new Random();

		public static int CreatePIN(int length)
		{
			if (length < 1) length = 1;
			else if (length > 9) length = 9;
			return _Random.Next(Convert.ToInt32(Math.Pow(10, length)));
		}
	}
}