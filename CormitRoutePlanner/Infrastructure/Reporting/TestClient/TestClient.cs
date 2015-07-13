/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\maurice.verheijen
Date Generated : 24/06/2009 2:48 p.m.
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FernBusinessBase;
using ImardaReportBusiness;
using System.Diagnostics;
using System.IO;
namespace ImardaTestClient
{

	[TestFixture]
	public partial class TestClient
	{
		Guid _CompanyID = Guid.NewGuid();
		[SetUp]
		public void Init()
		{

		}
		private void initFullEntity(FullBusinessEntity entity, Guid id)
		{
			entity.ID = id;
			entity.UserID = Guid.NewGuid();
			entity.Active = true;
			entity.DateCreated = DateTime.Now;
			entity.DateModified = DateTime.Now;
			entity.CompanyID = _CompanyID;
			entity.Deleted = false;
		}

		[Test]
		public void TestCreateReportType()
		{
			//GetItemResponse<ReportType> CreateReportType(GenericRequest req);

			var companyID = new Guid("e649e5e1-98c4-4601-a83c-4657028d0e17");
			var userID = new Guid("1b477f9a-1170-4a53-af28-12eec6fb1310");

			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				var parameters = new ParamsForCreateReportType
				{
					CompanyID = companyID,
					UserID = userID,
					Name = "Travel",
					Version = "1.1",
					Definition = new FileInfo(@"C:\TeamImarda\Imarda360\Imarda360.Reports\SmartTrack_Report\vehicle\Location Report.rdl"),
					DataSourceReferences = "DataSource1|/Data Sources/TrackingDataSource"
				};
				

				GetItemResponse<ReportType> resp = channel.CreateReportType(parameters.AsGenericRequest());
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.Item != null);
			}

		}


		[Test]
		public void TestCreateReportTypeForExisting()
		{
			//GetItemResponse<ReportType> CreateReportType(GenericRequest req);

			var companyID = new Guid("e649e5e1-98c4-4601-a83c-4657028d0e17");
			var userID = new Guid("1b477f9a-1170-4a53-af28-12eec6fb1310");

			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				var parameters = new ParamsForCreateReportType
				{
					CompanyID = companyID,
					UserID = userID,
					Name = "Travel",
					Version = "1",
				};

				GetItemResponse<ReportType> resp = channel.CreateReportType(parameters.AsGenericRequest());
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.Item != null);
			}

		}

		//[Test][Ignore]
		//public void CreateRecordsForExisting()
		//{
		//  var companyID = new Guid("e649e5e1-98c4-4601-a83c-4657028d0e17");
		//  var userID = new Guid("1b477f9a-1170-4a53-af28-12eec6fb1310");

		//  IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
		//  var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC		//	string[] arr = { "Breach", "Load Switch Recalibration", "Log Off Not Complete", "Outstanding Safety Checklist",
		// 				  "Unload Outside Geofence", "Daily Utilisation", "Over Speed", "Top Speed", "Total Distance",
		// 				  "Visited Streets", "Weekend Movement", "Weekly Utilisation", "Geofence Summary",
		// 				  "Geofence Travel", "GeoToGeo Summary"};
		//	foreach (string name in arr)
		//	{
		// 	 var parameters = new ParamsForCreateReportType
		// 	 {
		// 	   CompanyID = companyID,
		// 	   UserID = userID,
		// 	   Name = name,
		// 	   Version = "1",
		// 	 };

		// 	 GetItemResponse<ReportType> resp = channel.CreateReportType(parameters.AsGenericRequest());
		// 	 if (resp.Status)
		// 	 {
		// 	   Console.WriteLine(resp.Item.Name + " " + resp.Item.ID.ToString("D"));
		// 	 }
		//	}
		//  }

		//}


		[Test]
		public void TestCreateLinkedReportType()
		{
			//GetItemResponse<ReportType> CreateLinkedReportType(GenericRequest req);

			var companyID = new Guid("e649e5e1-98c4-4601-a83c-4657028d0e17");
			var userID = new Guid("1b477f9a-1170-4a53-af28-12eec6fb1310");
			var ownerID = companyID;

			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				var parameters = new ParamsForCreateLinkedReportType
				{
					CompanyID = companyID,
					UserID = userID,
					Name = "Travel",
					Version = "1",
					OwnerID = ownerID,
					ReportParameters = "hello|world"
				};

				GetItemResponse<ReportType> resp = channel.CreateLinkedReportType(parameters.AsGenericRequest());
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.Item != null);
			}
		}

		[Test]
		public void TestCreateSnapshot()
		{
			//GetItemResponse<Report> CreateSnapshot(GenericRequest req);
			//Guid rtID = new Guid("FFF2F38A-E5C3-4BA5-B9BA-977543C89123");
			Guid rtID = new Guid("51eda16f-724b-466e-82ad-85b2f2104fc6");
			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				var req = new GenericRequest(rtID);
				GetItemResponse<Report> resp = channel.CreateSnapshot(req);
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.Item != null);
				var report = resp.Item;
				Assert.AreEqual("http://MSREPORTSVC/ReportServer", report.BaseUrl);
				string pattern = "yyyy-MM-ddTHH:mm:ss";
				string path, historyID;
				report.GetPathAndHistoryID(out path, out historyID);
				Assert.AreEqual(pattern.Length, historyID.Length);

				Assert.AreEqual("/Instances/08c46d66-b886-44d0-a3c2-3aa9b12c4d98/Travel", path);
				Assert.LessOrEqual(DateTime.Now.ToString(pattern), historyID);
				Assert.AreEqual("administrator", report.LoginUser.ToLower());
				Assert.AreEqual("imarda1234", report.Password);
				Assert.IsNull(report.Domain);
			}
		}

		[Test]
		public void TestGetSnapshot()
		{
			Guid reportID = new Guid("534405B2-C217-4AD8-BB8A-A14B108E9C4d");
			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				GetItemResponse<Report> resp = channel.GetSnapshot(new IDRequest(reportID));
				Console.WriteLine(resp);
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.Item != null);
				var report = resp.Item;

				string expectedPath = "/Instances/08c46d66-b886-44d0-a3c2-3aa9b12c4d98/Travel";
				string expectedHistoryID = "2009-07-09T17:30:06";

				Assert.AreEqual("http://MSREPORTSVC/ReportServer", report.BaseUrl);
				string path, historyID;
				report.GetPathAndHistoryID(out path, out historyID);
				Assert.AreEqual(expectedPath, path);
				Assert.AreEqual(expectedHistoryID, historyID);
				Assert.AreEqual("administrator", report.LoginUser.ToLower());
				Assert.AreEqual("imarda1234", report.Password);
				Assert.IsNull(report.Domain);
			}
		}


		[Test]
		public void TestCreateTypeAndSnapshot()
		{
			var companyID = new Guid("e649e5e1-98c4-4601-a83c-4657028d0e17");
			var userID = new Guid("1b477f9a-1170-4a53-af28-12eec6fb1310");
			var ownerID = companyID;

			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				var parameters = new ParamsForCreateLinkedReportType
				{
					CompanyID = companyID,
					UserID = userID,
					Name = "Travel",
					Version = "1",
					OwnerID = ownerID,
					ReportParameters = "hello|world"
				};

				GetItemResponse<ReportType> resp1 = channel.CreateLinkedReportType(parameters.AsGenericRequest());
				Assert.IsTrue(resp1 != null);
				Assert.IsTrue(resp1.Status);
				Assert.IsTrue(resp1.Item != null);

				Guid rtID = resp1.Item.ID;
				var req = new GenericRequest(rtID);
				GetItemResponse<Report> resp2 = channel.CreateSnapshot(req);
				Assert.IsTrue(resp2 != null);
				Assert.IsTrue(resp2.Status);
				Assert.IsTrue(resp2.Item != null);
				var report = resp2.Item;
				string url = report.GetReportURL("60.234.77.199");
				Process.Start(url);
			}
		}

		[Test]
		public void TestGetOwnerReports()
		{
			Guid ownerID = new Guid("e649e5e1-98c4-4601-a83c-4657028d0e17");
			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				var req = new GenericRequest(ownerID, "%");
				GetListResponse<ReportType> resp = channel.GetOwnerReports(req);
				Console.WriteLine(resp);
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.List != null);
				Assert.IsTrue(resp.List.Count > 0);
				foreach (ReportType rt in resp.List) Console.WriteLine("{0}: '{1}' {2}", rt.ID, rt.Name, rt.OwnerID);
			}
		}


		[Test]
		public void TestGetCategoryReports()
		{
			Guid catID = new Guid("209ab46d-b226-4936-a6a5-e854fde802f5");
			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				var req = new GenericRequest(catID);
				GetListResponse<ReportType> resp = channel.GetCategoryReports(req);
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.List != null);
				foreach (ReportType rt in resp.List) Console.WriteLine("{0}: '{1}' {2}", rt.ID, rt.Name, rt.OwnerID);
			}
		}

		[Test]
		public void TestGetCategories()
		{
			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				GetListResponse<Category> resp = channel.GetCategoryList();
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.List != null);
				Assert.IsTrue(resp.List.Count > 0);
				foreach (Category cat in resp.List) Console.WriteLine("{0}: '{1}' {2}", cat.ID, cat.Name, cat.Description);
			}
		}


		[Test]
		public void ReportTypeSelect()
		{
			GetItemResponse<ImardaReportBusiness.ReportType> resp = null;

			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				IDRequest request = new IDRequest(new Guid("ac92e162-f8bd-4535-b5b2-0edef1cced13"));
				resp = channel.GetReportType(request);
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.Item != null);
				Assert.IsTrue(resp.Item is ReportType);
				var rt = (ReportType)resp.Item;
				Assert.AreEqual("Hello World", rt.Name);
			}
		}

		[Test]
		public void TestAllSnapshotsOfOneLinkedReportType()
		{
			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				Guid rtid = new Guid("7D33BC7E-0A76-46FF-842C-4B3EB5BCC8CC");
				GetListResponse<Report> resp = channel.GetReportListByReportTypeID(new IDRequest(rtid));
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.List != null);
				Assert.IsTrue(resp.List.Count > 0);
				foreach (Report r in resp.List)
				{
					Assert.IsNotNull(r.BaseUrl);
					Assert.IsNotNull(r.LoginUser);
					Assert.IsNotNull(r.Password);
					Console.WriteLine("{0}: {1}", r.ID, r.GetReportURL("60.234.77.199"));
				}
			}

		}

