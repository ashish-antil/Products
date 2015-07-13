using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using ServiceStack.Common.Extensions;

namespace Imarda.Lib
{
	public enum ValueFormat
	{
		Strings,
		Mix,
		Arrays,
	}

	public static class StringUtils
	{
		public const char BrokenBar = '?'; // \u00A6
		public const string Pilcrow = "\u00B6";

		#region Value Map functions

		private static readonly string[] _Separator = new[] { "||" };


		/// <summary>
		/// Create an IDictionary containing key->value mappings, where value can be a single string or a string[].
		/// </summary>
		/// <param name="s">the string containing the name-value format</param>
		/// <param name="option">what kind of value to map to</param>
		/// <param name="caseInSensitive">true = ignore case</param>
		/// <returns></returns>
		/// <remarks>
		/// Input string format is:
		/// "key1|value1||key2|value2a|value2b|value2c||key3||key4||key5|value5"
		/// where names should not be duplicate. Where the value is missing, an empty string is inserted.
		/// </remarks>
		public static IDictionary KeyValueMap(this string s, ValueFormat option, bool caseInSensitive)
		{
			string[] nvArray = s.Split(_Separator, StringSplitOptions.RemoveEmptyEntries);
            var map = new HybridDictionary(nvArray.Length, caseInSensitive);
            foreach (string nv in nvArray)
			{
				int p = nv.IndexOf('|');
				if (p == -1)
				{
					if (option == ValueFormat.Arrays) map[nv] = new string[0];
					else map[nv] = string.Empty;
				}
				else
				{
					string key = nv.Substring(0, p);
					if (option == ValueFormat.Strings)
					{
						map[key] = nv.Substring(p + 1);
					}
					else
					{
						string[] values = nv.Substring(p + 1).Split('|');
						if (option == ValueFormat.Mix && values.Length == 1) map[key] = values[0];
						else
						{
							for (int k = 0; k < values.Length; k++) if (values[k] == Pilcrow) values[k] = string.Empty;
							map[key] = values;
						}
					}
				}
			}
			return map;
		}

		public static Dictionary<string, string> KeyStringMap(this string s, bool caseInSensitive)
		{
			var map = s.KeyValueMap(ValueFormat.Strings, caseInSensitive);
			var dict = new Dictionary<string, string>(caseInSensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);
			foreach (string key in map.Keys) dict[key] = map[key] as string;
			return dict;
		}


		/// <summary>
		/// Turns an IDictionary into something like "key1|value1||key2|value2a|value2b|value2c||key3||key4||key5|value5".
		/// </summary>
		/// <param name="map"></param>
		/// <returns></returns>
		public static string KeyValueString(this IDictionary map)
		{
			if (map == null) return string.Empty;

			var sb = new StringBuilder();
            // use ToList<string> to get a Keys snapshot to prevent System.InvalidOperationException: Collection was modified; enumeration operation may not execute.
			foreach (var key in map.Keys.OfType<string>().ToArray())
			{
				sb.Append(key);
				object val = map[key];
				if (val is string)
				{
					val = ((string)val).Replace('|', BrokenBar);
				}
				else if (val is string[])
				{
					var arr = (string[])val;
					val = string.Join("|", arr.Select(t => string.IsNullOrEmpty(t) ? Pilcrow : t.Replace('|', BrokenBar)).ToArray());
				}
				if (val != null && val.ToString() != "") sb.Append('|').Append(val);
				sb.Append("||");
			}
			if (sb.Length > 0) sb.Length -= 2;
			return sb.ToString();
		}

		/// <summary>
		/// Replace or add multiple values to a key-value string. If the key exists, its value
		/// will be replaced. If it does not exist, then a new key-value pair will be added.
		/// This only works with string values, not with string[] values.
		/// </summary>
		/// <param name="s">string of format k|v||k|v||k...</param>
		/// <param name="kvpairs">key, value, key, value, etc.</param>
		/// <returns>new string with new key-values</returns>
		public static string SetKeyValuePairs(this string s, params string[] kvpairs)
		{
			if (kvpairs.Length % 2 == 1) throw new ArgumentException("key without value");
			IDictionary d = s.KeyValueMap(ValueFormat.Strings, true);
			for (int i = 0; i < kvpairs.Length - 1; i += 2) d[kvpairs[i]] = kvpairs[i + 1];
			return d.KeyValueString();
		}

