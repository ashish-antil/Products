/***********************************************************************
Auto Generated Code

Generated by   : PROLIFICXNZ\maurice.verheijen
Date Generated : 24/06/2009 2:48 p.m.
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;


namespace ImardaReportBusiness
{
	partial class ImardaReport
	{

		#region Get Report
		public GetItemResponse<Report> GetReport(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Report>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Report>>(ex);
			}
		}
		#endregion
		#region GetReportUpdateCount
		

		public GetUpdateCountResponse GetReportUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();

			try
			{
				response = GenericGetEntityUpdateCount<Report>("Report", request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}

			return response;
		}
		#endregion
		#region GetReportListByTimeStamp
		

		public GetListResponse<Report> GetReportListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Report>("Report", request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Report>>(ex);
			}
		}
		#endregion
		#region GetReportList
		

		public GetListResponse<Report> GetReportList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<Report>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Report>>(ex);
			}
		}
		#endregion
		#region Save Report
		public BusinessMessageResponse SaveReport(SaveRequest<Report> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				Report report = request.Item;
				object[] properties = new object[]{			report.ID,
						report.ReportTypeID,
						report.ReportNumber,
						report.SnapshotName,
						report.ScheduledTaskID,
						report.Expiry,
						report.Notes,
						report.Status,
						report.CompanyID,
						report.UserID,
						report.DateCreated,
						report.DateModified,
						report.Deleted,
						report.Active
				};
				response = GenericSaveEntity<Report>(report.CompanyID, report.Attributes, properties);    //Review IM-3747
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region SaveReportList
		

		public BusinessMessageResponse SaveReportList(SaveListRequest<Report> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Report report in request.List)
				{
					object[] properties = new object[]
					{
						report.ID,
						report.ReportTypeID,
						report.ReportNumber,
						report.SnapshotName,
						report.ScheduledTaskID,
						report.Expiry,
						report.Notes,
						report.Status,
						report.CompanyID,
						report.UserID,
						report.DateCreated,
						report.DateModified,
						report.Deleted,
						report.Active

					};
                    response = GenericSaveEntity<Report>(report.CompanyID, report.Attributes, properties);    //Review IM-3747
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region Delete Report
		

		public BusinessMessageResponse DeleteReport(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Report>("Report", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
	}
}


