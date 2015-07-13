using System;

namespace FernBusinessBase
{
	public class CommunicationException : Exception
	{
		public CommunicationException(Exception innerException)
			: base("A service call threw a communication exception", innerException)
		{
		}
	}
}
