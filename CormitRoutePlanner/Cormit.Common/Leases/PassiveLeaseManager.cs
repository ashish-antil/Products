using System.Collections;
using System.Linq;
using System.Threading;

namespace Imarda.Lib
{
	public class PassiveLeaseManager : LeaseManager
	{
		public override ILeaseHandle[] Acquire(object[] keys, int millis, WaitCallback expiryCallback)
		{
			lock (_Sync)
			{
				int count = keys.Length;
				var leases = new ILease[count];
				var expired = new int[count];
				var create = new int[count];
				int e = 0; // index for expired[]
				int c = 0; // index for create[]

				ILease lease;
				int threadID = Thread.CurrentThread.ManagedThreadId;

				for (int i = 0; i < keys.Length; i++)
				{
					object key = keys[i];
					if (_Leases.TryGetValue(key, out lease))
					{
						leases[i] = lease;
						if (lease.Expired) expired[e++] = i;
						else if (lease.ThreadID != threadID) return null;
					}
					else
					{
						create[c++] = i;
					}
				}

				for (int i = 0; i < e; i++)
				{
					int k = expired[i];
					lease = leases[k];
					Free(lease);
					if (lease.ExpiryCallback != null) ThreadPool.QueueUserWorkItem(lease.ExpiryCallback, lease);
					leases[k] = Add(keys[k], millis, expiryCallback);
				}

				for (int i = 0; i < c; i++)
				{
					int k = create[i];
					leases[k] = Add(keys[k], millis, expiryCallback);
				}
				return leases;
			}
		}

		public override ILeaseHandle Acquire1(object key, int millis, WaitCallback expiryCallback)
		{
			lock (_Sync)
			{
				ILease lease;
				int threadID = Thread.CurrentThread.ManagedThreadId;

				if (_Leases.TryGetValue(key, out lease))
				{
					if (lease.Expired)
					{
						Free(lease);
						if (lease.ExpiryCallback != null) ThreadPool.QueueUserWorkItem(lease.ExpiryCallback, lease);
						// get a new lease below
					}
					else
					{
						return lease.ThreadID == threadID ? lease : null;
					}
				}
				return Add(key, millis, expiryCallback);
			}
		}

		private ILease Add(object key, int millis, WaitCallback expiryCallback)
		{
			ILease lease = new PassiveLease(key, millis > 0 ? millis : DefaultTimeout, expiryCallback);
			lease.ThreadID = Thread.CurrentThread.ManagedThreadId;
			_Leases.Add(key, lease);
			return lease;
		}


		public override void CleanUp()
		{
			var expired = new ArrayList();
			lock (_Sync)
			{
				foreach (object key in _Leases.Keys.Where(key => _Leases[key].Expired))
				{
					expired.Add(key);
				}
				Remove(expired);
			}
		}
	}
}