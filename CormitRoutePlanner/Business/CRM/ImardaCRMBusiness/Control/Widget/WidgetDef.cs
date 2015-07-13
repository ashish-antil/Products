using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetListResponse<WidgetDef> GetWidgetDefList(IDRequest request)
		{
			try
			{
				//Guid companyID = new Guid(request["companyID"]);
				//Guid userID = new Guid(request["userID"]);
				return GenericGetEntityList<WidgetDef>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<WidgetDef>>(ex);
			}
		}

		public BusinessMessageResponse SaveWidgetDefList(SaveListRequest<WidgetDef> request)
		{
			//save to storage
			var response = new BusinessMessageResponse();
			try
			{
				foreach (WidgetDef entity in request.List)
				{
					BaseEntity.ValidateThrow(entity);
					object[] properties = new object[]
					{
						entity.ID,
						entity.Active,
						entity.Deleted,
						entity.DateCreated,
						entity.DateModified = DateTime.UtcNow,
						entity.CompanyID,
						entity.UserID,
						entity.Title,
						entity.Type,
						entity.Row,
						entity.Column,
						entity.RowSpan,
						entity.ColumnSpan
#if EntityProperty_NoDate
						,entity.`field`
#endif
#if EntityProperty_Date
						,BusinessBase.ReadyDateForStorage(entity.`field`)
#endif
					};
					response = GenericSaveEntity<WidgetDef>(entity.CompanyID, entity.Attributes, properties);   //Review IM-3747
				}
			}

			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}

	}
}
