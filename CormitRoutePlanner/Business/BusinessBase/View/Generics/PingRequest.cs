using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	/// <summary>
	/// request to send data to server to find out the time it takes
	/// </summary>
	/// <typeparam name="T">data that need to be transferred .</typeparam>
	[MessageContract]
	public class PingRequest 
	{
		
		private byte[] _FileData;
 
		[MessageBodyMember]
		public byte[] FileData
		{
			get { return _FileData; }
			set { _FileData = value; }
		}

	}
}