		/// <summary>
		/// Extends s1 with key-value pairs from s2. Where the key is the same, the value of the 
		/// s2 key will replace the value of the s1 key. If the value is an array, the entire
		/// array will be replaced.
		/// </summary>
		/// <param name="arr">k|v-strings</param>
		/// <returns>merged k|v string</returns>
		public static string MergeNonArrayKeyValuePairs(params string[] arr)
		{
			string s = arr[0];
			for (int i = 1; i < arr.Length; i++)
			{
				s = MakeValidKV(s + "||" + arr[i]);
			}
			return s.KeyValueMap(ValueFormat.Strings, true).KeyValueString();
		}

		public static string MakeValidKV(string s)
		{
			if (s.StartsWith("|")) s = s.TrimStart('|');
			if (s.EndsWith("||"))
				s = s.TrimEnd('|'); // check for || rather than | coz "k1|a|b|c|" is valid kv-string: empty last array elem.
			return s;
		}

		/// <summary>
		/// Append to an existing k|v string. 
		/// </summary>
		/// <param name="s">existing k|v string</param>
		/// <param name="k">key, should not contain |</param>
		/// <param name="v">value should not start or end with |</param>
		/// <returns>new k|v string</returns>
		public static string AppendKV(this string s, string k, string v)
		{
			if (k.Contains('|')) k = k.Replace('|', BrokenBar);
			if (v.StartsWith("|") || v.EndsWith("|")) throw new ArgumentException("Value should not start or end with '|'");
			if (string.IsNullOrEmpty(s)) return k + '|' + v; // "" => "k|v"
			if (s.EndsWith("||")) return s + k + '|' + v; // "a|b||" => "a|b||k|v"
			if (s.EndsWith("|")) return s + '|' + k + '|' + v; // "a|" (invalid) => "a||k|v" (valid)
			return s + "||" + k + '|' + v; // "a|b" => "a|b||k|v"
		}

		/// <summary>
		/// Go through the dictionary and for each array value, add the individual elements as entries to a new dictionary.
		/// </summary>
		/// <remarks>
		/// The key for each element is the array key with '#' replaced by the index in the array + 1. If no '#' is 
		/// in the base key than append '_' and the number. E.g. "Car[#]" => "Car[1]", "Car[2]" ...; "Boat" => "Boat_1", "Boat_2", ...
		/// Non-array values get copied to the new dictionary. The boolean copyOriginal indicates whether to copy the original array as well.
		/// </remarks>
		/// <param name="src"></param>
		/// <param name="copyOriginalArray">true to copy the original array into the destination dictionary</param>
		/// <returns>new dictionary with individual array elements</returns>
		public static IDictionary Flatten(IDictionary src, bool copyOriginalArray) //TODO move to EAHelper.cs
		{
			var dest = new HybridDictionary(src.Count);
			foreach (string key in src.Keys)
			{
				object val = src[key];
				bool placeholderInKey = key.Contains('#');
				if (val is Array)
				{
					var arr = (object[])val;
					for (int i = 0; i < arr.Length; i++)
					{
						string num = (i + 1).ToString();
						string keyi = placeholderInKey ? key.Replace("#", num) : key + "_" + num;
						dest[keyi] = arr[i];
					}
					if (copyOriginalArray) dest[key] = src[key];
				}
				else // single value element, not an array
				{
					if (placeholderInKey) // # is in the key, treat it as an array key
					{
						dest[key.Replace('#', '1')] = src[key]; // "a#|b" => "a1|b"
						if (copyOriginalArray) dest[key] = src[key];
					}
					else
					{
						dest[key] = src[key];
					}
				}
			}
			return dest;
		}


