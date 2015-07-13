#region

using System.ServiceModel;

#endregion

// ReSharper disable once CheckNamespace
namespace FernBusinessBase
{
	/// <summary>
	///     Encapsulates a request to pass an object
	/// </summary>
	[MessageContract]
	public class ObjectRequest : ParameterMessageBase
	{
		public ObjectRequest()
		{
		}

		public ObjectRequest(object content)
		{
			Content = content;
		}

		[MessageBodyMember]
		public object Content { get; set; }
	}
}