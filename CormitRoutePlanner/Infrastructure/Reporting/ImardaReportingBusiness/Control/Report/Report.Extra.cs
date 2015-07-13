using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using System.Data;
using FernBusinessBase.Errors;
using ImardaReportBusiness;

namespace ImardaReportBusiness
{
	partial class ImardaReport
	{
		public GetListResponse<Report> GetReportListByReportTypeID(IDRequest request)
		{
			Initialize();
			var response = new GetListResponse<Report>();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Report>());
				object[] args = new object[] { request.ID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetReportListByReportTypeID", args))
				{
					response.List = new List<Report>();
					while (dr.Read())
					{
						var report = GetFromData<Report>(dr);
						SetAccessInformation(report);
						response.List.Add(report);
					}
					response.Status = true;
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Report>>(ex);
			}
		}

		public BusinessMessageResponse DeleteLinkedReports(IDRequest request)
		{
			try
			{
				Initialize();
				int countDB = 0;
				int countRS = 0;
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ReportType>());
				IDataReader dr;
				if (request.ID == Guid.Empty)
				{
					string name = request["name"];
					dr = db.ExecuteDataReader("GetReportsByNamePattern", name);
				}
				else
				{
					dr = db.ExecuteDataReader("SPGetReportType", request.ID);
				}
				using (dr)
				{
					while (dr.Read())
					{
						DeleteOneLinkedReport(db, dr, ref countDB, ref countRS);
					}
					return new BusinessMessageResponse { StatusMessage = string.Format("{0}|{1}", countDB, countRS) };
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}


		private void DeleteOneLinkedReport(ImardaDatabase db, IDataReader dr, ref int countDB, ref int countRS)
		{
			var rt = new ReportType();
			rt.AssignData(dr);
			countDB += db.ExecuteNonQuery("SPDeleteReportType", rt.ID); // includes child records from the Report table
			string path = InstancesFolder + '/' + rt.OwnerID.ToString().ToLower() + '/' + rt.Name;
			try
			{
				_Rs2005.DeleteItem(path);
				countRS++;
			}
			catch (Exception ex)
			{
				ErrorHandler.HandleInternal(ex);
			}
		}



	}
}

