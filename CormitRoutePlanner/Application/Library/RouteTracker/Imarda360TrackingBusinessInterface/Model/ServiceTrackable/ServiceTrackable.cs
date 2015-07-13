using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using FernBusinessBase;
using System.Data;
using Imarda360Base;
using Imarda.Lib;

namespace Imarda360Application.Tracking
{
	[DataContract]
	public class ServiceTrackable : SolutionEntity
	{
		#region Constructor
		public ServiceTrackable()
		{
		}

		public ServiceTrackable(ImardaTrackingBusiness.ServiceTrackable trackable)
		{
			ID = trackable.ID;
			DisplayName = trackable.DisplayName;
			Odometer = trackable.Odometer;
			EngineHrs = trackable.EngineHrs;
			InputDeviceHrs = trackable.InputDeviceHrs;
			Active = trackable.Active;
			Deleted = trackable.Deleted;
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

		#endregion

		#region Methods

		#endregion


		public override string ToString()
		{
			return string.Format("ServiceTrackable({0}; {1})", DisplayName, ID);
		}

	}
}