using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.Serialization;
using FernBusinessBase;
using FernBusinessBase.Control;

namespace ImardaSecurityBusiness
{
	[DataContract]
	public class BillableFeature : FullBusinessEntity
	{
		public BillableFeature()
		{
		}

		[DataMember]
		public Guid FeatureID { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public int InstanceCount { get; set; }

		[DataMember]
		public bool Billable { get; set; }

		[DataMember]
		public decimal Price { get; set; }

		[DataMember]
		public int UnitCount { get; set; }


#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr) 
		{
			//base.AssignData(dr);
			FeatureID = GetValue<Guid>(dr, "FeatureID");
			Name = GetValue<string>(dr, "Name");
			Description = GetValue<string>(dr, "Description");
			InstanceCount = GetValue<int>(dr, "InstanceCount");
			Billable = GetValue<bool>(dr, "Billable");
			Price = GetValue<decimal>(dr, "Price");
			UnitCount = GetValue<int>(dr, "UnitCount");

#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}

	}
}