using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using System.Data;
using FernBusinessBase.Control;

namespace ImardaTrackingBusiness
{
	[DataContract]
	public class ServiceTrackable : FullBusinessEntity
	{
		#region Constructor
		public ServiceTrackable()
			: base()
		{
		}
		#endregion

		#region Properties
		[DataMember]
		public string DisplayName
		{ get; set; }

		[DataMember]
		public int Odometer
		{ get; set; }

		[DataMember]
		public int EngineHrs
		{ get; set; }

		[DataMember]
		public int InputDeviceHrs
		{ get; set; }
#if EntityProperty
		[DataMember]
		public `cstype` `field` { get; set; }
#endif
		
		#endregion

		#region Methods
		public override void AssignData(IDataReader dr) 
		{
			base.AssignData(dr);
			DisplayName = GetColumn<string>(dr, "DisplayName");
			int odo = 0;
			string odometer = dr["Odometer"].ToString();
			if (odometer.IndexOf('.') >= 0)
				int.TryParse(dr["Odometer"].ToString().Substring(0, odometer.IndexOf('.')), out odo);
			else
				int.TryParse(dr["Odometer"].ToString(), out odo);

			Odometer = odo;
			DataColumnCollection cols = dr.Table.Columns;
			if (HasColumn(dr, "EngineHrs")) EngineHrs = GetColumn<int>(dr, "EngineHrs");
			if (HasColumn(dr, "InputDeviceHrs")) InputDeviceHrs = GetColumn<int>(dr, "InputDeviceHrs");
#if EntityProperty_NoDate
			`field` = GetColumn<`cstype`>(dr, "`field`");
#endif
#if EntityProperty_Date
			`field` = GetDateTime(dr, "`field`");
#endif
		
		}

		#endregion

		public override string ToString()
		{
			return string.Format("ServiceTrackable({0}; {1};)", ID, DisplayName);
		}
	}
}