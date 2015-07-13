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

		#region Get ReportType
		public GetItemResponse<ReportType> GetReportType(IDRequest request)
		{
			try
			{
				return GenericGetEntity<ReportType>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<ReportType>>(ex);
			}
		}
		#endregion
		#region GetReportTypeUpdateCount
		

		public GetUpdateCountResponse GetReportTypeUpdateCount(GetUpdateCountRequest request)
		{

			try
			{
				GetUpdateCountResponse response = GenericGetEntityUpdateCount<ReportType>("ReportType", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}
		#endregion
		#region GetReportTypeListByTimeStamp
		

		public GetListResponse<ReportType> GetReportTypeListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<ReportType>("ReportType", request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ReportType>>(ex);
			}
		}
		#endregion
		#region GetReportTypeList
		

		public GetListResponse<ReportType> GetReportTypeList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<ReportType>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ReportType>>(ex);
			}
		}
		#endregion
		#region Save ReportType
		public BusinessMessageResponse SaveReportType(SaveRequest<ReportType> request)
		{
			try
			{
				ReportType reportType = request.Item;
				object[] properties = new object[]{			
					reportType.ID,
					reportType.Name,
					reportType.Version,
					reportType.Description,
					reportType.InputFormUrl,
					reportType.IsTemplate,
					reportType.OwnerID,
					reportType.Delivery,
					reportType.ExpiryRule,
					reportType.CompanyID,
					reportType.UserID,
					reportType.DateCreated,
					reportType.DateModified,
					reportType.Deleted,
					reportType.Active
				};
                BusinessMessageResponse response = GenericSaveEntity<ReportType>(reportType.CompanyID, reportType.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
		#region SaveReportTypeList
		

		public BusinessMessageResponse SaveReportTypeList(SaveListRequest<ReportType> request)
		{
			try
			{
				BusinessMessageResponse response = null;
				foreach (ReportType reportType in request.List)
				{
					object[] properties = new object[]
					{
						reportType.ID,
						reportType.Name,
						reportType.Version,
						reportType.Description,
						reportType.InputFormUrl,
						reportType.IsTemplate,
						reportType.OwnerID,
						reportType.Delivery,
						reportType.ExpiryRule,
						reportType.CompanyID,
						reportType.UserID,
						reportType.DateCreated,
						reportType.DateModified,
						reportType.Deleted,
						reportType.Active

					};
                    response = GenericSaveEntity<ReportType>(reportType.CompanyID, reportType.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
		#region Delete ReportType
		

		public BusinessMessageResponse DeleteReportType(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<ReportType>("ReportType", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
	}
}

