using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Imarda360Base
{
	[MessageContract]
	public class Get360ListResponse<T> : SolutionMessageResponse where T : SolutionEntity, new()
	{
		private List<T> _List;

		public Get360ListResponse()
		{
			_List = new List<T>();
		}

		public Get360ListResponse(List<T> list)
		{
			_List = list;
		}

		[MessageBodyMember]
		public List<T> List
		{
			get { return _List; }
			set { _List = value; }
		}

		public override object Payload
		{
			get { return _List; }
		}

		public override string ToString()
		{
			if (_List == null) return typeof(T).Name + ": null list";
			return typeof(T).Name + " " + _List.Count + " items";
		}

	}
}
