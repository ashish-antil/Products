using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		public GetItemResponse<Client> GetClient(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Client>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Client>>(ex);
			}
		}

		public GetUpdateCountResponse GetClientUpdateCount(GetUpdateCountRequest request)
		{
			try
			{
				var response = GenericGetEntityUpdateCount<Client>("Client", request.TimeStamp, true, request.ID, request.LastRecordID);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}
		}

		public GetListResponse<Client> GetClientListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Client>("Client", request.TimeStamp, request.Cap, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Client>>(ex);
			}
		}

		public GetListResponse<Client> GetClientList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<Client>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Client>>(ex);
			}
		}

		#region GetClientExtent
		#region SQL Build Helpers
		private string GetClientInnerSelectSQL(int scope, Guid companyID)
		{
			var innerSelect = string.Format(
				"SELECT c.* " +
				"FROM Client c " +
				"WHERE " +
				//"  ( c.CompanyID = '{1}' " +
				//"    OR " +
				//"    (c.CompanyID = '11111111-1111-1111-1111-111111111111' AND '{1}' = '78c46d66-b886-44d0-a3c2-3aa9b12c4d98') " +
				//"  ) " +
				"c.Active = 1 AND c.Deleted=0 ", scope, companyID.ToString());
			return innerSelect;
		}

		#endregion

		public GetListResponse<Client> GetClientExtent(GetFilteredExtentRequest request)
		{
			var scope = 0;
			if (request.Scope.HasValue)
				scope = request.Scope.Value;
			//note! Scope is currently not used
			var innerSelect = GetClientInnerSelectSQL(scope, request.CompanyID);

			try
			{
				var response = GenericGetExtentWithCustomSelect<Client>(
							request.CompanyID,
							request.CreatedAfter, request.CreatedBefore,
							request.ModifiedAfter, request.ModifiedBefore,
							request.Deleted, request.Active, request.Template, request.Path,
							request.Limit, request.Offset, request.SortColumns,
							null, null,
							innerSelect, request.Conditions, request.LogicalOperator);
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Client>>(ex);
			}

		}
		#endregion
		
		public BusinessMessageResponse SaveClient(SaveRequest<Client> request)
		{
			try
			{
				Client entity = request.Item; 			   
				var properties = new object[]
				{
						entity.ID,
						entity.CompanyID,
						entity.UserID,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.Active,
						entity.Deleted

						,entity.ClientId
						,entity.ClientSecret
						,entity.ClientName
						,entity.ClientType
						,entity.ClientUri
						,entity.ContactInfo
						,entity.Legal
						,entity.Notes
						,entity.SupportedFlows
						,entity.ConsentQuery
						,entity.TokenSigning
						,entity.RegisteredUris
						,entity.AccessTokenType
						,entity.AccessTokenExpiration
						,entity.AuthorizationCodeExpiration
						,entity.IdentityTokenExpiration
						,entity.IssueRefreshToken
						,entity.RefreshTokenUsage
						,entity.RefreshTokenExpiration
						,entity.AbsoluteRefreshTokenExpiration
						,entity.SlidingRefreshTokenExpiration

#if EntityProperty_NoDate
						,entity.`field`
#endif

						,BusinessBase.ReadyDateForStorage(entity.ValidFrom)
						,BusinessBase.ReadyDateForStorage(entity.ValidUntil)

#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                var response = GenericSaveEntity<Client>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse SaveClientList(SaveListRequest<Client> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Client entity in request.List)
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
						,entity.ClientId
						,entity.ClientSecret
						,entity.ClientName
						,entity.ClientType
						,entity.ClientUri
						,entity.ContactInfo
						,entity.Legal
						,entity.Notes
						,entity.SupportedFlows
						,entity.ConsentQuery
						,entity.TokenSigning
						,entity.RegisteredUris
						,entity.AccessTokenType
						,entity.AccessTokenExpiration
						,entity.AuthorizationCodeExpiration
						,entity.IdentityTokenExpiration
						,entity.IssueRefreshToken
						,entity.RefreshTokenUsage
						,entity.RefreshTokenExpiration
						,entity.AbsoluteRefreshTokenExpiration
						,entity.SlidingRefreshTokenExpiration

#if EntityProperty_NoDate
						,entity.`field`
#endif

						,BusinessBase.ReadyDateForStorage(entity.ValidFrom)
						,BusinessBase.ReadyDateForStorage(entity.ValidUntil)

#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
                    response = GenericSaveEntity<Client>(entity.CompanyID, entity.Attributes, properties);    //Review IM-3747
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}

		public BusinessMessageResponse DeleteClient(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Client>("Client", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}