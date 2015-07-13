using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Diagnostics;
using ServiceStack.Common.Extensions;

namespace Imarda.Lib
{
	public class XmlUtils
	{
        public static T Deserialize<T>(string s)
        {
            return (T)Deserialize(s, typeof(T));
        }
		
        /// <summary>
        /// Deserializes a string into an object of the specified <typeparamref name="T"/> type using a list of known types
        /// </summary>
        /// <typeparam name="T">Type of the object to deserialize into</typeparam>
        /// <param name="s">String to deserialize</param>
        /// <param name="knownTypes">List of known types to be used in the deserialization</param>
        /// <returns></returns>
		public static T Deserialize<T>(string s, IEnumerable<Type> knownTypes)
		{
			var quotas = new XmlDictionaryReaderQuotas { MaxStringContentLength = 64 * 1024 };
            using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(s), quotas))
            {
                var ser = new DataContractSerializer(typeof(T), knownTypes);
                object obj = ser.ReadObject(reader, false);
                return (T)obj;
            }
		}

		/// <summary>
		/// Helper method to get a string deserialized.
		/// If type is given, uses that to deserialize, if null then the fully qualified assembly
		/// name is expected to preceed the XML, separated by a |
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static object Deserialize(string str, Type type)
		{
			if (type == null)
			{
				int p = str.IndexOf('|');
				string assemblyQualifiedName = str.Substring(0, p);
				type = Type.GetType(assemblyQualifiedName);
			}

			try
			{
				var quotas = new XmlDictionaryReaderQuotas { MaxStringContentLength = 64 * 1024 };
				using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(str),quotas))
				{
					var ser = new DataContractSerializer(type);
					object obj = ser.ReadObject(reader, false);
					return obj;
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex); // debugging
				return null;
			}
		}

		/// <summary>
		/// Creates basic xml file content from object - must be DataContract with DataMembers
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string SerializeXmlForFile(object obj)
		{
			return Serialize(obj, false, false, true);
		}

		/// <summary>
		/// Helper method to get an object serialized for storage in the Task.Arguments field.
		/// </summary>
		/// <param name="obj">object to be serialized</param>
		/// <param name="serializeTypeInfo">true: prefix xml string by assembly qualified name</param>
		/// <param name="omitXmlDeclaration"></param>
		/// <param name="indent"></param>
		/// <returns></returns>
		public static string Serialize(object obj, bool serializeTypeInfo, bool omitXmlDeclaration = true, bool indent = false)
		{
			var type = obj.GetType();
			var serializer = new DataContractSerializer(type);
			var sb = new StringBuilder();
			var settings = new XmlWriterSettings
			{
				OmitXmlDeclaration = omitXmlDeclaration,
				Encoding = Encoding.UTF8,
				Indent = indent
			};
			using (var writer = XmlWriter.Create(sb, settings))
			{
				serializer.WriteObject(writer, obj);
			}
			return serializeTypeInfo ? type.AssemblyQualifiedName + '|' + sb : sb.ToString();
		}

		public static string Serialize(object obj)
		{
			return Serialize(obj, false);
		}

	}
}