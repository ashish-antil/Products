using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;


namespace RecipeLib
{
	public static class Crypto
	{

		private static readonly byte[] _GeneralIV = new Guid("a58ed3ca-4827-4dd2-b402-7f66e906abac").ToByteArray();

		private static readonly Rijndael _Algorithm = new RijndaelManaged();

		static Crypto()
		{
			_Algorithm.IV = _GeneralIV;
		}

		private static byte[] CreateKey(string password)
		{
			var sha256 = SHA256.Create();
			byte[] buf = Encoding.UTF8.GetBytes(password);
			buf = sha256.ComputeHash(buf);
			return buf;
		}

		public static string EncryptStringToBase64(string plain, string password)
		{
			_Algorithm.Key = CreateKey(password);
			return Convert.ToBase64String(EncryptBytes(Encoding.UTF8.GetBytes(plain)), Base64FormattingOptions.InsertLineBreaks);
		}

		public static string DecryptStringFromBase64(string cipher64, string password)
		{
			_Algorithm.Key = CreateKey(password);
			return DecryptString(Convert.FromBase64String(cipher64));
		}

		static string DecryptString(byte[] cipher)
		{
			if (cipher == null) return null;
			byte[] buf = DecryptBytes(cipher);
			if (buf == null) return null;
			int i = buf.Length;
			while (i-- > 0)
			{
				if (buf[i] != 0) break;
			}
			i++;
			var str = new byte[i];
			Array.Copy(buf, 0, str, 0, i);
			return Encoding.UTF8.GetString(str);
		}


		private static byte[] EncryptBytes(byte[] plain)
		{
			ICryptoTransform encryptor = _Algorithm.CreateEncryptor();
			var msEncrypt = new MemoryStream();
			var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
			csEncrypt.Write(plain, 0, plain.Length);
			csEncrypt.FlushFinalBlock();
			return msEncrypt.ToArray();
		}

		private static byte[] DecryptBytes(byte[] cipher)
		{
			try
			{
				ICryptoTransform decryptor = _Algorithm.CreateDecryptor();
				var msDecrypt = new MemoryStream(cipher);
				var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
				int len = cipher.Length;
				var plain = new byte[len];
				csDecrypt.Read(plain, 0, len);
				return plain;
			}
			catch
			{
				return null;
			}
		}
	}
}
