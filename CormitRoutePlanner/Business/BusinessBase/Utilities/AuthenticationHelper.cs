using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Imarda.Lib;
namespace FernBusinessBase
{
	public static class AuthenticationHelper
	{
		private static CryptoRandom _Random = new CryptoRandom();


		#region "Helper function"
		public static string ComputeHashOldStyle(string saltString, string password)
		{
			string result = "";
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] salt = Encoding.UTF8.GetBytes(saltString);
			byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
			byte[] passwordSalt = md5.ComputeHash(passwordBytes);

			byte[] saltyPassword = new byte[salt.Length + passwordSalt.Length];
			// Copy plain text bytes into resulting array.
			for (int i = 0; i < salt.Length; i++)
				saltyPassword[i] = salt[i];

			// Append salt bytes to the resulting array.
			for (int i = 0; i < passwordSalt.Length; i++)
				saltyPassword[salt.Length + i] = passwordSalt[i];

			result = Convert.ToBase64String(saltyPassword);
			return result;
		}
		#endregion


		public static byte[] ComputePasswordHash(Guid salt, string password)
		{
			SHA256 sha = SHA256.Create();
			byte[] s = salt.ToByteArray();
			byte[] p = Encoding.UTF8.GetBytes(password);
			byte[] b = new byte[s.Length + p.Length];
			Buffer.BlockCopy(s, 0, b, 0, s.Length);
			Buffer.BlockCopy(p, 0, b, s.Length, p.Length);
			return sha.ComputeHash(b);
		}

		public static Guid GenerateSalt()
		{
			byte[] buffer = new byte[16];
			_Random.NextBytes(buffer);
			byte[] merge = Guid.NewGuid().ToByteArray();
			for (int i = 0; i < buffer.Length; i++) buffer[i] ^= merge[i];
			return new Guid(buffer);
		}

		#region Security

		/*
		/// <summary>
		/// Encrypts the plainText using the key as salt. 
		/// </summary>
		/// <param name="plainText"></param>
		/// <param name="useHashing">whether to calc a hash of the key before using it in the encryption (this is pointless, though)</param>
		/// <param name="key"></param>
		/// <returns></returns>
		public string Encrypt(string plainText, bool useHashing, string key)
		{
			byte[] buffer;
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			if (useHashing)
			{
				MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
				buffer = provider.ComputeHash(Encoding.UTF8.GetBytes(key));
				provider.Clear();
			}
			else
			{
				buffer = Encoding.UTF8.GetBytes(key);
			}
			TripleDESCryptoServiceProvider provider2 = new TripleDESCryptoServiceProvider();
			provider2.Key = buffer;
			provider2.Mode = CipherMode.ECB;
			provider2.Padding = PaddingMode.PKCS7;
			byte[] inArray = provider2.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
			provider2.Clear();
			return Convert.ToBase64String(inArray, 0, inArray.Length);
		}
		*/

		//TODO MV: the useHashing is pointless, it does not add anything to security in encrypt/decrypt.

		public static string Decrypt(string cipherText, bool useHashing, string key)
		{
			byte[] keyArray;
			//get the byte code of the string

			byte[] toEncryptArray = Convert.FromBase64String(cipherText);

			//Get your key from config file to open the lock!
			//string key = (string)settingsReader.GetValue("reportServerCredential", typeof(String));
			if (useHashing)
			{
				//if hashing was used get the hash code with regards to your key
				MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
				keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
				//release any resource held by the MD5CryptoServiceProvider

				hashmd5.Clear();
			}
			else
			{
				//if hashing was not implemented get the byte code of the key
				keyArray = UTF8Encoding.UTF8.GetBytes(key);
			}

			TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
			//set the secret key for the tripleDES algorithm
			tdes.Key = keyArray;
			//mode of operation. there are other 4 modes.
			//We choose ECB(Electronic code Book)
			tdes.Mode = CipherMode.ECB;
			//padding mode(if any extra byte added)
			tdes.Padding = PaddingMode.PKCS7;
			ICryptoTransform cTransform = tdes.CreateDecryptor();
			byte[] resultArray = cTransform.TransformFinalBlock
					(toEncryptArray, 0, toEncryptArray.Length);
			//Release resources held by TripleDes Encryptor
			tdes.Clear();
			//return the Clear decrypted TEXT
			return UTF8Encoding.UTF8.GetString(resultArray);
		}

		private static int RandomNumber(int min, int max)
		{
			return _Random.Next(min, max);
		}

		// rewrite the method to generate of password, don't use a pattern (hackers have rainbow-tables), generate complete random

		private static string RandomString(int size, bool lowerCase)
		{
			StringBuilder builder = new StringBuilder();
			char ch;
			for (int i = 0; i < size; i++)
			{
				ch = (char)_Random.Next('A', 'Z' + 1);
				builder.Append(ch);
			}
			if (lowerCase)
				return builder.ToString().ToLower();
			return builder.ToString();
		}

		public static string GetPassword()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(RandomString(4, true));
			builder.Append(RandomNumber(1000, 9999));
			builder.Append(RandomString(2, false));
			return builder.ToString();
		}
		#endregion
	}
}
