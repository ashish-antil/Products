using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace Imarda.Lib.Utilities
{
	public static class ReflectionUtils
	{
		/// <summary>
		///     Returns a list of PropertyInfo for the public properties of a class
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <param name="sort">Optional alpha sort</param>
		/// <param name="declaredOnly">inherited members excluded</param>
		/// <param name="dataMembersOnly">includes only members with the DataMember attribute</param>
		/// <param name="ignoredAttributeType"></param>
		/// <returns></returns>
		public static List<PropertyInfo> GetPropertyInfos<T>(T t, bool sort = false, bool declaredOnly = false, bool dataMembersOnly = false, Type ignoredAttributeType = null)
			where T : class
		{
			var typ = (typeof (T));
			var bindingsFlags = BindingFlags.Public | BindingFlags.Instance;
			if (declaredOnly) bindingsFlags |= BindingFlags.DeclaredOnly;
			var propInfos = typ.GetProperties(bindingsFlags);

			//foreach (var propertyInfo in propInfos)
			//{
			//	var attribs = Attribute.GetCustomAttributes(propertyInfo);
			//	var b = Attribute.IsDefined(propertyInfo, typeof(DataMemberAttribute));
			//}

			if (dataMembersOnly)
			{
				propInfos = propInfos.Where(p => Attribute.IsDefined(p, typeof (DataMemberAttribute))).ToArray();
			}

			if (null != ignoredAttributeType)
			{
				propInfos = propInfos.Where(p => !Attribute.IsDefined(p, ignoredAttributeType)).ToArray();
			}

			Debug.Assert(propInfos != null, "propInfos != null");
			var lstPropInfos = new List<PropertyInfo>(propInfos);
			if (sort)
			{
				SortPropertyInfos(ref lstPropInfos);
			}
/*
#if DEBUG
			foreach (var pi in lstPropInfos)
			{
				Debug.WriteLine(pi.Name, pi.PropertyType);
			}
#endif
*/

			return lstPropInfos;
		}

		public static void SortPropertyInfos(ref List<PropertyInfo> lstPropInfos)
		{
			lstPropInfos.Sort((x, y) => String.CompareOrdinal(x.Name, y.Name));
/*
#if DEBUG
			foreach (var pi in lstPropInfos)
			{
				Debug.WriteLine(pi.Name, pi.PropertyType);
			}
#endif
*/
		}

		/// <summary>
		///     Returns the values for the public properties of the class
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <param name="sort">Optional alpha sort based on property names</param>
		/// <param name="declaredOnly"></param>
		/// <param name="dataMembersOnly"></param>
		/// <param name="ignoredAttributeType"></param>
		/// <returns></returns>
		public static object[] GetPropertyValues<T>(T t, bool sort = false, bool declaredOnly = false, bool dataMembersOnly = false, Type ignoredAttributeType = null)
			where T : class
		{
			var lstPropInfos = GetPropertyInfos(t, sort, declaredOnly, dataMembersOnly, ignoredAttributeType);
			var lstObjs = new List<object>();
			lstPropInfos.ForEach(p => lstObjs.Add(p.GetValue(t, null)));
			return lstObjs.ToArray();
		}

		public static object[] GetPropertyValues<T>(T t, List<PropertyInfo> lstPropInfos, bool sort = false)
	where T : class
		{
			var lstObjs = new List<object>();
			var lstPropInfos2 = new List<PropertyInfo>(lstPropInfos);
			if (sort)
			{
				SortPropertyInfos(ref lstPropInfos2);
			}
			lstPropInfos2.ForEach(p => lstObjs.Add(p.GetValue(t, null)));
			return lstObjs.ToArray();
		}

		/// <summary>
		///     Returns the name and type of the public properties of the class
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="t"></param>
		/// <param name="sort"></param>
		/// <param name="declaredOnly"></param>
		/// <param name="dataMembersOnly"></param>
		/// <returns></returns>
		public static string GetPropertyInfosString<T>(T t, bool sort = false, bool declaredOnly = false, bool dataMembersOnly = false)
			where T : class
		{
			var sb = new StringBuilder(t.GetType().Name);
			var lstPropInfos = GetPropertyInfos(t, sort, declaredOnly, dataMembersOnly);
			lstPropInfos.ForEach(p => sb.AppendLine(p.Name + " " + p.PropertyType.Name));
			return sb.ToString();
		}
	}
}