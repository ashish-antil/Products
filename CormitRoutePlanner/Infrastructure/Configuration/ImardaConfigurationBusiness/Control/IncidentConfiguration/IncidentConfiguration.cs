using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaConfigurationBusiness
{
	partial class ImardaConfiguration
	{
		public GetItemResponse<IncidentConfiguration> GetIncidentConfiguration(IDRequest request)
		{
			try
			{
				return GenericGetEntity<IncidentConfiguration>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<IncidentConfiguration>>(ex);
			}
		}

		public GetUpdateCountResponse GetIncidentConfigurationUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<IncidentConfiguration>("IncidentConfiguration", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<IncidentConfiguration> GetIncidentConfigurationListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<IncidentConfiguration>("IncidentConfiguration", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<IncidentConfiguration>>(ex);
			}
		}

		public GetListResponse<IncidentConfiguration> GetIncidentConfigurationList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<IncidentConfiguration>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<IncidentConfiguration>>(ex);
			}
		}
		
		public BusinessMessageResponse SaveIncidentConfiguration(SaveRequest<IncidentConfiguration> request)
		{
			try
			{
				IncidentConfiguration entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,entity.Name
						,entity.Description
						,entity.DeviceType
						,entity.Filename
						,entity.FileID

#if EntityProperty_NoDate
						,entity.`field`
#endif

						,BusinessBase.ReadyDateForStorage(entity.FileCreationDate)
						,BusinessBase.ReadyDateForStorage(entity.FileModificationDate)

#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
				var response = GenericSaveEntity<IncidentConfiguration>(entity.CompanyID, entity.Attributes, properties);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveIncidentConfigurationList(SaveListRequest<IncidentConfiguration> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (IncidentConfiguration entity in request.List)
				{
					var properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted
						,entity.Name
						,entity.Description
						,entity.DeviceType
						,entity.Filename
						,entity.FileID

#if EntityProperty_NoDate
						,entity.`field`
#endif

						,BusinessBase.ReadyDateForStorage(entity.FileCreationDate)
						,BusinessBase.ReadyDateForStorage(entity.FileModificationDate)

#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<IncidentConfiguration>(entity.CompanyID, entity.Attributes, properties);
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteIncidentConfiguration(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<IncidentConfiguration>("IncidentConfiguration", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public SimpleResponse<bool> CheckIfUniqueForIncidentConfiguration(IDRequest request)
		{
			var response = new SimpleResponse<bool>();
			try
			{
				var fieldName = request["FieldName"];
				var value = request["Value"];
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<IncidentConfiguration>());
				string query = string.Format("SELECT COUNT(*) FROM IncidentConfiguration WHERE {0} = '{1}' AND ID <> '{2}' AND Deleted = 0", fieldName, value, request.ID);
				using (IDataReader dr = db.ExecuteDataReader(CommandType.Text, query))
				{
					if (dr.Read() && Convert.ToInt32(dr[0]) > 0)
						response = new SimpleResponse<bool>(false);
					else
						response = new SimpleResponse<bool>(true);
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<bool>>(ex);
			}
		}


	}
}