using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Imarda.Lib;
using System.Linq;

namespace FernBusinessBase
{
	/// <summary>
	/// Encapsulates a request to perform some operation, whose function or target will
	/// differ based on the given ID (eg. get an object with the given ID)
	/// </summary>
	[MessageContract]
	[Serializable]
	public class IDListRequest : ParameterMessageBase, IWithOptions, IEnumerable<Guid>
	{
		[MessageBodyMember]
		private readonly List<Guid> _IDList;

		[MessageBodyMember]
		public RetrievalOptions Options { get; set; }

		/// <summary>
		/// Check if all options included.
		/// </summary>
		/// <param name="options">one or more EntityOptions flags</param>
		/// <returns>true if all the passed flags are set</returns>
		public bool HasAll(RetrievalOptions options)
		{
			return (Options & options) == options;
		}

		/// <summary>
		/// Check if one or more options included.
		/// </summary>
		/// <param name="options">one or more EntityOptions flags</param>
		/// <returns>true if one or more of the flags are set</returns>
		public bool HasSome(RetrievalOptions options)
		{
			return (Options & options) != 0;
		}


		/// <summary>
		/// Empty request, e.g. when only Parameters are needed
		/// </summary>
		public IDListRequest()
		{
			_IDList = new List<Guid>();
		}

		/// <summary>
		/// Create a request initialised with multiple IDs. You can add more IDs with Add().
		/// </summary>
		/// <param name="ids">collection of ids</param>
		public IDListRequest(IEnumerable<Guid> ids)
			: this()
		{
			_IDList.AddRange(ids);
		}

		/// <summary>
		/// Add one id to the list.
		/// </summary>
		/// <param name="id">id</param>
		public void Add(Guid id)
		{
			_IDList.Add(id);
		}

		/// <summary>
		/// Use the string returned by this to pass to Stored Procedures that use SplitGuid function.
		/// </summary>
		/// <returns></returns>
		public string AsStoredProcParameter()
		{
			return string.Concat(_IDList.Select(id => id.ToString()).ToArray());
		}


		/// <summary>
		/// Allow the use of the IDListRequest like: foreach (Guid g in myIDListRequest) { ... }.
		/// Also allows use in LINQ queries.
		/// </summary>
		/// <returns></returns>
		public IEnumerator<Guid> GetEnumerator()
		{
			return _IDList.GetEnumerator();
		}

		/// <summary>
		/// Retruns a copy of the list as an array.
		/// </summary>
		/// <returns></returns>
		public Guid[] ToArray()
		{
			return _IDList.ToArray();
		}

		public override string ToString()
		{
			return string.Format("IDListRequest(count {0}, {1} params, opt {2}, CompID={3})",
				_IDList != null ? _IDList.Count : -1, (HasParameters ? Parameters.Count : 0), Options, CompanyID.ShortString());
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
