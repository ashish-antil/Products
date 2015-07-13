using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace ImardaSecurityBusiness
{
	[DataContract]
	public class Application
	{

		#region Instance Variables
		private Guid _ID;
		private string _Name;
		private bool _Active;
		private bool _Deleted;
		private DateTime _Updated;
		#endregion

		public Application()
		{
		}

		#region Properties
		[DataMember]
		public DateTime Updated
		{
			get { return _Updated; }
			set { _Updated = value; }
		}

		[DataMember]
		public bool Deleted
		{
			get { return _Deleted; }
			set { _Deleted = value; }
		}

		[DataMember]
		public bool Active
		{
			get { return _Active; }
			set { _Active = value; }
		}

		[DataMember]
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

		[DataMember]
		public Guid ID
		{
			get { return _ID; }
			set { _ID = value; }
		}
		#endregion

	}
}