		/// <summary>
		/// Use backslash to escape non-ASCII chars, using 2 digit hex code.
		/// Only works on 8-bit chars. 
		/// </summary>
		/// <param name="special">some other characters that also need escaping</param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string EscapeField(char[] special, string text)
		{
			var sb = new StringBuilder();
			foreach (char c in text)
			{
				if (c < 32 || c > 127 || Array.IndexOf(special, c) != -1)
				{
					sb.Append('\\').Append(((int)c).ToString("X2"));
				}
				else sb.Append(c);
			}
			return sb.ToString();
		}

		/// <summary>
		/// Unescape a text that was escaped with EscapeField().
		/// </summary>
		/// <param name="text">escaped text</param>
		/// <returns>string with unescaped chars, but backslash has been replaced by / </returns>
		public static string UnescapeField(string text)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '\\')
				{
					if (i < text.Length - 2)
					{
						string x = string.Concat(text[i + 1], text[i + 2]);
						int n;
						if (int.TryParse(x, NumberStyles.HexNumber, null, out n))
						{
							var d = (char)n;
							if (d == '\\') sb.Append('/');
							else if (n < 9 || (n > 10 && n < 13) || (n > 13 && n < 32) || n > 126)
							{
								sb.Append('~').Append(n.ToString("00"));
							}
							else sb.Append(d);
							i += 2;
						}
						else sb.Append('/');
					}
					else sb.Append('/');
				}
				else sb.Append(c);
			}
			return sb.ToString();
		}

		#endregion Value Map functions

		public static string ShortString(this Guid guid)
		{
			return guid.ToString().ToUpperInvariant().Substring(0, 4);
		}

		/// <summary>
		/// Convert a GUID to a base64 string, without the trailing padding "=="
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		public static string ToBase64(this Guid guid)
		{
			return guid == Guid.Empty ? string.Empty : Convert.ToBase64String(guid.ToByteArray()).Remove(22);
		}

		/// <summary>
		/// Convert a base64 string, optionally ending in "==", to a Guid.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		public static Guid FromBase64(this string s)
		{
			try
			{
				if (string.IsNullOrEmpty(s)) return Guid.Empty; // special compact representation
				if (s.Length == 22) s += "==";
				return new Guid(Convert.FromBase64String(s));
			}
			catch
			{
				throw new ArgumentException("string is not a base64 of a Guid");
			}
		}

		public static string Truncate(this string text, int max)
		{
			if (text == null) return string.Empty;
			const string suffix = "...";
			int p = text.IndexOf("\n", StringComparison.InvariantCulture);
			string subject = (p == -1 ? text : text.Remove(p)).TrimEnd();
			if (subject.Length > max) subject = subject.Remove(max - suffix.Length) + suffix;
			return subject;
		}

		public static string UrlEncode(string url)
		{
			// http://www.w3.org/Addressing/rfc1738.txt
			const string encode = @" 20""22#23%25&26+2B/2F:3A<3C=3D>3E?3F@40[5B\5C]5D^5E`60{7B|7C}7D~7E";

			var sb = new StringBuilder();
			foreach (char c in url)
			{
				if (char.IsLetterOrDigit(c)) sb.Append(c);
				else
				{
					int k;
					for (k = 0; k < encode.Length; k += 3)
					{
						char e = encode[k];
						if (c == e)
						{
							sb.Append('%').Append(encode, k + 1, 2);
							break;
						}
					}
					if (k >= encode.Length) sb.Append(c);
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// Get a text representation of a byte[], show ASCII or show hex.
		/// </summary>
		/// <param name="data">byte array to get a printable string from</param>
		/// <param name="max">max length</param>
		/// <returns></returns>
		public static string GetPrintable(byte[] data, int max)
		{
			string result;
			if (data == null)
			{
				result = "null";
			}
			else
			{
				bool binary = false;
				int n = Math.Min(data.Length, max);
				for (int i = 0; i < n; i++)
				{
					byte c = data[i];
					if (c != 10 && c != 13 && (c < 32 || c >= 127))
					{
						binary = true;
						break;
					}
				}
				if (binary)
				{
					var sb = new StringBuilder("0x");
					for (int i = 0; i < n; i++) sb.AppendFormat("{0:X2} ", data[i]);
					if (n < data.Length) sb.Append("...");
					result = sb.ToString();
				}
				else
				{
					result = Encoding.ASCII.GetString(data).TrimEnd();
				}
			}
			return result.TrimEnd();
		}

		private static readonly string _PathValidatorExpression = "^[^" + string.Join("", Array.ConvertAll(Path.GetInvalidPathChars(), x => Regex.Escape(x.ToString()))) + "]+$";
		private static readonly Regex _PathValidator = new Regex(_PathValidatorExpression, RegexOptions.Compiled);

		private static readonly string _FileNameValidatorExpression = "^[^" + string.Join("", Array.ConvertAll(Path.GetInvalidFileNameChars(), x => Regex.Escape(x.ToString()))) + "]+$";
		private static readonly Regex _FileNameValidator = new Regex(_FileNameValidatorExpression, RegexOptions.Compiled);

		private static readonly string _PathCleanerExpression = "[" + string.Join("", Array.ConvertAll(Path.GetInvalidPathChars(), x => Regex.Escape(x.ToString()))) + "]";
		private static readonly Regex _PathCleaner = new Regex(_PathCleanerExpression, RegexOptions.Compiled);

		private static readonly string _FileNameCleanerExpression = "[" + string.Join("", Array.ConvertAll(Path.GetInvalidFileNameChars(), x => Regex.Escape(x.ToString()))) + "]";
		private static readonly Regex _FileNameCleaner = new Regex(_FileNameCleanerExpression, RegexOptions.Compiled);

		public static bool ValidatePath(string path)
		{
			return _PathValidator.IsMatch(path);
		}

		public static bool ValidateFileName(string fileName)
		{
			return _FileNameValidator.IsMatch(fileName);
		}

		public static string CleanPath(string path)
		{
			return _PathCleaner.Replace(path, "");
		}

		public static string CleanFileName(string fileName)
		{
			return _FileNameCleaner.Replace(fileName, "");
		}

		public static string ReplaceFirstOccurrance(string original, string oldValue, string newValue)
		{
			if (String.IsNullOrEmpty(original)) return String.Empty;
			if (String.IsNullOrEmpty(oldValue) || String.IsNullOrEmpty(newValue)) return original;

			int loc = original.IndexOf(oldValue);
			return loc == -1 ? original : original.Remove(loc, oldValue.Length).Insert(loc, newValue);
		}

		/// <summary>
		/// Returns a string from a delimited string, at the position requested.
		/// eg. caller could request the string found at the 3rd position in a comma delimited string which has 10 "fields"
		/// The posn starts at 1.
		/// If the delimiter is not found, or the field position is not present, then an empty string is returned.
		/// </summary>
		/// <param name="line"></param>
		/// <param name="delim"></param>
		/// <param name="posn"></param>
		/// <returns></returns>
		public static string GetField(string line, char delim, int posn)
		{
			if (line.Trim() == "") return string.Empty;
			int firstDelim = line.IndexOf(delim);
			if (firstDelim == -1) return string.Empty;		// delimiter not present in string

			string[] fields = line.Split(delim);
			if (fields.Length < posn) return string.Empty;	// field position is not present

			return fields[posn - 1];						// returns requested field within the delimited string
		}

		public static int GetFieldToInt(string line, char delim, int posn)
		{
			string fieldValue = GetField(line, delim, posn);
			int fieldInt;
			Int32.TryParse(fieldValue, out fieldInt);
			return fieldInt;
		}

        public static short GetFieldToShort(string line, char delim, int posn)
        {
            string fieldValue = GetField(line, delim, posn);
            short fieldIShort;
            short.TryParse(fieldValue, out fieldIShort);
            return fieldIShort;
        }

        public static byte GetFieldToByte(string line, char delim, int posn)
        {
            string fieldValue = GetField(line, delim, posn);
            byte fieldByte;
            Byte.TryParse(fieldValue, out fieldByte);
            return fieldByte;
        }

        public static double GetFieldToDouble(string line, char delim, int posn)
		{
			string fieldValue = GetField(line, delim, posn);
			double fieldInt;
			Double.TryParse(fieldValue, out fieldInt);
			return fieldInt;
		}

		public static bool StringIsGreater(string firstString, string secondString)
		{
			return String.Compare(firstString, secondString) > 0;
		}

		public static  bool StringIsLesser(string firstString, string secondString)
		{
			return String.Compare(firstString, secondString) < 0;
		}

		public static string ToBitString(BitArray bits)
		{
			var sb = new StringBuilder();

			for (int i = 0; i < bits.Count; i++)
			{
				char c = bits[i] ? '1' : '0';
				sb.Append(c);
			}

			return sb.ToString();
		}
		
		public static string ToBitStringReversed(BitArray bits)
		{
			var sb = new StringBuilder();

			for (int i = bits.Count-1; i > -1; i--)
			{
				char c = bits[i] ? '1' : '0';
				sb.Append(c);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Used to display the content of a byte array in its "raw format" as numbers, space delimited
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static string ToByteString(byte[] bytes)
		{
			string bytesAsString = "";
			foreach (var b in bytes)
			{
				bytesAsString += b + " ";
			}
			return bytesAsString;
		}

		public static string ObjectToString(object obj)
		{
			MemoryStream ms = new MemoryStream();
			new BinaryFormatter().Serialize(ms, obj);         
			return Convert.ToBase64String(ms.ToArray());
		}

		public static object StringToObject(string base64String)
		{
			byte[] bytes = Convert.FromBase64String(base64String);
			MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
			ms.Write(bytes, 0, bytes.Length);
			ms.Position = 0;
			return new BinaryFormatter().Deserialize(ms);
		}

		public static string EntityToString(object obj)
		{
			var ms = new MemoryStream();
			new NetDataContractSerializer().Serialize(ms, obj);
			return Convert.ToBase64String(ms.ToArray());
		}

		public static object StringToEntity(string base64String)
		{
			byte[] bytes = Convert.FromBase64String(base64String);
			MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
			ms.Write(bytes, 0, bytes.Length);
			ms.Position = 0;
			return new NetDataContractSerializer().Deserialize(ms);
		}

		private static readonly Regex _EmailRx = new Regex(@"\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

		public static bool HasValidEmailAddress(this string s)
		{
			return _EmailRx.IsMatch(s);
		}

		public static bool IsTrueString(string s)
		{
			return string.Equals(s, Boolean.TrueString, StringComparison.OrdinalIgnoreCase);
		}

		public static int LevenshteinDistance(string src, string dest)
		{
			var d = new int[src.Length + 1, dest.Length + 1];
			int i, j;
			char[] str1 = src.ToCharArray();
			char[] str2 = dest.ToCharArray();

			for (i = 0; i <= str1.Length; i++)
			{
				d[i, 0] = i;
			}
			for (j = 0; j <= str2.Length; j++)
			{
				d[0, j] = j;
			}
			for (i = 1; i <= str1.Length; i++)
			{
				for (j = 1; j <= str2.Length; j++)
				{
					int cost;
					if (str1[i - 1] == str2[j - 1])
						cost = 0;
					else
						cost = 1;

					d[i, j] =
						Math.Min(
							d[i - 1, j] + 1,              // Deletion
							Math.Min(
								d[i, j - 1] + 1,          // Insertion
								d[i - 1, j - 1] + cost)); // Substitution

					if ((i > 1) && (j > 1) && (str1[i - 1] ==
						str2[j - 2]) && (str1[i - 2] == str2[j - 1]))
					{
						d[i, j] = Math.Min(d[i, j], d[i - 2, j - 2] + cost);
					}
				}
			}

			return d[str1.Length, str2.Length];
		}

        public static byte[] FromHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] HexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }
		
		public static string ToTitleCase(string s)
        {
            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s.ToLower());
        } 
		
		public static List<string> ToSplittedList(this string s, char separator)
		{
			s.ThrowIfNullOrEmpty();
			var splits = s.Split(separator);
			return splits.ToList();
		}
		//error free convert, returns 0 on invalid value
		public static int GetIntValue(string value)
		{
			if (string.IsNullOrEmpty(value))
				return 0;
			try
			{
				return Convert.ToInt32(value);
			}
			catch
			{
				return 0;
			}
		}
    }
}