using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FernBusinessBase.Model
{
	/// <summary>
	/// This class is a helper class for passing multiple conditions to generate sql command on the fly
	/// </summary>
	[DataContract]
	[Serializable]
	public class SingleArgCondition
	{
		#region Instant Variables
		private string _PropertyName;
		private SearchOperation _Operation;
		private object _value;
		#endregion

		#region Constructor
		public SingleArgCondition()
		{
		}

		public SingleArgCondition(string properyName, SearchOperation operation, object value)
		{
			_PropertyName = properyName;
			_Operation = operation;
			_value = value;
		}
		#endregion

		#region Properties
		[DataMember]
		public string PropertyName
		{
			get { return _PropertyName; }
			set { _PropertyName = value; }
		}

		[DataMember]
		public SearchOperation Operation
		{
			get { return _Operation; }
			set { _Operation = value; }
		}

		[DataMember]
		public object Value
		{
			get { return _value; }
			set { _value = value; }
		}
		#endregion
	}
}
