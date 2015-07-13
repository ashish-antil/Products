using System;

namespace Imarda.Lib.Extensions
{
	public static class ExceptionExtensions
	{
		public static T ToMessageResponse<T>(this Exception e)
			where T : IServiceMessageResponse, new()
		{
			return new T { Status = false, StatusMessage = e.Message + Environment.NewLine + e.StackTrace};
		}


	}
}