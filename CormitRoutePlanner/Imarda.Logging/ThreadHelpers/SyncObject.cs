using System;
using System.Threading;

namespace Imarda.Logging
{
	/// <summary>
	/// You can use this object in a using-construct instead of lock (x) to print debug info.
	/// </summary>
	public class SyncObject : IDisposable
	{
		public ErrorLogger Logger { get; set; }

		private readonly string _Name;
		private readonly object _Lock;
		private Thread _CurrentOwner;

		public SyncObject(string name)
		{
			_Name = name;
			_Lock = new object();
		}

		public void Dispose()
		{
			_CurrentOwner = null;
			Monitor.Exit(_Lock);
			//if (string.IsNullOrEmpty(Thread.CurrentThread.Name)) throw new Exception();
			Logger.DebugFormat("- {0} . {1}", _Name, Thread.CurrentThread.Name);
		}

		public IDisposable Lock
		{
			get
			{
				if (_CurrentOwner != null) Logger.DebugFormat("x {0} . {1} , {2}", _Name, Thread.CurrentThread.Name, _CurrentOwner);
				Monitor.Enter(_Lock);
				Logger.DebugFormat("+ {0} . {1}", _Name, Thread.CurrentThread.Name);
				return this;
			}

		}
	}
}
