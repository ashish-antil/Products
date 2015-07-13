using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	[MessageContract]
	public class GetUpdateCountRequestList
	{
		private List<GetUpdateCountRequest> _List;

		public GetUpdateCountRequestList()
		{
			_List = new List<GetUpdateCountRequest>();
		}

		[MessageBodyMember]
		public List<GetUpdateCountRequest> List
		{
			get { return _List; }
			set { _List = value; }
		}

	}
}
