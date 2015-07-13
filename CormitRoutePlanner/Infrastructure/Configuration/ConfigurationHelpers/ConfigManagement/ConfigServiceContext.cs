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
	public class ConfigServiceContext : IDisposable
	{
		#region
		private static object _Lock = new object();

		private static Dictionary<int, ConfigServiceContext> _Contexts = new Dictionary<int, ConfigServiceContext>();

		private static void Begin(ConfigServiceContext context)
		{
			lock (_Lock)
			{
				int id = Thread.CurrentThread.ManagedThreadId;
#if DEBUG
				ConfigServiceContext ctx;
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

		public static ConfigServiceContext Get()
		{
			ConfigServiceContext ctx;
			lock (_Lock)
			{
				if (_Contexts.TryGetValue(Thread.CurrentThread.ManagedThreadId, out ctx))
				{
					return ctx;
				}

				string msg = string.Format(
					"Thread {0} not associated with a {1}", 
					Thread.CurrentThread.ManagedThreadId,
					typeof(ConfigServiceContext).Name
					);
				throw new ConfigException(msg);
			}
		}

		#endregion


		private BaseContext _Context;
		private bool _Cacheable;


		public ConfigServiceContext(IConfigData dataProvider, bool cacheable, params Guid[] levels)
			: this(dataProvider, cacheable, new BaseContext(levels))
		{
		}

		public ConfigServiceContext(IConfigData dataProvider, bool cacheable, BaseContext context)
		{
			DataProvider = dataProvider;
			_Cacheable = cacheable;
			_Context = context;
			ConfigServiceContext.Begin(this);
		}

		public IConfigData DataProvider { get; set; }

		public BaseContext Context
		{
			get { return _Context; }
		}

		public bool Cacheable
		{
			get { return _Cacheable; }
		}

		public void Dispose()
		{
			ConfigServiceContext.End();
			Subdispose();
		}

		protected virtual void Subdispose()
		{
		}

		public override int GetHashCode()
		{
			return _Context.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return _Context.Equals(obj);
		}
	}
}
