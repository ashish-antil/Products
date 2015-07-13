using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Imarda.Lib
{
	public interface ILeaseHandle
	{
		object Key { get; }
		bool Expired { get; }
		EventWaitHandle Done { get; }
		bool Owned { get; }
	}

	public abstract class LeaseManager
	{
		/// <summary>
		/// Contains the leases. Lookup by a key.
		/// </summary>
		protected Dictionary<object, ILease> _Leases;

		protected object _Sync = new object();


		protected LeaseManager()
		{
			DefaultTimeout = 3000;
			_Leases = new Dictionary<object, ILease>();
		}

		/// <summary>
		/// Millis lease time used to initialize and renew each lease.
		/// </summary>
		public int DefaultTimeout { get; set; }

		public virtual ILeaseHandle Acquire1(object key, int millis, WaitCallback expiryCallback)
		{
			ILeaseHandle[] leases = Acquire(new[] { key }, millis, expiryCallback);
			return leases == null ? null : leases[0];
		}

		/// <summary>
		/// Get leases for the given keys. Either get all or fail. Provide an optional callback that determines
		/// what to do on expiry. The callback will be executed in its own thread, is meant
		/// to do some fast error logging and release of resouces.
		/// </summary>
		/// <param name="key">all the keys</param>
		/// <param name="millis">specify a positive expiry value in millis, or non-positive to use InitialLeaseTime</param>
		/// <param name="expiryCallback">optional, executed on expiry for each lease, lease's key is argument for the callback</param>
		/// <returns>not null if successfully acquired, null if already leased out</returns>
		public abstract ILeaseHandle[] Acquire(object[] key, int millis, WaitCallback expiryCallback);


		/// <summary>
		/// Get an existing lease.
		/// </summary>
		/// <param name="key">lease key</param>
		/// <returns>handle to the lease (either valid or expired), or null if does not exist</returns>
		protected ILeaseHandle Get(object key)
		{
			lock (_Sync) return _Leases[key];
		}

		/// <summary>
		/// Indicate we're finished with the object and no longer need the lease.
		/// The callback will not be called. Next call to Acquire with the same key
		/// will return true.
		/// </summary>
		/// <param name="handles"></param>
		public virtual void Free(ILeaseHandle[] handles)
		{
			lock (_Sync)
			{
				foreach (ILeaseHandle handle in handles) Free(handle);
			}
		}

		public void Free(ILeaseHandle[] handles, object key)
		{
			foreach (ILeaseHandle h in handles)
			{
				if (h.Key.Equals(key))
				{
					Free(h);
					return;
				}
			}
		}

		public void Free(ILeaseHandle handle)
		{
			lock (_Sync)
			{
				var lease = (ILease)handle;
				_Leases.Remove(lease.Key);
				lease.Invalidate();
				if (lease.Done != null) lease.Done.Set();
			}
		}


		public void FreeSimple(object key)
		{
			lock (_Sync)
			{
				ILease lease;
				if (_Leases.TryGetValue(key, out lease))
				{
					_Leases.Remove(key);
					lease.Invalidate();
					if (lease.Done != null) lease.Done.Set();
				}
			}
		}

		public abstract void CleanUp();

		protected void Remove(ArrayList keys)
		{
			foreach (ILease lease in from object key in keys select _Leases[key])
			{
				Free(lease);
				if (lease.ExpiryCallback != null) ThreadPool.QueueUserWorkItem(lease.ExpiryCallback, lease);
			}
		}

		public bool IsLeased(object key)
		{
			lock (_Sync)
			{
				ILease lease;
				if (_Leases.TryGetValue(key, out lease)) return !lease.Expired;
				return false;
			}
		}

		#region Nested type: ILease

		protected internal interface ILease : ILeaseHandle
		{
			WaitCallback ExpiryCallback { get; }
			int ThreadID { get; set; }
			void Invalidate();
		}

		#endregion
	}
}