using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Reflection;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to save the given Item to the database.
	/// </summary>
	/// <typeparam name="T">The Type of the item that needs to be saved.</typeparam>
	[MessageContract]
    public class SaveRequest<T> : ParameterMessageBase
		where T : BaseEntity, new()
	{
		private T _Item;

		public SaveRequest()
		{
		}

		public SaveRequest(T item)
		{
			Item = item;
		}

		/// <summary>
		/// Get/set the item.
		/// </summary>
		[MessageBodyMember]
		public T Item
		{
			get { return _Item; }
			set
			{
				_Item = value;
				_Item.PrepareForSave();
			}
		}

		public string[] Validate(bool all)
		{
			var entity = Item as FullBusinessEntity;
			if (entity != null) return entity.Validate(all);
			return null;
		}
	}
}
