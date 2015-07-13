using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Imarda.Lib
{
	/// <summary>
	/// Finds all IMeasurement classes and builds a dictionary which maps TypeName:UnitString to 
	/// PropertyInfo and one of its UnitAttributes. This is for quick look up of the conversion 
	/// property without having to search through the attributes of a class for each conversion.
	/// </summary>
	public class UnitConverter
	{
		#region UnitConverter is a Singleton

		private static UnitConverter _Instance;

		/// <summary>
		/// Constructs the one and only UnitConverter.
		/// Adds all IMeasurements to the dictionary.
		/// </summary>
		private UnitConverter()
		{
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type t in types.Where(t => typeof (IMeasurement).IsAssignableFrom(t)))
			{
				Add(t);
			}
		}

		public static UnitConverter Instance
		{
			get { return _Instance ?? (_Instance = new UnitConverter()); }
		}

		private class ConversionInfo
		{
			internal UnitAttribute UnitInfo { get; set; }
			internal PropertyInfo PropInfo { get; set; }
		}

		#endregion

		private readonly Dictionary<string, ConversionInfo> _Map = new Dictionary<string, ConversionInfo>();

		internal bool Get(Type type, string unitString, out UnitAttribute ua, out PropertyInfo pinfo)
		{
			string key = MakeKey(type, unitString);
			ConversionInfo val;
			if (_Map.TryGetValue(key, out val))
			{
				ua = val.UnitInfo;
				pinfo = val.PropInfo;
				return true;
			}
			else
			{
				ua = null;
				pinfo = null;
				return false;
			}
		}

		private void Add(Type type)
		{
			PropertyInfo[] properties = type.GetProperties();
			foreach (PropertyInfo pinfo in properties)
			{
				var arr = (UnitAttribute[]) pinfo.GetCustomAttributes(typeof (UnitAttribute), false);
				foreach (UnitAttribute ua in arr)
				{
					string key = MakeKey(type, ua.UnitSymbol);
					var val = new ConversionInfo {PropInfo = pinfo, UnitInfo = ua};
					_Map[key] = val;
				}
			}
		}

		/// <summary>
		/// Creates lookup key, which consists of Type:Unit, e.g. Length:mile, Mass:tonne.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="unitString"></param>
		/// <returns></returns>
		private static string MakeKey(Type type, string unitString)
		{
			return type.Name + ':' + unitString;
		}
	}
}