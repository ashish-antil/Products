using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		#region GetAccessLogUpdateCount
		

		public GetUpdateCountResponse GetAccessLogUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();
			try
			{
				response = GenericGetEntityUpdateCount<AccessLog>(request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
			return response;
		}
		#endregion

		#region GetAccessLogListByTimeStamp
		

		public GetListResponse<AccessLog> GetAccessLogListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<AccessLog>(request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<AccessLog>>(ex);
			}
		}
		#endregion

		#region Get AccessLog list
		public GetListResponse<AccessLog> GetAccessLogList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<AccessLog>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<AccessLog>>(ex);
			}
		} 
		#endregion

		#region Save Access LOG
		

		public BusinessMessageResponse SaveAccessLog(SaveRequest<AccessLog> request)
		{

			var response = new BusinessMessageResponse();

			try
			{
				AccessLog log = request.Item;
				if (log != null)
				{
					response = GenericSaveEntity<AccessLog>(request.CompanyID, log.Attributes, null); //Review IM-3747

					if (!response.Status)
						throw new ApplicationException(response.StatusMessage);
				}
				response.Status = true;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}

			return response;
		} 
		#endregion

		#region Save Access Log List
		

		public BusinessMessageResponse SaveAccessLogList(SaveListRequest<AccessLog> request)
		{

			var response = new BusinessMessageResponse();

			try
			{
				foreach (AccessLog log in request.List)
				{
					response = GenericSaveEntity<AccessLog>(request.CompanyID, log.Attributes, null); //Review IM-3747

					if (!response.Status)
						throw new ApplicationException(response.StatusMessage);
				}
				response.Status = true;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}

			return response;
		} 
		#endregion

		#region Delete Access Log
		

		public BusinessMessageResponse DeleteAccessLog(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<AccessLog>("AccessLog", new object[] { request.ID });
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		} 
		#endregion
	}
}
