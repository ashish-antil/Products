using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FernBusinessBase;

namespace FernBusinessBase.Errors
{
	public class ErrorCode
	{
		public ErrorCode()
		{
		}

		public ErrorCode(string errorCode)
		{
			ErrorCode.unpackErrorCode(errorCode, ref _serviceCodePart, ref _funcAreaPart, ref _errorOpPart, ref _errorSubcode);
		}

		public ErrorCode(ErrorServiceCode service, ErrorFuncAreaCode funcArea)
		{
			_serviceCodePart = (byte)service;
			_funcAreaPart = (ushort)funcArea;
			_errorOpPart = 0;
			_errorSubcode = 0;
		}

		public ErrorCode(ErrorServiceCode service, ErrorFuncAreaCode funcArea, ErrorOperation error)
		{
			_serviceCodePart = (byte)service;
			_funcAreaPart = (ushort)funcArea;
			_errorOpPart = (ushort)error;
			_errorSubcode = 0;
		}

		public ErrorCode(ErrorServiceCode service, ErrorFuncAreaCode funcArea, ErrorOperation error, ErrorSubcode errorSubCode)
		{
			_serviceCodePart = (byte)service;
			_funcAreaPart = (ushort)funcArea;
			_errorOpPart = (ushort)error;
			_errorSubcode = (ushort)errorSubCode;
		}

		private byte _serviceCodePart = 0;

		public Byte ServiceCodePart
		{
			get { return _serviceCodePart; }
			set { _serviceCodePart = value; }
		}

		private ushort _funcAreaPart = 0;

		public ushort FuncAreaPart
		{
			get { return _funcAreaPart; }
			set { _funcAreaPart = value; }
		}

		private ushort _errorOpPart = 0;

		public ushort ErrorOperationPart
		{
			get { return _errorOpPart; }
			set { _errorOpPart = value; }
		}

		private ushort _errorSubcode = 0;

		public ushort ErrorSubcode
		{
			get { return _errorSubcode; }
			set { _errorSubcode = value; }
		}

		#region Implicit Conversions

		public static implicit operator ErrorCode(string code)
		{
			return new ErrorCode(code);
		}

		public static implicit operator string(ErrorCode code)
		{
			return packErrorCode(code._serviceCodePart, code._funcAreaPart, code._errorOpPart, code._errorSubcode);
		}

		#endregion

		#region Static Utility Methods

		public static string packErrorCode(byte service, ushort funcArea, ushort errorOp, ushort errorSubCode)
		{

			return service.ToString("000")
					+ "/"
					+ funcArea.ToString("000")
					+ "/"
					+ errorOp.ToString("00000")
					+ "/"
					+ errorSubCode.ToString("00000");
		}

		public static void unpackErrorCode(string errorCode, ref byte service, ref ushort funcArea, ref ushort error, ref ushort errorSubCode)
		{

			string[] split = errorCode.Split('/');

			if (split.Length >= 1)
				service = Convert.ToByte(split[0]);
			if (split.Length >= 2)
				funcArea = Convert.ToByte(split[1]);
			if (split.Length >= 3)
				error = Convert.ToUInt16(split[2]);
			if (split.Length == 4)
			{
				errorSubCode = Convert.ToUInt16(split[3]);
			}
		}

		#endregion
	}
}
