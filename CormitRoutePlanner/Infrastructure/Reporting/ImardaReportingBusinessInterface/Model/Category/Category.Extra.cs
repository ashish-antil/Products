using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaReportBusiness 
{
	[DataContract]
	public class Category : FullBusinessEntity
	{
		#region Constructor
		public Category()
			: base()
		{
		}
		#endregion

		#region Properties
		[DataMember]
		public string Name
		{ get; set; }
		[DataMember]
		public string Description
		{ get; set; }
		[DataMember]
		public int SortIndex
		{ get; set; }
#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif
		
		#endregion

		#region Methods
		public override void AssignData(IDataReader dr) 
		{
			//TODO add the other fields to the database.
			ID = GetValue<Guid>(dr, "ID");
			Name = GetValue<string>(dr, "Name");
			Description = GetValue<string>(dr, "Description");
			SortIndex = GetValue<int>(dr, "SortIndex");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		
		}

		#endregion
	}
}
