/***********************************************************************
Auto Generated Code.

Generated by   : IMARDAINC\Qian.Chen
Date Generated : 12/03/2010 12:10 p.m.
Copyright (c)2009 CodeGenerator 1.2
***********************************************************************/
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
	public class NotificationPlan : FullBusinessEntity, IComparable<NotificationPlan> 
	{				
		#region Constructor
		public NotificationPlan()
		{
		}
		#endregion
		
		#region Properties

		public const int NameMaxLen = 50;
		[ValidLength(0, NameMaxLen)]
		[DataMember]
		public string Name { get; set; }

		public const int DescriptionMaxLen = 200;
		[ValidLength(-1, DescriptionMaxLen)]
		[DataMember]
		public string Description { get; set; }
#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif
		

		#endregion

		#region Methods
		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			Name = GetValue<string>(dr, "Name");
			Description = GetValue<string>(dr, "Description");
#if EntityProperty_NoDate
			`field` = GetValue<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		
		}

		#endregion

		#region IComparable<NotificationPlan> Members

		public int CompareTo(NotificationPlan other)
		{
			return other == null ? 1 : string.CompareOrdinal(Name, other.Name);
		}

		#endregion
	}
}

