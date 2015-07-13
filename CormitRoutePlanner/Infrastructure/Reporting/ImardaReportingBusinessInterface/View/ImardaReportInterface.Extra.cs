using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using System.IO;
using System.Collections;


namespace ImardaReportBusiness
{
	public static class ReportStatus
	{
		public const byte Planned = 0; // record created, but nothing scheduled or generated yet
		public const byte Scheduled = 1;
		public const byte Rendered = 2;
		public const byte Delivered = 3;
		public const byte Error = 99;
	}
	public static class ReportTypeID
	{		
		public static Guid GetReportID(string name)
		{
			Guid id = Guid.Empty;
			#region Vehicle Management
			if (name.Equals("Travel", StringComparison.CurrentCultureIgnoreCase  ) )
				id = new Guid("B529ADBF-2957-4256-9EFF-BB036DA3DF85");
			else if (name.Equals("Sensor", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("ADA11881-4C4E-49CB-B8AE-28F69263606E");
			else if (name.Equals("Location", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("DF0AAC6F-F850-432F-B725-316A58E2A2FA");
			else if (name.Equals("TripSummary", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("8590E872-A242-4038-99E7-8FB4CDB5CB85");
			else if (name.Equals("IdleTime", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("DD238271-2C59-4FEE-B921-AD9542FC49AE");
			else if (name.Equals("VisitedSuburb", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("CE21571A-05B3-4964-A610-C9FEF3286F21");
			else if (name.Equals("Geofence", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("7BF255CE-EC39-4239-9FDF-EE03E86C8285");
			else if (name.Equals("UnncessaryIdle", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("FFB02C3F-D725-4CD4-97B7-3F8F17B16B4D");
				
			#endregion
			#region Driver
			else if (name.Equals("FatigueManagement", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("794BC482-2830-4F41-A580-2D354106CA7C");
			else if (name.Equals("FatigueSummary", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("2D3C652A-FB22-440A-8F0C-342D30481B14");
			else if (name.Equals("DriverLogin", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("6E4F97EA-19AB-4BDB-A74D-371606E8F3C7");
			else if (name.Equals("DriverActivity", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("07A2D9D0-92B6-4485-A02F-765C0C3869B7");
			#endregion
			#region Geofence
			else if (name.Equals("GeofenceSummary", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("F8CB48BC-92C0-45d1-9FD1-966999A23654");
			else if (name.Equals("GeofenceTravel", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("ACE769C6-7B99-401c-9722-3A96CEDF0B81");
			else if (name.Equals("GeoToGeoSummary", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("26A3ABDE-8C2E-405f-8025-9AB05C38CDEC");
			#endregion
			#region Fleet
			else if (name.Equals("DailyUtilisation", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("89395232-35A0-47e7-A8CA-7B09B08DAEC7");
			else if (name.Equals("OverSpeed", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("5AE20535-A45F-473a-AEB4-C660B6C73442");
			else if (name.Equals("TopSpeed", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("F2D69CA9-1EB5-4966-BACC-B6E480F1C730");
			else if (name.Equals("TotalDistance", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("D69A7630-1582-433d-AF08-D8E7088B421D");
			else if (name.Equals("VisitedStreets", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("66A14DF4-543E-4be9-882F-3E266732B8EE");
			else if (name.Equals("WeeklyUtilisation", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("0320FC48-1D90-437d-91C7-3E79337324C5");
			#endregion
			return id;
		}										
	}
	public static class CategoryID
	{
		public static Guid GetCategoryID(string name)
		{
			Guid id = Guid.Empty;
			
			if (name.Equals("Vehicle", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("209AB46D-B226-4936-A6A5-E854FDE802F5");
			else if (name.Equals("Geofence", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("4DC4A4E7-9F49-43A9-823D-D458363AA8D9");
			else if (name.Equals("Fleet", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("13D7B7F9-F853-4CE8-8F43-CE934FDE8549");
			else if (name.Equals("Driver", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("3B458BB4-7C5D-45E7-8179-735EF2BEE7BE");
			else if (name.Equals("Custom", StringComparison.CurrentCultureIgnoreCase))
				id = new Guid("7DF6CE84-5B02-4F3A-A8F0-58DF63A7C6DC");						
			return id;
		}
	}
	partial interface IImardaReport
	{
		//[OperationContract]
		//BusinessMessageResponse Connect(GenericRequest req);

		[OperationContract]
		GetItemResponse<ReportType> CreateReportType(GenericRequest req);

		[OperationContract]
		GetItemResponse<ReportType> CreateLinkedReportType(GenericRequest req);

		[OperationContract]
		GetItemResponse<Report> CreateSnapshot(GenericRequest req);

		[OperationContract]
		GetItemResponse<Report> GetSnapshot(IDRequest request);

		[OperationContract]
		GetListResponse<ReportType> GetOwnerReports(GenericRequest req);

		[OperationContract]
		GetListResponse<ReportType> GetCategoryReports(GenericRequest req);

	}

	/// <summary>
	/// Helper class to convert the parameters into a GenericRequest to pass to <code>CreateReportType</code>.
	/// </summary>
	/// <example>channel.CreateReportType(new ParamsForCreateReportType { Name="XYZ Report", Version="1.1" ...}.AsGenericRequest()</example>
	public class ParamsForCreateReportType
	{
		public Guid CompanyID { set; private get; }
		public Guid UserID { set; private get; }
		public string Name { set; private get; }
		public string Version { set; private get; }
		public FileInfo Definition { set; private get; }
		public string ReportParameters { set; private get; }
		public string DataSourceReferences { set; private get; }

		private string AllParameters()
		{
			return string.Format("{0}||{1}", ReportParameters, DataSourceReferences).Trim('|');
		}

		public GenericRequest AsGenericRequest()
		{
			return new GenericRequest(
				Guid.Empty,
				CompanyID,
				UserID,
				Name,
				Version,
				Definition.FullName,
				AllParameters()
			);
		}
	}

	public class ParamsForCreateLinkedReportType
	{
		public Guid CompanyID { set; private get; } 
		public Guid UserID { set; private get; }
		public string Name { set; private get; }
		public string Version { set; private get; }
		public Guid OwnerID { set; private get; }   
		public string ReportNumber { set; private get; }
		public string ReportParameters { set; private get; }
		public string Description { set; private get; }

		public GenericRequest AsGenericRequest()
		{
			//ID=empty, [0]=companyID, [1]=userID, [2]=typeName, [3]=version, [4]=ownerID, [5]=report#, [6]=parameters</param>
			return new GenericRequest(
				Guid.Empty,
				CompanyID, 
				UserID,	
				Name,
				Version,
				OwnerID,   
				ReportNumber ?? DateTime.UtcNow.ToString("yyMMdd'T'HHmmssfff"),
				ReportParameters,
				Description
			);
		}

	}
}

