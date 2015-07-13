using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaCRMBusiness 
{
	[DataContract]
	public class CustomerCommunicationNode : FullBusinessEntity 
	{			
		#region Constructor	
		public CustomerCommunicationNode()
		{
		}
		#endregion

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public Guid ParentID { get; set; }


		public override void AssignData(IDataReader dr)
		{
			base.AssignData(dr);
			Name = DatabaseSafeCast.Cast<string>(dr["Name"]);
			ParentID = DatabaseSafeCast.Cast<Guid>(dr["ParentID"]);
		}

	}
}
