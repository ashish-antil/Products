using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Imarda.Lib
{
	/// <summary>
	/// This class creates cryptography-strength random numbers.
	/// I has almost the same interface as the class Random. Use this class to generate salts, passwords, secret codes etc.
	/// </summary>
	public class CryptoRandom
	{
		private RNGCryptoServiceProvider _Rand = new RNGCryptoServiceProvider();

		/// <summary>
		/// Generate integer in the range (min <= x < max). Min and max should be positive.
		/// </summary>
		/// <param name="min">inclusive minimum</param>
		/// <param name="max">exclusive maximum</param>
		/// <returns>strong random number</returns>
		public int Next(int min, int max)
		{
			var b = new byte[4];
			_Rand.GetBytes(b);
			int n = (int)(BitConverter.ToUInt32(b, 0) % (max - min) + min);
			return n;
		}

		/// <summary>
		/// Generate integer in the range (0 <= x < max). Max should be positive.
		/// </summary>
		/// <param name="max">exclusive maximum</param>
		/// <returns>strong random number</returns>
		public int Next(int max)
		{
			return Next(0, max);
		}

		/// <summary>
		/// Fill a byte array with random bits.
		/// </summary>
		/// <param name="bytes">array to be written</param>
		public void NextBytes(byte[] bytes)
		{
			_Rand.GetBytes(bytes);
		}
	}

}
