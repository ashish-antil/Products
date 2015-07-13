using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	[MessageContract]
	public class GetUpdateCountResponseList : BusinessMessageResponse
	{
		private List<GetUpdateCountResponse> _List;

		public GetUpdateCountResponseList()
		{
			_List = new List<GetUpdateCountResponse>();
		}

		[MessageBodyMember]
		public List<GetUpdateCountResponse> List
		{
			get { return _List; }
			set { _List = value; }
		}

		public override object Payload
		{
			get { return _List; }
		}

	}
}
