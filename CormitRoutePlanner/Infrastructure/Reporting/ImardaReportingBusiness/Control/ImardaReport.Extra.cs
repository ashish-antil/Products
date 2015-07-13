using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using ImardaReportBusiness.ReportingServiceWebReference;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Collections;
using Imarda.Lib;
using System.Data;
using System.Configuration;
using FernBusinessBase.Errors;

namespace ImardaReportBusiness
{
	partial class ImardaReport
	{
		private string _WebServiceUrl;
		private string _ViewingUrl;
		private string _User;
		private string _Password;
		private string _Domain;


		private const string TemplatesFolder = "/Templates";
		private const string InstancesFolder = "/Instances";

		private ReportingService2005 _Rs2005;

		///// <summary>
		///// 
		///// </summary>
		///// <param name="req">[0]=url, [1]=credential type, [2]=user, [3]=pwd, [4]=domain</param>
		///// <returns></returns>
		////[MethodMarker(Connect)]
		//public BusinessMessageResponse Connect(GenericRequest req)
		//{
		//  //[EntryMarker(Connect)]
		//  try
		//  {
		//    var p = req.Parameters;
		//    string url = GetValue(p, 0);
		//    string cred = GetValue(p, 1);
		//    string username = GetValue(p, 2);
		//    string password = GetValue(p, 3);
		//    string domain = GetValue(p, 4);
		//    _Rs2005 = new ReportingService2005(url);
		//    _Rs2005.Timeout = 300000;//300 secs = 5mins
		//    if (cred == "network")
		//    {
		//      _Rs2005.Credentials = new NetworkCredential(username, password, domain);
		//    }
		//    else
		//    {
		//      _Rs2005.Credentials = CredentialCache.DefaultCredentials;
		//    }
		//    _Rs2005.DeleteRole("nonexistent");
		//    return new BusinessMessageResponse { Status = true };
		//  }
		//  catch (Exception ex)
		//  {
		//    return new BusinessMessageResponse
		//      {
		//        Status = false,
		//        StatusMessage = ex.ToString()
		//      };
		//  }
		//}

		private static string GetValue(object[] par, int index)
		{
			if (index >= par.Length) return null;
			object o = par[index];
			if (o == null || string.Empty.Equals(o)) return null;
			if (o is string) return (string)o;
			return null;
		}

		/// <summary>
		/// Create a report type (template).
		/// [5] contains parameters of the format "key1|value1||key2|value2a|value2b|value2c||MyDataSource|/DataSources/Src1",
		/// where any key endingin "DataSource" adds a DataSourceReference to the report.
		/// </summary>
		/// <param name="req">[0]=companyID, [1]=userID, [2]=typeName, 
		/// [3]=version, [4]=RDL file path, [5]=parameters</param>
		/// <returns></returns>
		public GetItemResponse<ReportType> CreateReportType(GenericRequest req)
		{
			try
			{
				Initialize();
				object[] p = req.Parameters;
				DateTime now = DateTime.UtcNow;
				var typeFolder = (string)p[2];
				var reportTypeVersion = (string)p[3];
				var definition = (string)p[4];
				var reportType = new ReportType
				{
					ID = SequentialGuid.NewDbGuid(),
					CompanyID = (Guid)p[0],
					UserID = (Guid)p[1],
					Name = typeFolder,
					Version = reportTypeVersion,
				};
				_Log.InfoFormat("Create {0}", reportType);

				if (definition != null)
				{
					CreateFolder(typeFolder, TemplatesFolder);
					var tmplFolder = TemplatesFolder + '/';
					Warning[] warnings = _Rs2005.CreateReport(reportTypeVersion, tmplFolder + typeFolder, true, File.ReadAllBytes(definition), null);
					string reportPath = tmplFolder + typeFolder + '/' + reportTypeVersion;
					var reportParameters = (string)p[5];
					SetParameters(reportPath, reportParameters, Guid.Empty);
					SetDataSources(reportPath, reportParameters);
				}
				SaveReportType(new SaveRequest<ReportType> { Item = reportType });
				_Log.Info("..saved");
				return Success<ReportType>(reportType);
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("..error {0}", ex.Message);
				return ErrorHandler.Handle<GetItemResponse<ReportType>>(ex);
			}
		}

