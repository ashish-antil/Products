using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Imarda.Lib;

namespace FernBusinessBase
{
	[MessageContract]
	[Serializable]
    public class KeyValueListResponse : ParameterMessageBase, IServiceMessageResponse
	{
		/// <summary>
		/// Create an OK response.
		/// </summary>
		public KeyValueListResponse()
		{
			Status = true;
			StatusMessage = "OK";
		}

		/// <summary>
		/// Initialize a KeyValueListResponse. 
		/// </summary>
		/// <param name="key1">first key</param>
		/// <param name="nonNullValue1">first value</param>
		/// <param name="args">subsequent key, value pairs</param>
		public KeyValueListResponse(string key1, object nonNullValue1, params object[] args)
			: this()
		{
			if (key1 == null) throw new ArgumentNullException("key1");
			if (nonNullValue1 == null) throw new ArgumentNullException("nonNullValue1");
			Parameters.Add(key1, nonNullValue1.ToString());
			AddParameters(args);
		}

		[MessageBodyMember]
		public bool Status { get; set; }

		[MessageBodyMember]
		public string StatusMessage { get; set; }

		[MessageBodyMember]
		public string ErrorCode { get; set; }

		public object Payload
		{
			get { return Parameters; }
		}
	}
}
