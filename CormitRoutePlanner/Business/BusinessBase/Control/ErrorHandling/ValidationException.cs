using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FernBusinessBase
{
	public class ValidationException : Exception
	{
		private readonly string[] _Errors;

		public ValidationException()
		{
		}

		public ValidationException(string[] errors)
		{
			_Errors = errors;
		}

		public string[] Errors { get { return _Errors; } }
	}
}
