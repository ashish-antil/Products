#region

using System;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.Linq;
using Imarda.Lib;

#endregion

namespace ImardaAttributingBusiness
{
    partial class ImardaAttributing
    {

        //#region GetAllAttributeValues
        //public GetListResponse<AttributeValue> GetAttributeValueList(IDRequest request)
        //{
        //    try
        //    {
        //        var response = GenericGetCachedEntityList<AttributeValue>(request);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorHandler.Handle<GetListResponse<AttributeValue>>(ex);
        //    }
        //}
        //#endregion

        //#region Get Attribute Values for the one entity type and entity GUID  e.g. "Unit",4ABB7A8D-30A3-4FC4-BA12-08CE46585F2B (so e.g. all attributes for a specific Unit)
        //public GetListResponse<AttributeValue> GetAttributeValueListByEntityType_EntityID(IDRequest request)
        //{
        //    try
        //    {
        //        return GenericGetCachedEntityListByParams<AttributeValue>("SPGetAttributeValueByEntityType_EntityID", false, request);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorHandler.Handle<GetListResponse<AttributeValue>>(ex);
        //    }
        //}
        //#endregion

        //#region GetAttributeValue
        //public AttributeValue GetAttributeValue(FullBusinessEntity entity, string attributeName)
        //{
        //    var entityTypeName = entity.GetType().Name;
        //    var request = new IDRequest(entity.ID, "EntityTypeName", entityTypeName, "VarName", attributeName);
        //    var response = GetAttributeValueByEntityType_EntityID_Name(request);
        //    return response.Item;
        //}
        //#endregion

        //#region Get AttributeValue by ID
        //public GetItemResponse<AttributeValue> GetAttributeValueByID(IDRequest request)
        //{
        //    try
        //    {
        //        //return GenericGetEntity<AttributeValue>(request);
        //        return GenericGetCachedEntity<AttributeValue>(request);
        //        //var response = GenericGetCachedEntityByParams<AttributeValue>("SPGetVehicleByName", false, request);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorHandler.Handle<GetItemResponse<AttributeValue>>(ex);
        //    }
        //}
        //#endregion

        //#region Get AttributeValue by EntityType, EntityID, Attribute name
        //public GetItemResponse<ImardaAttributingBusiness.AttributeValue> GetAttributeValueByEntityType_EntityID_Name(IDRequest request)
        //{
        //    try
        //    {
        //        return GenericGetCachedEntityByParams<AttributeValue>("SPGetAttributeValueByEntityType_EntityID_Name", false, request);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorHandler.Handle<GetItemResponse<AttributeValue>>(ex);
        //    }
        //}
        //#endregion

        // GenericGetCachedEntityByParams

        //public GetItemResponse<AttributeValue> GetAttributeValueByNameAndEntityType(IDRequest request)
        //{
        //    try
        //    {
        //        return ImardaDatabase.GetItem<AttributeValue>("SPGetAttributeValueByName", request.ID, request.GetString("VarName"));
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorHandler.Handle<GetItemResponse<AttributeValue>>(ex);
        //    }

        //}

        public BusinessMessageResponse SaveAttributeValueList(SaveListRequest<FernBusinessBase.AttributeValue> request)
        {
            var response = new BusinessMessageResponse();
            try
            {
                foreach (FernBusinessBase.AttributeValue item in request.List)
                {
                    AttributeValue attributeValue = ConvertToBusinessAttributeValue(item);
                    BaseEntity.ValidateThrow(attributeValue);

                    object[] properties = new object[]{			
							attributeValue.ID,
							attributeValue.AttributeID,
							attributeValue.EntityID,        //& gs-351
							attributeValue.Value,
							attributeValue.DateModified = DateTime.UtcNow,
							attributeValue.PrevValue,
							BusinessBase.ReadyDateForStorage(attributeValue.PrevDateModified),
							attributeValue.Deleted,
					};

                    response = GenericSaveEntity<AttributeValue>("AttributeValue", properties);
                    if (!response.Status) { break; }
                }
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveAttributeValue(SaveRequest<FernBusinessBase.AttributeValue> request)
        {
            var response = new BusinessMessageResponse();
            try
            {
                AttributeValue attributeValue = ConvertToBusinessAttributeValue(request.Item);
                BaseEntity.ValidateThrow(attributeValue);

                object[] properties = new object[]
                {
                    attributeValue.ID,
                    attributeValue.AttributeID,
                    attributeValue.EntityID, //& gs-351
                    attributeValue.Value,
                    attributeValue.DateModified = DateTime.UtcNow,
                    attributeValue.PrevValue,
                    BusinessBase.ReadyDateForStorage(attributeValue.PrevDateModified),
                    attributeValue.Deleted,
                };
                response = GenericSaveEntity<AttributeValue>("AttributeValue", properties);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
            return response;
        }

        public AttributeValue ConvertToBusinessAttributeValue(FernBusinessBase.AttributeValue av)
        {
            var busAv = new AttributeValue();
            busAv.ID = av.ID;
            busAv.CompanyID = av.CompanyID;
            busAv.UserID = av.UserID;
            busAv.DateCreated = av.DateCreated;
            busAv.DateModified = av.DateModified;
            busAv.Active = av.Active;
            busAv.Deleted = av.Deleted;
            busAv.AttributeID = av.AttributeID;
            busAv.EntityID = av.EntityID;
            busAv.Value = av.Value;
            busAv.PrevValue = av.PrevValue;
            busAv.PrevDateModified = av.PrevDateModified;
            busAv.VarName = av.VarName;
            busAv.FriendlyName = av.FriendlyName;
            busAv.Description = av.Description;
            busAv.GroupID = av.GroupID;
            busAv.VarType = av.VarType;
            busAv.Format = av.Format;
            busAv.CaptureHistory = av.CaptureHistory;
            busAv.Viewable = av.Viewable;
            busAv.EntityTypeID = av.EntityTypeID;
            busAv.EntityTypeName = av.EntityTypeName;
            return busAv;
        }
    }
}