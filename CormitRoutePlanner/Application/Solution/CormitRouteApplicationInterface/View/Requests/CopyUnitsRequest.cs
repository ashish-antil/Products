using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using FernBusinessBase;

namespace Imarda360Application
{
	[Flags]
	[DataContract]
	public enum UnitCopyFlags
	{
		[EnumMember]
		CreateMatchingVehicles = 1,
		[EnumMember]
		AutoAssignUnitsToVehicles = 2,
		[EnumMember]
		ActivateUnits = 4,
		[EnumMember]
		ActivateVehicles = 8,
	}

	[MessageContract]
	[Serializable]
	public class CopyUnitsRequest : IRequestBase
	{
		[MessageBodyMember]
		public Guid TemplateUnitID { get; set; }

		[MessageBodyMember]
		public Guid FleetID { get; set; }

		private Guid _SessionID;

		[MessageBodyMember]
		public UnitCopyFlags Options { get; set; }

		public bool Has(UnitCopyFlags flag)
		{
			return (Options & flag) != 0;
		}
	
		[MessageBodyMember]
		public List<string> TrackIDs { get; set; }

		public CopyUnitsRequest()
		{
		}

		public CopyUnitsRequest(Guid unitID, Guid fleetID, UnitCopyFlags options, List<string> trackIDs)
		{
			TemplateUnitID = unitID;
			FleetID = fleetID;
			Options = options;
			TrackIDs = trackIDs;
		}


		[MessageBodyMember]
		public Guid SessionID
		{
			get { return _SessionID; }
			set { _SessionID = value; }
		}

		public object SID
		{
			set { _SessionID = new Guid(value.ToString()); }
		}

		/// <summary>
		/// Local use only. Not transported across services.
		/// </summary>
		public object DebugInfo { get; set; }
	}
}
