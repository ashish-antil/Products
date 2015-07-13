#region

using System.ServiceModel;
using Imarda.Lib;

#endregion

namespace FernBusinessBase
{
    /// <summary>
    ///     Encapsulates a response from the server with a single item.
    /// </summary>
    /// <typeparam name="T">The type of the item that's returned.</typeparam>
    [MessageContract]
	public class GetItemResponse<T> : BusinessMessageResponse 
        where T : BaseEntity, new()
	{
		public GetItemResponse() {} 

		public GetItemResponse(T item)
		{
			_item = item;
		}

		private T _item;

		[MessageBodyMember]
		public T Item
		{
			get { return _item; }
			set { _item = value; }
		}

		public override object Payload
		{
			get { return _item; }
		}

		public override string ToString()
		{
			string detail = _item == null ? typeof (T).Name + ": null" : _item.ToString();
			return ServiceMessageHelper.ToString(this, detail);
		}
	}
}
