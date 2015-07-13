using Imarda.Logging;

namespace FernBusinessBase.Interfaces.Processors
{
	public abstract class SimpleProcessorBase : LoggableBase, ISimpleProcessor
	{
		private string _name;

		protected SimpleProcessorBase(ILogger errorLogger) : base(errorLogger)
		{
			_name = GetType().Name;
		}

		public string Name
		{
			get { return _name; }
			protected set { _name = value; }
		}

		public bool Start()
		{
			if(!Enabled){return false;}
			Started = StartProcessing();
			return Started;
		}

		public bool Started { get; protected set; }
		public void Stop()
		{
			StopProcessing();
		}

		protected abstract bool StartProcessing();
		protected abstract void StopProcessing();

		public bool Enabled { get; protected set; }
	}
}
