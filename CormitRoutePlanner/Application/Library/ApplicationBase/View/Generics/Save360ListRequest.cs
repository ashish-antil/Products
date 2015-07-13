using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Reflection;
using Imarda360Base;
using System.Globalization;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to save the given Item to the database.
	/// </summary>
	/// <typeparam name="T">The Type of the item that needs to be saved.</typeparam>
	[MessageContract]
	public class Save360ListRequest<T> : ParameterMessageBase
		where T : SolutionEntity, new()
	{
		private List<T> _List;

		public Save360ListRequest()
		{
		}

		public Save360ListRequest(List<T> list)
		{
			List = list;
		}

		/// <summary>
		/// Get/set the list.
		/// </summary>
		[MessageBodyMember]
		public List<T> List
		{
			get { return _List; }
			set
			{
				_List = value;
			}
		}

		public override string ToString()
		{
			return string.Format("{0}({1}:{2})", GetType().Name, typeof (T).Name, (_List == null ? "null" : _List.Count.ToString(CultureInfo.InvariantCulture)));
		}

		public string[] Validate(bool all)
		{
			if (List == null) return null;
			var errors = new List<string>();
			foreach (T item in List)
			{
				var entity = item as SolutionEntity;
				if (entity != null)
				{
					string[] e = entity.Validate(all);
					if (e == null) continue;
					if (all) return e;
					errors.AddRange(e);
				}
			}
			return errors.Count == 0 ? null : errors.ToArray();
		}

	}
}