		/// <summary>
		/// Create a report type (template). Parameter [3] is the report type version, whereas [5] is the link version.
		/// [6] contains parameters of the format "key1|value1||key2|value2".
		/// </summary>
		/// <param name="req">[0]=companyID, [1]=userID, [2]=typeName, [3]=version,
		/// [4]=ownerID, [5]=reportNumber, [6]=parameters, [7]=description</param>
		/// <remarks>typeName e.g. "Location Report", "Travel Report"</remarks>
		/// <returns></returns>
		public GetItemResponse<ReportType> CreateLinkedReportType(GenericRequest req)
		{
			try
			{
				Initialize();
				object[] p = req.Parameters;
				var typeFolder = (string)p[2];
				var reportTypeVersion = (string)p[3];
				var ownerID = (Guid)p[4];
				var ownerIDString = ownerID.ToString();
				var reportNumber = (string)p[5];
				var reportParameters = (string)p[6];
				var reportType = new ReportType
				{
					ID = SequentialGuid.NewDbGuid(),
					CompanyID = (Guid)p[0],
					UserID = (Guid)p[1],
					Name = typeFolder + '_' + reportNumber,
					Version = reportNumber,
					OwnerID = ownerID,
					Description = (string)p[7]
				};
				_Log.InfoFormat("Create Linked {0}", reportType);

				CreateFolder(ownerIDString, InstancesFolder);
				string linkToPath = TemplatesFolder + '/' + typeFolder + '/' + reportTypeVersion;

				var instFolder = InstancesFolder + '/';
				string reportPath = instFolder + ownerIDString + '/' + reportType.Name;

				_Log.InfoFormat("..folders: link to [{0}], instance [{1}]", linkToPath, reportPath);

				Exception ex = Execute.Try(
					 false,
					 3, // #tries
					 ConfigUtils.GetTimeSpan("LinkedReportCreationRetryDelay", TimeSpan.FromSeconds(1)),
					 () => _Rs2005.CreateLinkedReport(reportType.Name, instFolder + ownerIDString, linkToPath, null)
				);
				if (ex == null)
				{
					SetParameters(reportPath, reportParameters, reportType.ID);
					_Log.InfoFormat("..parameters " + reportParameters);
					SaveReportType(new SaveRequest<ReportType> { Item = reportType });
					_Log.Info("..saved link");
					return Success(reportType);
				}
				return ErrorHandler.Handle<GetItemResponse<ReportType>>(ex);
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("..error {0}", ex.Message);
				return ErrorHandler.Handle<GetItemResponse<ReportType>>(ex);
			}

		}

		/// <summary>
		/// Create a snapshot for the given report type ID (should be a linked report).
		/// </summary>
		/// <param name="req">ID=report type ID, linked report </param>
		/// <returns></returns>
		public GetItemResponse<Report> CreateSnapshot(GenericRequest req)
		{
			try
			{
				Guid typeID = (Guid)req.ID;
				GetItemResponse<ReportType> rtResponse = GetReportType(new IDRequest(typeID));
				ReportType reportType = rtResponse.Item;
				string ownerIDString = reportType.OwnerID.ToString("D");

				Initialize();
				DateTime now = DateTime.UtcNow;
				var report = new Report
				{
					ID = SequentialGuid.NewDbGuid(),
					ReportTypeID = typeID,
					CompanyID = reportType.CompanyID,
					UserID = reportType.UserID,
					Expiry = now.AddYears(1),
				};
				string reportPath = InstancesFolder + '/' + ownerIDString + '/' + reportType.Name;
				_Log.InfoFormat("Create Snapshot {0} for [{1}]", report, reportPath);
				Warning[] warnings;
				string historyID = _Rs2005.CreateReportHistorySnapshot(reportPath, out warnings);
				report.SnapshotName = reportPath + '|' + historyID;
				// to be used as http://MSREPORTSVC/ReportServer?{reportPath}&rs:Snapshot={historyID}
				// caller should construct this URL
				SetAccessInformation(report);
				_Log.InfoFormat("..name [{0}]", report.SnapshotName);
				report.Status = ReportStatus.Rendered;
				SaveReport(new SaveRequest<Report> { Item = report });
				_Log.InfoFormat("..saved snapshot");
				return Success<Report>(report);
			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("..error {0}", ex.Message);
				return ErrorHandler.Handle<GetItemResponse<Report>>(ex);
			}
		}


		public GetItemResponse<Report> GetSnapshot(IDRequest request)
		{
			try
			{
				var resp = GetReport(request);
				_Log.InfoFormat("GetSnapshot: {0}", resp.Item);
				SetAccessInformation(resp.Item);
				return resp;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Report>>(ex);
			}
		}


		/// <summary>
		/// Get all report types owned by the given owner, based on the given report name.
		/// </summary>
		/// <param name="req">.ID=ownerID, .Parameters[0]=report name</param>
		/// <returns></returns>
		public GetListResponse<ReportType> GetOwnerReports(GenericRequest req)
		{
			try
			{
				var ownerID = req.ID;
				var p = req.Parameters;
				string reportName = (string)p[0];
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ReportType>());
				using (IDataReader dr = db.ExecuteDataReader("SPGetOwnerReports", ownerID, reportName))
				{
					var list = new List<ReportType>();
					while (dr.Read()) list.Add(GetFromData<ReportType>(dr));
					return SuccessList<ReportType>(list);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ReportType>>(ex);
			}
		}


