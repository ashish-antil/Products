using System;

namespace ImardaConfigurationBusiness.Model.ExtensionProfile
{
	public class ProfileConstants
	{
		public static string ReqParamOnlyDefault="OnlyDefault";

		//country for country specific profiles and providers
		public static Guid CountryIso = new Guid("F4C2295B-3CC4-4E10-B856-E41CDEB11BD6"); //Iso31661A2

		//configuration
		public static Guid FleetConfiguration = new Guid("DA722150-7955-469F-A8C2-188DFAD36103");
		public static Guid Configuration = new Guid("9786C0AF-CB88-4583-971F-3B99D3AF60F5");
		public static Guid DefaultConfiguration = new Guid("EF6E602E-058D-483E-80B6-E0AAB6B98953");
		public static Guid RoadTypeGroupConfiguration = new Guid("BFCC12FF-4701-4701-9674-8862CFAFB8ED");

		//Atom filtering
		public static Guid LowSpeedThreshold = new Guid("09A1DAF8-FE74-478D-A6C3-652EA7DA1DAF");
		public static Guid MaxHdop = new Guid("F7049991-002D-4E1B-A556-B554E804BE36");
		public static Guid MinNbSatellites = new Guid("83A03009-9527-4150-BD1A-8B2AC16A38B8");

		//provider and tables
		public static Guid Provider = new Guid("E2579144-555D-4F36-B66E-6A557006A61A");

		//unused RT and RTG all from two tables defined in code and not in config
		//public static Guid RoadTypesTable = new Guid("BCAF1405-2220-4BA2-913E-B833889B73E7");
		//public static Guid RoadTypeGroupsTable = new Guid("A59E46FE-4114-48D8-934C-47BBF75A5D09");

		//configuration parameters
		public static Guid RoadTypeAssignement = new Guid("56136C13-CA0D-47C2-A7FB-982959E35743");
		public static Guid DefaultRoadTypeGroup = new Guid("52067724-4EFE-4849-B677-82F766FF0957");
		public static Guid DefaultRoadTypeGroupId = new Guid("EF6E602E-058D-483E-80B6-E0AAB6B98953");//pseudo-ID for the default RTG entry - it does not point to an actual entry in the RTG table
		public static Guid DistanceBreachThresholds = new Guid("8F5EE3A1-8DFF-4A90-A6EC-CFCB4CD65888");

		//pseudo road types
		//public static Guid Unsealed = new Guid("09A2605C-56EF-4B5F-9DD9-5B42F5793913");
		//public static Guid FourWheelDrive = new Guid("47816413-8DF3-4CE4-8805-2C3E4D6B667B");
		public static int UnsealedRoadType = 99;
		public static int FourWheelDriveRoadType = 98;
	

	}
}
