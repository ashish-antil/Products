/***********************************************************************
Auto Generated Code

Generated by   : adam-Laptop\adam
Date Generated : 28/04/2009 12:33 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using System.Data;
using FernBusinessBase.Control;

namespace Imarda360Application.CRM
{
	[DataContract]
	public class Task : FullBusinessEntity 
	{				
		#region Constructor
		public Task():base()
		{
		}
		#endregion
		
		#region Properties
		[DataMember]
		public string Description 
		{get ;set ;}
#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif
		
		#endregion

		#region Methods
		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			Description = GetColumn<string>(dr, "Description");
#if EntityProperty_NoDate
			`field` = GetColumn<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		

		}

		#endregion

	
	}
}