		/// <summary>
		/// Get all report types from the given category
		/// </summary>
		/// <param name="req">.ID=ownerID, .Parameters[0]=report name</param>
		/// <returns></returns>
		public GetListResponse<ReportType> GetCategoryReports(GenericRequest req)
		{
			try
			{
				var catID = req.ID;
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ReportType>());
				using (IDataReader dr = db.ExecuteDataReader("SPGetReportTypesByCategory", catID))
				{
					var list = new List<ReportType>();
					while (dr.Read()) list.Add(GetFromData<ReportType>(dr));
					return SuccessList<ReportType>(list);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ReportType>>(ex);
			}
		}


		private static GetItemResponse<T> Success<T>(T item) where T : BaseEntity, new()
		{
			return new GetItemResponse<T>() { Item = item, Status = true };
		}


		private static GetListResponse<T> SuccessList<T>(List<T> list) where T : BaseEntity, new()
		{
			return new GetListResponse<T>() { List = list, Status = true };
		}


		private void Initialize()
		{
			if (_Rs2005 == null)
			{
				_Log.Info("Initialize RS2005");

				if (ConfigurationManager.ConnectionStrings.Count > 0)
				{
					InitAccessInformation();
					_Rs2005 = new ReportingService2005(_WebServiceUrl);
					_Rs2005.Credentials = new NetworkCredential(_User, _Password, _Domain);
					_Rs2005.Timeout = 300000;//300 secs = 5mins
					_Log.InfoFormat("User {0}, URL {1}", _User, _WebServiceUrl);
				}
			}
		}

		private void InitAccessInformation()
		{
			if (_WebServiceUrl == null)
			{
				_WebServiceUrl = ConfigUtils.GetString("ReportServer");
				_ViewingUrl = ConfigUtils.GetString("ReportBaseUrl");
				var credentials = ConfigUtils.GetString("ReportServerCredentials").KeyValueMap(ValueFormat.Strings, true);
				_User = (string)credentials["user"];
				_Password = AuthenticationHelper.Decrypt((string)credentials["password"], true, _User);
				_Domain = (string)credentials["domain"];
			}
		}

		private void SetAccessInformation(Report report)
		{
			InitAccessInformation();
			report.BaseUrl = _ViewingUrl;
			report.LoginUser = _User;
			report.Password = _Password;
			report.Domain = _Domain;
		}


		private string GetReportPath(Guid userID, ReportType reportType)
		{
			return GetFolder(userID) + '/' + reportType.ID;
		}

		private string GetFolder(Guid userID)
		{
			return '/' + userID.ToString("D");
		}

		/// <summary>
		/// Prepare for rendering: get parameters.
		/// </summary>
		/// <param name="reportPath"></param>
		/// <param name="report"></param>
		/// <param name="userParams">Format: "name1|value1||name2|value2a|value2b|value2c||name3||name4|value4"</param>
		private void SetParameters(string reportPath, string userParams, Guid reportID)
		{
			ReportParameter[] reportParams = _Rs2005.GetReportParameters(reportPath, null, true, null, null);
			var map = userParams.KeyValueMap(ValueFormat.Strings, true);
			foreach (ReportParameter rp in reportParams)
			{
				if (rp != null)
				{
					if (rp.Name == "ReportID")
						rp.DefaultValues = new string[] { reportID.ToString() };
					else
					{
						var value = (string)map[rp.Name];
						if (value != null) rp.DefaultValues = new string[] { value };
					}
				}
			}
			_Rs2005.SetReportParameters(reportPath, reportParams);
		}

		/// <summary>
		/// Set the data source references.
		/// </summary>
		/// <param name="reportPath">identifies the report type (not a linked report type)</param>
		/// <param name="userParams">key|value format e.g. "DataSource1|/DataSources/MyDataSource"</param>
		private void SetDataSources(string reportPath, string userParams)
		{
			var map = userParams.KeyValueMap(ValueFormat.Strings, true);
			var list = new List<DataSource>();
			foreach (string s in map.Keys)
			{
				if (s.Contains("DataSource"))
				{
					string val = (string)map[s];
					var ds = new DataSource { Name = s, Item = new DataSourceReference { Reference = val } };
					list.Add(ds);
				}
			}
			_Rs2005.SetItemDataSources(reportPath, list.ToArray());
		}


		private string WarningsToString(Warning[] warnings)
		{
			if (warnings == null || warnings.Length == 0) return null;
			return string.Join("; ", Array.ConvertAll<Warning, string>(warnings, w => w.Code + ": " + w.Message));
		}

		private bool CreateFolder(string folder, string parent)
		{
			try
			{
				_Rs2005.CreateFolder(folder, parent, null);
				return true;
			}
			catch
			{
				// folder already exists
				return false;
			}
		}
	}
}

