using System;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.Collections.Generic;
using System.Data;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetItemResponse<Contact> GetContact(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Contact>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Contact>>(ex);
			}
		}

		public GetUpdateCountResponse GetContactUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<Contact>("Contact", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<Contact> GetContactListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Contact>("Contact", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		}
		//get contact list by type
		public GetListResponse<Contact> GetContactList(IDRequest request)
		{
			var response = new GetListResponse<Contact>();
			response.List = new List<Contact>();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Contact>());
				string spName = "SPGetContactList";
				byte type = 0;
				if (request.ContainsKey("Type")) type = byte.Parse(request["Type"]);
				int size = 0;
				if (request.ContainsKey("TopN")) int.TryParse(request["TopN"], out size);
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				Guid id = request.ID;
				var args = new object[] { includeInactive, id, type };
				using (IDataReader dr = db.ExecuteDataReader(spName, args))
				{
					while (dr.Read()) response.List.Add(GetFromData<Contact>(dr));
					if (size > 0 && response.List.Count > size)
					{
						response.List.RemoveRange(size - 1, response.List.Count - size);
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		}

		public GetListResponse<Contact> GetContactExtent(GetFilteredExtentRequest req)
		{
			try
			{
				var response = GenericGetExtent<Contact>(
							req.CompanyID,
							req.CreatedAfter, req.CreatedBefore,
							req.ModifiedAfter, req.ModifiedBefore,
							req.Deleted, req.Active, req.Template, req.Path,
							req.Limit, req.Offset, req.SortColumns,
							null, null);		 			 
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
			}
		}

		//# gs-103
		public BusinessMessageResponse SaveContact(SaveRequest<Contact> request)
		{
			try
			{
				Contact entity = request.Item;
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.Path,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.Code
						,entity.Name
						,entity.FirstName
						,entity.LastName
						,entity.ContactPersonID
						,entity.StreetAddress
						,entity.Suburb
						,entity.City
						,entity.Postcode
						,entity.State
						,entity.Country  

						,entity.PostalAddress
						,entity.PostalCity  
						,entity.PostalPostCode
						,entity.PostalState 
						,entity.PostalCountry

						,entity.Phone
						,entity.DirectPhone
						,entity.Mobile
						,entity.Fax
						,entity.Email
						,entity.Website
						,entity.SocialNetwork

						,entity.JobTitle
						,entity.Type
						,entity.ClientType
						,entity.Description

						,entity.IsPerson
						,entity.IsReseller

						,entity.SourceType
						,entity.SourceID
						,entity.StateID

						,entity.LastContactDate
						,entity.CreatedByID
						,entity.LastUpdateByID

						,entity.DefaultLocationID
#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<Contact>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		//. gs-103

		//# gs-103
		public BusinessMessageResponse SaveContactList(SaveListRequest<Contact> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Contact entity in request.List)
				{
					var properties = new object[]
					{
						entity.ID,
						entity.CompanyID,
						entity.Path,		//& gs-351
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.Code
						,entity.Name
						,entity.FirstName
						,entity.LastName
						,entity.ContactPersonID
						,entity.StreetAddress
						,entity.Suburb
						,entity.City
						,entity.Postcode
						,entity.State
						,entity.Country  

						,entity.PostalAddress
						,entity.PostalCity  
						,entity.PostalPostCode
						,entity.PostalState 
						,entity.PostalCountry

						,entity.Phone
						,entity.DirectPhone
						,entity.Mobile
						,entity.Fax
						,entity.Email
						,entity.Website
						,entity.SocialNetwork

						,entity.JobTitle
						,entity.Type
						,entity.ClientType
						,entity.Description

						,entity.IsPerson
						,entity.IsReseller

						,entity.SourceType
						,entity.SourceID
						,entity.StateID

						,entity.LastContactDate
						,entity.CreatedByID
						,entity.LastUpdateByID

						,entity.DefaultLocationID
#if EntityProperty_NoDate
						,entity.`field`
#endif


#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<Contact>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		//. gs-103

		public BusinessMessageResponse DeleteContact(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Contact>("Contact", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
