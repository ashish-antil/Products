using System;

namespace Imarda.Lib
{
	public class ActiveCache<K, T>
		where T : class
	{
		private JobQueueHandler _QH;
		private SimpleCache<K, T> _Cache;
		private LeaseManager _LeaseManager;

		private int _DefaultLeaseTime;

		public ActiveCache(JobQueueHandler qm, LeaseManager lm)
		{
			_QH = qm;
			_LeaseManager = lm;
			_Cache = new SimpleCache<K, T>();
			_DefaultLeaseTime = 5000;
			_QH.Start();
		}

		public bool Do(Job job)
		{
			return _QH.Submit(job);
		}

		public bool Active
		{
			get { return _QH.IsRunning; }
			set
			{
				if (value)
				{
					_QH.Start();
				}
				else
				{
					_QH.StopNow();
				}
			}
		}

		public JobQueueHandler QueueHandler { get { return _QH; } }

		public void Close()
		{
			_QH.Close();
		}


		public ILeaseHandle Get(K id, out T target, Func<K, T> retrieve)
		{
			target = null;
			ILeaseHandle h = _LeaseManager.Acquire1(id, _DefaultLeaseTime, null);
			if (h == null) return null;

			target = _Cache.Get(id);
			if (target == null && retrieve != null)
			{
				try
				{
					target = retrieve(id);
					if (target != null) _Cache.Put(id, target);
				}
				catch { }
			}

			if (target == null)
			{
				_LeaseManager.Free(h);
				return null;
			}
			else return h;
		}


		public bool Put(K id, T target)
		{
			ILeaseHandle h = _LeaseManager.Acquire1(id, _DefaultLeaseTime, null);
			if (h == null) return false;
			_Cache.Put(id, target);
			_LeaseManager.Free(h);
			return true;
		}

		public bool Remove(K key)
		{
			ILeaseHandle h = _LeaseManager.Acquire1(key, _DefaultLeaseTime, null);
			if (h == null) return false;
			bool removed = false;
			removed = _Cache.Remove(key);
			_LeaseManager.Free(h);
			return removed;
		}

		public void Unlock(params ILeaseHandle[] handles)
		{
			_LeaseManager.Free(handles);
		}


		public string[] ToStrings()
		{
			return _Cache.ToStrings();
		}

		public T[] ToArray()
		{
			return _Cache.ToArray();
		}
	}
}
