using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Imarda360Base
{
	/// <summary>
	/// Encapsulates a response from the server with a single item.
	/// </summary>
	/// <typeparam name="T">The type of the item that's returned.</typeparam>
	[MessageContract]
	public class Get360ItemResponse<T> : SolutionMessageResponse where T : SolutionEntity, new()
	{
		public Get360ItemResponse() 
		{
		}

		public Get360ItemResponse(T item)
		{
			_Item = item;
		}

		private T _Item;

		[MessageBodyMember]
		public T Item
		{
			get { return _Item; }
			set { _Item = value; }
		}

		public override object Payload
		{
			get { return _Item; }
		}

		public override string ToString()
		{
			if (_Item == null) return typeof(T).Name + ": null item";
			return _Item.ToString();
		}
	}
}
