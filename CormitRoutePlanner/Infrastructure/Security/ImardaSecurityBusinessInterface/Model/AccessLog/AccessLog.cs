using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness
{
	[DataContract]
	[Serializable]
	public class AccessLog : FullBusinessEntity
	{

		#region Instance Variables
		private DateTime _AccessDateTime;
		private Guid _SecurityEntityID;
		private Guid _SecurityObjectID;
		private bool _Success;
		private string _FailureMessage;
		#endregion

		#region Properties
		[DataMember]
		public DateTime AccessDateTime
		{
			get { return _AccessDateTime; }
			set { _AccessDateTime = value; }
		}

		[DataMember]
		public Guid SecurityEntityID
		{
			get { return _SecurityEntityID; }
			set { _SecurityEntityID = value; }
		}

		[DataMember]
		public Guid SecurityObjectID
		{
			get { return _SecurityObjectID; }
			set { _SecurityObjectID = value; }
		}

		[DataMember]
		public bool Success
		{
			get { return _Success; }
			set { _Success = value; }
		}

		[DataMember]
		public string FailureMessage
		{
			get { return _FailureMessage; }
			set { _FailureMessage = value; }
		}
		#endregion

		public AccessLog()
		{
		}

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);

			_AccessDateTime = GetDateTime(dr, "AccessDateTime");
			_SecurityEntityID = GetValue<Guid>(dr, "SecurityEntityID");
			_SecurityObjectID = GetValue<Guid>(dr, "SecurityObjectID");
			_Success = GetValue<bool>(dr, "Success");
			_FailureMessage = GetValue<string>(dr, "FailureMessage");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		
		}
	}
}
