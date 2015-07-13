using System;
using System.Threading;

namespace Imarda.Lib
{
	internal class PassiveLease : LeaseManager.ILease
	{
		private readonly EventWaitHandle _Done;
		private readonly WaitCallback _ExpiryCallback;
		private readonly DateTime _ExpiryTime;
		private readonly object _Key;


		public PassiveLease(object key, int millis, WaitCallback callback)
		{
			_Key = key;
			_ExpiryTime = DateTime.UtcNow.AddMilliseconds(millis);
			_ExpiryCallback = callback;
			_Done = new ManualResetEvent(false);
		}

		#region ILease Members

		public object Key
		{
			get { return _Key; }
		}

		public int ThreadID { get; set; }


		public void Invalidate()
		{
			ThreadID = -1;
		}


		public bool Owned
		{
			get { return ThreadID == Thread.CurrentThread.ManagedThreadId && !Expired; }
		}

		public EventWaitHandle Done
		{
			get { return _Done; }
		}

		/// <summary>
		/// Reduce the time left by one unit, if positive.
		/// </summary>
		/// <returns>true if expired, false if still time left</returns>
		public bool Expired
		{
			get { return DateTime.UtcNow > _ExpiryTime; }
		}

		public WaitCallback ExpiryCallback
		{
			get { return _ExpiryCallback; }
		}

		#endregion

		public override string ToString()
		{
			return string.Format("PassiveLease({0},{1},exp?{2})", _Key, _ExpiryTime, Expired);
		}
	}
}