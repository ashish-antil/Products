#region

using System.Collections.Generic;
using System.ServiceModel;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
	[MessageContract]
	public class SimpleListRequest<T> : ParameterMessageBase where T : BaseEntity, new()
	{
		public SimpleListRequest()
		{
		}

		public SimpleListRequest(List<T> list)
		{
			List = list;
		}

		[MessageBodyMember]
		public List<T> List { get; set; }
	}
}