#if implemented

		[Test]
		public void NormalManyToMany()
		{
			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				Guid id = new Guid("b746b336-bd93-475e-9fac-c1ab96058f8f");
				GetListResponse<Category> resp = channel.GetCategoryListByReportTypeID(new IDRequest(id));
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.List != null);
				Assert.IsTrue(resp.List.Count > 0);
				foreach (Category entity in resp.List)
				{
					Assert.IsNotNull(entity.Name);
					Console.WriteLine(entity.Name);
				}
			}
		}

		[Test]
		public void InverseManyToMany()
		{
			IImardaReport channel = ImardaProxyManager.Instance.IImardaReportProxy;
			var service = ImardaProxyManager.Instance.${Proxy};
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IC				Guid id = new Guid("209ab46d-b226-4936-a6a5-e854fde802f5");
				GetListResponse<ReportType> resp = channel.GetReportTypeListByCategoryID(new IDRequest(id));
				Assert.IsTrue(resp != null);
				Assert.IsTrue(resp.Status);
				Assert.IsTrue(resp.List != null);
				Assert.IsTrue(resp.List.Count > 0);
				foreach (ReportType entity in resp.List)
				{
					Assert.IsNotNull(entity.Name);
					Console.WriteLine(entity.Name);
				}
			}
		}

#endif
	}
}


