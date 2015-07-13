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
	public class Asset : FullBusinessEntity
	{
		public Asset()
		{
		}

		public const int DescriptionMaxLen = 50;
		[ValidLength(-1, DescriptionMaxLen)]
		[DataMember]
		public string Description { get; set; }

		public const int AccessCodeMaxLen = 8;
		[ValidLength(-1, AccessCodeMaxLen)]
		[DataMember]
		public string AccessCode { get; set; }

		public const int UserMaxLen = 50;
		[ValidLength(0, UserMaxLen)]
		[DataMember]
		public string User { get; set; }

		public const int EmailMaxLen = 250;
		[ValidLength(0, EmailMaxLen)]
		[DataMember]
		public string Email { get; set; }

		public const int PasswordHashMaxLen = 100;
		[ValidLength(0, PasswordHashMaxLen)]
		[DataMember]
		public string PasswordHash { get; set; }

		public const int MobilePhoneMaxLen = 20;
		[ValidLength(0, MobilePhoneMaxLen)]
		[DataMember]
		public string MobilePhone { get; set; }

		[DataMember]
		public Guid Salt { get; set; }

		[DataMember]
		public double Lat { get; set; }

		[DataMember]
		public double Lon { get; set; }
#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif

		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			Description = GetValue<string>(dr, "Description");
			AccessCode = GetValue<string>(dr, "AccessCode");
			User = GetValue<string>(dr, "User");
			Email = GetValue<string>(dr, "Email");
			PasswordHash = GetValue<string>(dr, "PasswordHash");
			MobilePhone = GetValue<string>(dr, "MobilePhone");
			Salt = GetValue<Guid>(dr, "Salt");
			Lat = (double)GetValue<decimal>(dr, "Lat");
			Lon = (double)GetValue<decimal>(dr, "Lon");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif


#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		}

	}
}