using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Reflection;
using Imarda360Base;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to save the given Item to the database.
	/// </summary>
	/// <typeparam name="T">The Type of the item that needs to be saved.</typeparam>
	[MessageContract]
	public class Save360Request<T> : ParameterMessageBase
		where T : SolutionEntity, new()
	{
		private T _Item;

		public Save360Request()
		{
		}

		public Save360Request(T item)
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
			}
		}

		public override string ToString()
		{
			return string.Format("{0}({1})", GetType().Name, typeof(T).Name);
		}

		public string[] Validate(bool all)
		{
			var entity = Item as SolutionEntity;
			if (entity != null) return entity.Validate(all);
			return null;
		}

	}
}
