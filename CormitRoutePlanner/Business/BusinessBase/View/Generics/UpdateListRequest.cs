using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace FernBusinessBase
{
	[MessageContract]
    public class UpdateListRequest<T> : ParameterMessageBase 
		where T : BaseEntity, new()
	{
		public UpdateListRequest() { }

		public UpdateListRequest(List<T> add, List<Guid> delete)
		{
			_ToBeAdded = add;
			ToBeDeleted = delete;
		}

		private List<T> _ToBeAdded;

		[MessageBodyMember]
		public List<T> ToBeAdded
		{
			get { return _ToBeAdded; }
			set
			{
				_ToBeAdded = value;
				foreach (T item in _ToBeAdded) item.PrepareForSave();
			}
		}

		[MessageBodyMember]
		public List<Guid> ToBeDeleted { get; set; }
	}
}
