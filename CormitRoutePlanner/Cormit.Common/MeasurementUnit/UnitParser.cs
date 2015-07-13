using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Imarda.Lib
{
	public class UnitParser
	{
		#region Singleton

		private static readonly UnitParser _Instance = new UnitParser();

		private UnitParser()
		{
			_Map = new Dictionary<string, MethodInfo>();
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type t in types.Where(t => typeof (IMeasurement).IsAssignableFrom(t)))
			{
				Add(t);
			}
		}

		public static UnitParser Instance
		{
			get { return _Instance; }
		}

		#endregion

		private readonly Dictionary<string, MethodInfo> _Map;

		public MethodInfo Get(string unitString)
		{
			unitString = unitString.Replace('\u00B2', '2').Replace('\u00B3', '3').Replace('\u00B0', '*');
			MethodInfo meth;
			return _Map.TryGetValue(unitString, out meth) ? meth : null;
		}

		private void Add(Type type)
		{
			MethodInfo[] methods = type.GetMethods();
			foreach (MethodInfo minfo in methods)
			{
				var arr = (ParseAttribute[]) minfo.GetCustomAttributes(typeof (ParseAttribute), false);
				foreach (ParseAttribute pa in arr)
				{
					_Map[pa.UnitSymbol] = minfo;
				}
			}
		}

		public IMeasurement Create(double val, string unit)
		{
			if (string.IsNullOrEmpty(unit)) return new Unitless(val);
			MethodInfo meth = Get(unit);
			return (meth == null) ? null : (IMeasurement) meth.Invoke(null, new object[] {val});
		}
	}
}