using System;
using System.Reflection;
using System.Text;
using Imarda.Lib;

// ReSharper disable once CheckNamespace
namespace FernBusinessBase.Errors
{
	/// <summary>
	/// Thrown by Check().
	/// </summary>
	public class MessageResponseException : Exception
	{
		private readonly IServiceMessageResponse _response;

		public MessageResponseException(IServiceMessageResponse response)
		{
			_response = response;
		}

		public MessageResponseException(IServiceMessageResponse response, string message)
			: base(message)
		{
			_response = response;
		}

		public IServiceMessageResponse Response { get { return _response; } }

		public override string ToString()
		{
			if (null != Response)
			{
				return ServiceMessageHelper.ToString(Response);
			}

			//IM-5885 - add exception message and stacktrace

			var sb = new StringBuilder();
			sb.AppendLine(GetType().Name);
			sb.AppendLine(Message);
			if (null != StackTrace)
			{
				sb.AppendLine(StackTrace);
			}

			return sb.ToString();

			//return Response == null ? "MessageResponseException" : ServiceMessageHelper.ToString(Response);
		}
	}

}
