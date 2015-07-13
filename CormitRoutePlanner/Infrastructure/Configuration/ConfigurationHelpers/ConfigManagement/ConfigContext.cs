using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Imarda360.Infrastructure.ConfigurationService
{
	/// <summary>
	/// A ConfigContext object contains parameters for the query to find a list of configuration versions.
	/// The static methods in this class manage a thread-indexed collection of ConfigContext objects.
	/// </summary>
	/// <example>
	/// <code>
	///		using (new ConfigContext(1, new Guid("09F46487-55C2-479c-AB3A-583EE312BD56"), new Guid...)) 
	///		{
	///			string s = CItem.GetString(dataStore.FindItem(myGuid), 1, 2); // resolves item using myContext
	///			...
	///		}
	/// </code>
	/// </example>
	public class ConfigContext : IDisposable
	{
		#region
		private static object _Lock = new object();

		private static Dictionary<int, ConfigContext> _Contexts = new Dictionary<int, ConfigContext>();

		private static void Begin(ConfigContext context)
		{
			lock (_Lock)
			{
				int id = Thread.CurrentThread.ManagedThreadId;
#if DEBUG
				ConfigContext ctx;
				if (_Contexts.TryGetValue(id, out ctx))
				{
					if (ctx != context) throw new ConfigException("Unpaired call to ConfigContext.Begin");
				}
				else
#endif
				_Contexts[id] = context;
			}
		}

		private static void End()
		{
			lock (_Lock)
			{
#if DEBUG
				if (!_Contexts.Remove(Thread.CurrentThread.ManagedThreadId))
				{
					throw new ConfigException("Unpaired call to ConfigContext.End");
				}
#else
				_Contexts.Remove(Thread.CurrentThread.ManagedThreadId);
#endif
			}
		}

		public static ConfigContext Get()
		{
			ConfigContext ctx;
			lock (_Lock)
			{
				if (_Contexts.TryGetValue(Thread.CurrentThread.ManagedThreadId, out ctx)) return ctx;
				else throw new ConfigException("Thread {0} not associated with a ConfigContext" + Thread.CurrentThread.ToString());
			}
		}

		#endregion



		private int _Hierarchy;
		private Guid[] _Levels;
		private int _HashCode;

		public ConfigContext(int hierarchy, params Guid[] levels)
		{
			_Hierarchy = hierarchy;
			_Levels = levels;
			for (int i = 0; i < Depth; i++) _HashCode ^= _Levels[i].GetHashCode();
			ConfigContext.Begin(this);
		}

		public override int GetHashCode()
		{
			return _HashCode;
		}

		public Guid this[int i]
		{
			get { return _Levels[i]; }
		}

		public int Depth { get { return _Levels.Length; } }

		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != GetType()) return false;
			var other = (ConfigContext) obj;
			if (other._HashCode != _HashCode || other.Depth != Depth) return false;
			for (int i = 0; i < Depth; i++)
			{
				if (other[i] != this[i]) return false;
			}
			return true;
		}

		public void Dispose()
		{
			ConfigContext.End();
		}
	}
}
