using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace FernBusinessBase
{
	[MessageContract]
    public class SaveListRequest<T> : ParameterMessageBase where T : BaseEntity, new()
	{
		public SaveListRequest() {}

		public SaveListRequest(List<T> list)
		{
			List = list;
		}


		private List<T> _List;

		[MessageBodyMember]
		public List<T> List
		{
			get { return _List; }
			set
			{
				_List = value;
				foreach (T item in _List) item.PrepareForSave();
			}
		}

		/// <summary>
		/// Validate all objects in the list.
		/// </summary>
		/// <param name="all">true to check all properties, false to return immediately on the first error</param>
		/// <returns>null = OK, or string[] with errors (may have duplicates)</returns>
		public string[] Validate(bool all)
		{
			if (List == null) return null;
			var errors = new List<string>();
			foreach (T item in List)
			{
				var entity = item as FullBusinessEntity;
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
