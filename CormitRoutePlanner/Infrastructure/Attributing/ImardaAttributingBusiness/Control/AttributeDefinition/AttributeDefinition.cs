#region

using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

#endregion

namespace ImardaAttributingBusiness
{
    partial class ImardaAttributing
    {

        #region Get AttributeDefinition

        public GetItemResponse<AttributeDefinition> GetAttributeDefinition(IDRequest request)
        {
            try
            {
                return GenericGetEntity<AttributeDefinition>(request);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<AttributeDefinition>>(ex);
            }
        }

        #endregion

        public GetListResponse<AttributeDefinition> GetAttributeDefintionListWithHistoryByCompanyID(IDRequest request)
        {
            try
            {
                return ImardaDatabase.GetList<AttributeDefinition>("SPGetAttributeDefintionListWithHistoryByCompanyID", request.ID);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<AttributeDefinition>>(ex);
            }

        }

        #region Save AttributeDefinition
        public BusinessMessageResponse SaveAttributeDefinition(SaveRequest<FernBusinessBase.AttributeDefinition> request)
        {
            var ad = request.Item;

            object[] adProperties = new object[]
            {
                ad.ID,
                ad.CompanyID,
                ad.UserID,
                ad.DateCreated,
                ad.DateModified,
                ad.Active,
                ad.Deleted,
                ad.VarName,
                ad.FriendlyName,
                ad.Description,
                ad.GroupID,
                ad.VarType,
                ad.Format,
                ad.CaptureHistory,
                ad.Viewable,
                ad.EntityTypeName
            };
            return GenericSaveEntity<AttributeDefinition>("AttributeDefinition", adProperties);
        }

        #endregion

        public BusinessMessageResponse SaveAttributeDefinitionList(SaveListRequest<FernBusinessBase.AttributeDefinition> request)
        {
            var response = new BusinessMessageResponse();
            try
            {
                foreach (FernBusinessBase.AttributeDefinition item in request.List)
                {
                    AttributeDefinition ad = ConvertToBusinessAttributeDefinition(item);
                    BaseEntity.ValidateThrow(ad);

                    object[] adProperties = new object[]{			
							ad.ID,
                            ad.CompanyID,
                            ad.UserID,
                            ad.DateCreated,
                            ad.DateModified,
                            ad.Active,
                            ad.Deleted,
                            ad.VarName,
                            ad.FriendlyName,
                            ad.Description,
                            ad.GroupID,
                            ad.VarType,
                            ad.Format,
                            ad.CaptureHistory,
                            ad.Viewable,
                            ad.EntityTypeName
					};

                    response = GenericSaveEntity<AttributeDefinition>("AttributeDefinition", adProperties);
                    if (!response.Status) { break; }
                }
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public AttributeDefinition ConvertToBusinessAttributeDefinition(FernBusinessBase.AttributeDefinition ad)
        {
            var busAd = new AttributeDefinition();
            busAd.ID = ad.ID;
            busAd.CompanyID = ad.CompanyID;
            busAd.UserID = ad.UserID;
            busAd.DateCreated = ad.DateCreated;
            busAd.DateModified = ad.DateModified;
            busAd.Active = ad.Active;
            busAd.Deleted = ad.Deleted;
            busAd.VarName = ad.VarName;
            busAd.FriendlyName = ad.FriendlyName;
            busAd.Description = ad.Description;
            busAd.GroupID = ad.GroupID;
            busAd.VarType = ad.VarType;
            busAd.Format = ad.Format;
            busAd.CaptureHistory = ad.CaptureHistory;
            busAd.Viewable = ad.Viewable;
            busAd.EntityTypeName = ad.EntityTypeName;
            busAd.EntityTypeID = ad.EntityTypeID;
            return busAd;
        }

        public BusinessMessageResponse DeleteAttributeDefinition(IDRequest request)
        {
            var response = new BusinessMessageResponse();
            try
            {
                var entityName = request.GetString("EntityName");
                var attribName = request.GetString("AttributeName");
                return GenericDeleteEntity<AttributeDefinition>("AttributeDefinition", request.ID, entityName, attribName);
            }
            catch (Exception ex)
            {
                response.Status = false;
                response.ErrorCode = ex.StackTrace + ", " + ex.Message;
                response.StatusMessage = ex.StackTrace + ", " + ex.Message;
                return response;
            }
        }
    }
}