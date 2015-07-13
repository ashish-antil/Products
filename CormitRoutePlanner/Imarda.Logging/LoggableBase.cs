using System.Collections.Specialized;

namespace Imarda.Logging
{
	public abstract class LoggableBase:ILoggable
	{
		protected LoggableBase(ILogger errorLogger)
		{
			Log = errorLogger;
		}
		public ILogger Log { get; private set; }
	}
}
