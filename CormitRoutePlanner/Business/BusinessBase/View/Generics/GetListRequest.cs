using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	/// <summary>
	/// Request that contains a list of Type+ID combinations of objects to be retrieved.
	/// </summary>
	[MessageContract]
	[Serializable]
    public class GetListRequest : ParameterMessageBase, IWithOptions
	{
		public GetListRequest()
		{
		}

		public GetListRequest(params EntityTypeAndID[] arr)
		{
			TypeAndIDs = arr;
		}

		public GetListRequest(RetrievalOptions options)
		{
			Options = options;
		}

		[MessageBodyMember]
		public EntityTypeAndID[] TypeAndIDs { get; set; }


		#region IWithOptions Members

		public RetrievalOptions Options { get; set; }

		public bool HasAll(RetrievalOptions options)
		{
		  return (Options & options) == options;
		}

		public bool HasSome(RetrievalOptions options)
		{
		  return (Options & options) != 0;
		}

		#endregion
	}
}
