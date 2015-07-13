//& IM-3222
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	[MessageContract]
	public class SaveStringListRequest : ParameterMessageBase
	{
		public SaveStringListRequest() { }

		public SaveStringListRequest(Dictionary<Guid, string> data)
		{
			Data = data;
		}


		[MessageBodyMember]
		public Dictionary<Guid, string> Data { get; set; }

		public void Add(Guid g, string s)
		{
			if (Data == null) Data = new Dictionary<Guid, string>();
			Data[g] = s;
		}
	}
}
