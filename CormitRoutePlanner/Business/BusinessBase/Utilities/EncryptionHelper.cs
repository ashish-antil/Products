#region

using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Imarda.Lib;

#endregion

namespace FernBusinessBase
{
	public static class EncryptionHelper
	{
		//87961661-ffde-4456-bfa1-0cce3baca7d4
		private static byte[] _IV = { 0x87, 0x96, 0x16, 0x61, 0xff, 0xde, 0x44, 0x56, 0xbf, 0xa1, 0x0c, 0xce, 0x3b, 0xac, 0xa7, 0xd4 };
		private const string _PasswordLetters = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789";
		private const string _PasswordSymbols = "!?.,:;@*-+=/[]_";
		private static CryptoRandom _Random = new CryptoRandom();

		public static string GenPassword(int n)
		{
			var pwd = new char[n];
			for (int i = 0; i < n; i++) pwd[i] = _PasswordLetters[_Random.Next(_PasswordLetters.Length)];
			pwd[_Random.Next(n)] = _PasswordSymbols[_Random.Next(_PasswordSymbols.Length)];
			return new string(pwd);
		}

		public static string Encrypt(Guid key, string plain, int minLength)
		{
			try
			{
				byte[] bkey = key.ToByteArray();
				var des = TripleDESCryptoServiceProvider.Create();
				byte[] input = Encoding.UTF8.GetBytes(plain);
				Pad(ref input, minLength);
				var ms = new MemoryStream();
				ICryptoTransform transform = des.CreateEncryptor(bkey, _IV);
				var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
				cs.Write(input, 0, input.Length);
				cs.FlushFinalBlock();
				byte[] output = ms.ToArray();
				return Convert.ToBase64String(output); // return cipher
			}
			catch
			{
				return string.Empty;
			}
		}


		public static string Decrypt(Guid key, string cipher)
		{
			try
			{
				byte[] bkey = key.ToByteArray();
				var des = TripleDESCryptoServiceProvider.Create();
				byte[] input = Convert.FromBase64String(cipher);
				var ms = new MemoryStream();
				ICryptoTransform transform = des.CreateDecryptor(bkey, _IV);
				var cs = new CryptoStream(ms, transform, CryptoStreamMode.Write);
				cs.Write(input, 0, input.Length);
				cs.FlushFinalBlock();
				byte[] output = ms.ToArray();
				Trim(ref output);
				return Encoding.UTF8.GetString(output); // return plain
			}
			catch
			{
				return string.Empty;
			}
		}


		/// <summary>
		/// Pad an array with random bytes if its size is less then 'min'.
		/// Leave a 0 byte between the content and the padding.
		/// Use the EncryptionHelper.Trim method to remove the padding from a given byte array
		/// </summary>
		/// <param name="arr">array i/o</param>
		/// <param name="min">minimum number of bytes</param>
		/// <returns>true if padded, false if not.</returns>
		public static bool Pad(ref byte[] arr, int min)
		{
			if (arr.Length < min)
			{
				byte[] arr2 = new byte[min];
				_Random.NextBytes(arr2);
				arr.CopyTo(arr2, 0);
				arr2[arr.Length] = 0;
				arr = arr2;
				return true;
			}
			else return false;
		}

		/// <summary>
		/// Remove the padding from the given array. The padding starts after the first 0 byte.
		/// </summary>
		/// <param name="arr">array, i/o</param>
		/// <returns>true if trimmed</returns>
		public static bool Trim(ref byte[] arr)
		{
			int i = 0;
			while (i < arr.Length)
			{
				if (arr[i] == 0)
				{
					byte[] arr2 = new byte[i];
					Array.Copy(arr, arr2, i);
					arr = arr2;
					return true;
				}
				i++;
			}
			return false;
		}
	}
}
