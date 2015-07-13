//& IM-4369
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Imarda.Lib;

namespace Imarda.Lib
{
	public enum ExpiryControl
	{
		None,
		Delete, // delete the item
		Restart, // restart the timer for expiry by adding the default period
		Repeat, // trigger again on the next round of checking
	}

	public class EventTimer
	{
		#region EventTimer is a Singleton

		public static EventTimer Instance
		{
			get
			{
				if (_Instance == null) _Instance = new EventTimer();
				return _Instance;
			}
		}

		private static EventTimer _Instance;

		/// <summary>
		/// Constructs the one and only ImardaTaskProxyManager.
		/// </summary>
		private EventTimer()
		{
			var period = ConfigUtils.GetTimeSpan("GatewayEventTimer", TimeSpan.FromSeconds(30));
			_Timer = new Timer(new TimerCallback(CheckAll), null, TimeSpan.FromSeconds(30), period);
		}

		#endregion

		private Dictionary<string, ExpiryDefinition> _Expiry = new Dictionary<string, ExpiryDefinition>();
		private Timer _Timer;
		private readonly object _Sync = new object();

		private void CheckAll(object obj)
		{
			DateTime now = DateTime.UtcNow;
			var list = new List<string>();
			lock (_Sync)
			{
				foreach (string key in _Expiry.Keys)
				{
					DateTime dt = _Expiry[key].Expiry;
					if (now > dt) list.Add(key);
				}
			}
			// Handle the expired items
			foreach (string key in list)
			{
				// If handling delegate defined, execute it and use return value to decide what to do with the key
				if (HandleExpiry != null)
				{
					ExpiryControl action = HandleExpiry(key);
					switch (action)
					{
						case ExpiryControl.Repeat:
							break;
						case ExpiryControl.Delete:
							Delete(key);
							break;
						case ExpiryControl.Restart:
							Restart(key, null);
							break;
					}
				}
				else
				{
					// Default handling is to delete the key from the dictionary so it won't trigger again.
					Delete(key);
				}
			}
		}

		public TimeSpan DefaultPeriod { get; set; }

		public Func<string, ExpiryControl> HandleExpiry { get; set; }

		public void SetOnce(string key, DateTime expiry)
		{
			lock (_Sync)
			{
				_Expiry[key] = new ExpiryDefinition { Expiry = expiry, Period = TimeSpan.FromDays(100000) };
			}
		}

		public void Restart(string key, DateTime? expiry = null, TimeSpan? period = null)
		{
			lock (_Sync)
			{
				TimeSpan newPeriod;
				DateTime newExpiry;
				if (period == null)
				{
					ExpiryDefinition ed;
					newPeriod = _Expiry.TryGetValue(key, out ed) ? ed.Period : DefaultPeriod;
					newExpiry = (expiry ?? DateTime.UtcNow) + newPeriod;
				}
				else
				{
					newPeriod = period.Value;
					newExpiry = (expiry ?? DateTime.UtcNow) + newPeriod;
				}
				_Expiry[key] = new ExpiryDefinition { Expiry = newExpiry, Period = newPeriod };
			}

		}

		public void Delete(string key)
		{
			lock (_Sync) _Expiry.Remove(key);
		}

	}

	public struct ExpiryDefinition
	{
		public DateTime Expiry { get; set; }
		public TimeSpan Period { get; set; }
	}
}
