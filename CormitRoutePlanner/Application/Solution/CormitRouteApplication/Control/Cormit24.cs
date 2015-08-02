using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cormit.Application.RouteApplication.Security;
using Cormit.Application.Tracking;
using CormitRouteTrackingApplicationLib.Tracking;
using FernBusinessBase;
using FernBusinessBase.Errors;
using FernBusinessBase.Extensions;
using FernBusinessBase.View.Generics;
using Imarda.Lib;
using Imarda360Application.CRM;
using Imarda360Base;

using System.Globalization;
using ImardaCRMBusiness;
using ImardaSecurityBusiness;
using ConfigUtils = Imarda.Lib.ConfigUtils;
using IImardaCRM = Imarda360Application.CRM.IImardaCRM;
using IImardaSecurity = Cormit.Application.RouteApplication.Security.IImardaSecurity;


// ReSharper disable once CheckNamespace
namespace Cormit.Application.RouteApplication
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public class Cormit24 : BusinessBase, ICormit24
	{
		public static readonly Guid InstanceID = SequentialGuid.NewDbGuid();
		public const string Description = "C24 Application";

		//private static readonly ErrorLogger _Log = ErrorLogger.GetLogger("i360");


		private readonly MethodCallLogger _CallLogger;
        private readonly SessionObjectCache _SessionManager;
        private readonly ICormitRouteTracking _Tracking;
		private readonly IImardaSolution _Solution;
	    private readonly IImardaCRM _CRM;
        private readonly IImardaSecurity _Security;

		public Cormit24()
		{
            _SessionManager = SessionObjectCache.Instance;
			_Solution = FacadeImplementation.Instance;
            _Tracking = CormitRouteTracking.Instance;
		    _CRM = ImardaCRM.Instance;
		    _Security = ImardaSecurity.Instance;
		    //_CallLogger = new MethodCallLogger(_Log);

		}

		private void LogResult(IRequestBase request, object response)
		{
			_CallLogger.LogResult(request, response);
		}


		private void Log(string name, IRequestBase request)
		{
			_CallLogger.LogCall(name, request);
		}


		

		private static void ValidateRequest<T>(Save360Request<T> request)
			where T : SolutionEntity, new()
		{
			string[] errors = request.Validate(true);
			if (errors == null)
			{
				return;
			}
			throw new ValidationException(errors);
		}

		private static void ValidateRequest<T>(SaveRequest<T> request)
			where T : BaseEntity, new()
		{
			string[] errors = request.Validate(true);
			if (errors == null)
			{
				return;
			}
			throw new ValidationException(errors);
		}

		private static void ValidateRequest<T>(SaveListRequest<T> request)
			where T : BaseEntity, new()
		{
			string[] errors = request.Validate(true);
			if (errors == null)
			{
				return;
			}
			throw new ValidationException(errors);
		}


		private static void ValidateEntity(object entity)
		{
			string[] errors = BaseEntity.Validate(entity, true);
			if (errors == null)
			{
				return;
			}
			throw new ValidationException(errors);
		}
        public SimpleResponse<string[]> GetMessage(IDRequest request)
        {
            throw new NotImplementedException();
        }
        #region IImardaCRM Members

        public GetListResponse<Asset> GetAssetListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetAssetListByTimeStamp", request);
                //var session = _SessionManager.CheckPermissions(request, AuthToken.GetAssetListByTimeStamp);
                //TimeZoneConverter.Globalize(request, session);
                var response = _CRM.GetAssetListByTimeStamp(request);
                //TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Asset>>(ex);
            }
        }

        public GetListResponse<Asset> GetAssetList(IDRequest request)
        {
            try
            {
                Log("GetAssetList", request);
                //var session = _SessionManager.CheckPermissions(request, AuthToken.GetAssetList);
                var response = _CRM.GetAssetList(request);
                //TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Asset>>(ex);
            }
        }

        public BusinessMessageResponse SaveAssetList(SaveListRequest<Asset> request)
        {
            try
            {
                Log("SaveAssetList", request);
                //var session = _SessionManager.CheckPermissions(request, AuthToken.SaveAssetList);
                //TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveAssetList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveAsset(SaveRequest<Asset> request)
        {
            try
            {
                Log("SaveAsset", request);
                //var session = _SessionManager.CheckPermissions(request, AuthToken.SaveAsset);
                //TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveAsset(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteAsset(IDRequest request)
        {
            try
            {
                Log("DeleteAsset", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.DeleteAsset);
                var response = _CRM.DeleteAsset(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<Asset> GetAsset(IDRequest request)
        {
            try
            {
                Log("GetAsset", request);
                //var session = _SessionManager.CheckPermissions(request, AuthToken.GetAsset);
                var response = _CRM.GetAsset(request);
                //TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<Asset>>(ex);
            }
        }

        public GetUpdateCountResponse GetAssetUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetAssetUpdateCount", request);
                //var session = _SessionManager.CheckPermissions(request, AuthToken.GetAssetUpdateCount);
                var response = _CRM.GetAssetUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public GetListResponse<WidgetDef> GetWidgetDefList(IDRequest request)
        {
            try
            {
                Log("GetWidgetDefList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetUserConfigSettings); //to be ????);
                var response = _CRM.GetWidgetDefList(request);
                TimeZoneConverter.LocalizeObject(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<WidgetDef>>(ex);
            }
        }

        public BusinessMessageResponse SaveWidgetDefList(SaveListRequest<WidgetDef> request)
        {
            try
            {
                Log("SaveWidgetDefList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetUserConfigSettings); //to be ????);
                TimeZoneConverter.GlobalizeObject(request.List, session);
                var response = _CRM.SaveWidgetDefList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }


        public GetListResponse<RoleType> GetRoleTypeListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetRoleTypeListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetRoleTypeListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<RoleType> response = _CRM.GetRoleTypeListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<RoleType>>(ex);
            }
        }

        public GetListResponse<RoleType> GetRoleTypeList(IDRequest request)
        {
            try
            {
                Log("GetRoleTypeList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetRoleTypeList);
                GetListResponse<RoleType> response = _CRM.GetRoleTypeList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<RoleType>>(ex);
            }
        }

        public BusinessMessageResponse SaveRoleTypeList(SaveListRequest<RoleType> request)
        {
            try
            {
                Log("SaveRoleTypeList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveRoleTypeList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveRoleTypeList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveRoleType(SaveRequest<RoleType> request)
        {
            try
            {
                Log("SaveRoleType", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveRoleType);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveRoleType(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteRoleType(IDRequest request)
        {
            try
            {
                Log("DeleteRoleType", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.DeleteRoleType);
                var response = _CRM.DeleteRoleType(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<RoleType> GetRoleType(IDRequest request)
        {
            try
            {
                Log("GetRoleType", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetRoleType);
                GetItemResponse<RoleType> response = _CRM.GetRoleType(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<RoleType>>(ex);
            }
        }

        public GetUpdateCountResponse GetRoleTypeUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetRoleTypeUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetRoleTypeUpdateCount);
                var response = _CRM.GetRoleTypeUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public GetListResponse<NotificationPlan> GetNotificationPlanListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetNotificationPlanListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request,
                                                                                                                                 AuthToken.GetNotificationPlanListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<NotificationPlan> response = _CRM.GetNotificationPlanListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationPlan>>(ex);
            }
        }

        public GetListResponse<NotificationPlan> GetNotificationPlanList(IDRequest request)
        {
            try
            {
                Log("GetNotificationPlanList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationPlanList);
                GetListResponse<NotificationPlan> response = _CRM.GetNotificationPlanList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationPlan>>(ex);
            }
        }

        public BusinessMessageResponse SaveNotificationPlanList(SaveListRequest<NotificationPlan> request)
        {
            try
            {
                Log("SaveNotificationPlanList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveNotificationPlanList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveNotificationPlanList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveNotificationPlan(SaveRequest<NotificationPlan> request)
        {
            try
            {
                Log("SaveNotificationPlan", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveNotificationPlan);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveNotificationPlan(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteNotificationPlan(IDRequest request)
        {
            try
            {
                Log("DeleteNotificationPlan", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.DeleteNotificationPlan);
                var response = _CRM.DeleteNotificationPlan(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<NotificationPlan> GetNotificationPlan(IDRequest request)
        {
            try
            {
                Log("GetNotificationPlan", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationPlan);
                GetItemResponse<NotificationPlan> response = _CRM.GetNotificationPlan(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<NotificationPlan>>(ex);
            }
        }

        public GetUpdateCountResponse GetNotificationPlanUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetNotificationPlanUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationPlanUpdateCount);
                var response = _CRM.GetNotificationPlanUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public GetListResponse<NotificationItem> GetNotificationItemListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetNotificationItemListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request,
                                                                                                                                 AuthToken.GetNotificationItemListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<NotificationItem> response = _CRM.GetNotificationItemListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
            }
        }

        public GetListResponse<NotificationItem> GetNotificationItemList(IDRequest request)
        {
            try
            {
                Log("GetNotificationItemList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationItemList);
                GetListResponse<NotificationItem> response = _CRM.GetNotificationItemList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
            }
        }

        public GetListResponse<NotificationItem> GetNotificationTemplateList(IDRequest request)
        {
            try
            {
                Log("GetNotificationTemplateList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationItemList);
                GetListResponse<NotificationItem> response = _CRM.GetNotificationTemplateList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
            }
        }

        public BusinessMessageResponse SaveNotificationItemList(SaveListRequest<NotificationItem> request)
        {
            try
            {
                Log("SaveNotificationItemList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveNotificationItemList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveNotificationItemList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveNotificationItem(SaveRequest<NotificationItem> request)
        {
            try
            {
                Log("SaveNotificationItem", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveNotificationItem);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveNotificationItem(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteNotificationItem(IDRequest request)
        {
            try
            {
                Log("DeleteNotificationItem", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.DeleteNotificationItem);
                var response = _CRM.DeleteNotificationItem(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<NotificationItem> GetNotificationItem(IDRequest request)
        {
            try
            {
                Log("GetNotificationItem", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationItem);
                GetItemResponse<NotificationItem> response = _CRM.GetNotificationItem(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<NotificationItem>>(ex);
            }
        }

        public GetUpdateCountResponse GetNotificationItemUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetNotificationItemUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationItemUpdateCount);
                var response = _CRM.GetNotificationItemUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public GetListResponse<NotificationItem> GetNotificationItemListByNotificationPlanID(IDRequest request)
        {
            try
            {
                Log("GetNotificationItemListByNotificationPlanID", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationItemList);
                GetListResponse<NotificationItem> response = _CRM.GetNotificationItemListByNotificationPlanID(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
            }
        }

        public GetListResponse<Role> GetRoleListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetRoleListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetRoleListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<Role> response = _CRM.GetRoleListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Role>>(ex);
            }
        }

        public GetListResponse<Role> GetRoleList(IDRequest request)
        {
            try
            {
                Log("GetRoleList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetRoleList);
                GetListResponse<Role> response = _CRM.GetRoleList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Role>>(ex);
            }
        }

        public BusinessMessageResponse SaveRoleList(SaveListRequest<Role> request)
        {
            try
            {
                Log("SaveRoleList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveRoleList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveRoleList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveRole(SaveRequest<Role> request)
        {
            try
            {
                Log("SaveRole", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveRole);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveRole(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteRole(IDRequest request)
        {
            try
            {
                Log("DeleteRole", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.DeleteRole);
                var response = _CRM.DeleteRole(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<Role> GetRole(IDRequest request)
        {
            try
            {
                Log("GetRole", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetRole);
                GetItemResponse<Role> response = _CRM.GetRole(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<Role>>(ex);
            }
        }

        public GetUpdateCountResponse GetRoleUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetRoleUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetRoleUpdateCount);
                var response = _CRM.GetRoleUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public GetUpdateCountResponse GetPersonUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetPersonUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetPersonUpdateCount);
                var response = _CRM.GetPersonUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public GetItemResponse<Person> GetPerson(IDRequest request)
        {
            try
            {
                Log("GetPerson", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetPerson);
                GetItemResponse<Person> response = _CRM.GetPerson(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<Person>>(ex);
            }
        }
        public BusinessMessageResponse DeletePerson(IDRequest request)
        {
            try
            {
                Log("DeletePerson", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.DeletePerson);
                var response = _CRM.DeletePerson(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }


        public BusinessMessageResponse SaveNotificationHistory(SaveRequest<NotificationHistory> request)
        {
            try
            {
                Log("SaveNotificationHistory", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveNotificationHistory);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveNotificationHistory(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteNotificationHistory(IDRequest request)
        {
            try
            {
                Log("DeleteNotificationHistory", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.DeleteNotificationHistory);
                var response = _CRM.DeleteNotificationHistory(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<NotificationHistory> GetNotificationHistory(IDRequest request)
        {
            try
            {
                Log("GetNotificationHistory", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationHistory);
                GetItemResponse<NotificationHistory> response = _CRM.GetNotificationHistory(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<NotificationHistory>>(ex);
            }
        }

        public GetUpdateCountResponse GetNotificationHistoryUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetNotificationHistoryUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request,
                                                                                                                                 AuthToken.GetNotificationHistoryUpdateCount);
                var response = _CRM.GetNotificationHistoryUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public GetListResponse<NotificationHistory> GetNotificationHistoryListByNotificationPlanID(IDRequest request)
        {
            try
            {
                Log("GetNotificationHistoryListByNotificationPlanID", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationHistoryList);
                GetListResponse<NotificationHistory> response = _CRM.GetNotificationHistoryListByNotificationPlanID(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationHistory>>(ex);
            }
        }

        public SolutionMessageResponse InstallNotificationItemsToNotificationPlan(SaveListRequest<NotificationItem> request)
        {
            try
            {
                Log("InstallNotificationItemsToNotificationPlan", request);
                SessionObject session = _SessionManager.CheckPermissions(request,
                                                                                                                                 AuthToken.InstallNotificationItemsToNotificationPlan);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.InstallNotificationItemsToNotificationPlan(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<SolutionMessageResponse>(ex);
            }
        }

        public BusinessMessageResponse SavePersonList(SaveListRequest<Person> request)
        {
            try
            {
                Log("SavePersonList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SavePersonList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SavePersonList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SavePerson(SaveRequest<Person> request)
        {
            try
            {
                Log("SavePerson", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SavePerson);

                string userName = null;
                SecurityEntity existingUser = null;
                if (request.Get("LoginName", out userName))
                {
                    var securityRequest = new IDRequest();
                    securityRequest.Put("username", userName);
                    GetItemResponse<SecurityEntity> resp = _Security.GetSecurityEntityByLoginUserName(securityRequest);
                    existingUser = resp.Item;
                }
                BusinessMessageResponse response;
                if (existingUser == null || existingUser.CRMId == request.Item.ID)
                {
                    TimeZoneConverter.GlobalizeObject(request.Item, session);
                    response = _CRM.SavePerson(request);
                }
                else
                {
                    response = new BusinessMessageResponse { ErrorCode = "APcr", Status = false, StatusMessage = "Login name already exists" };
                }
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetListResponse<Person> GetPersonList(IDRequest request)
        {
            try
            {
                Log("GetPersonList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetPersonList);
                GetListResponse<Person> response = _CRM.GetPersonList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Person>>(ex);
            }
        }
        public GetListResponse<Person> GetPersonListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetPersonListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetPersonListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<Person> response = _CRM.GetPersonListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Person>>(ex);
            }
        }

        public SolutionMessageResponse SendNotification(IDRequest request)
        {
            try
            {
                Log("SendNotification", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SendNotification);
                var response = _CRM.SendNotification(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<SolutionMessageResponse>(ex);
            }
        }
        public SolutionMessageResponse SendNewPassword(GenericRequest request)
        {
            try
            {
                Log("SendNewPassword", request);
                var username = (string)request[0];
                var email = (string)request[1];
                var securityRequest = new IDRequest(Guid.Empty, "username", username);

                GetItemResponse<SecurityEntity> securityEntity = _Security.GetSecurityEntityByLoginUserName(securityRequest);
                if (securityEntity.Item == null)
                {
                    return new SolutionMessageResponse { Status = false, StatusMessage = "UserName not found" };
                }
                var crmRequest = new IDRequest(securityEntity.Item.CRMId);
                GetItemResponse<Person> person = _CRM.GetPerson(crmRequest);
                if (person.Item == null || person.Item.Email != email)
                {
                    return new SolutionMessageResponse { Status = false, StatusMessage = "Invalid email address" };
                }
                //generate password and save it to securityentity
                string unecrytedPwd = AuthenticationHelper.GetPassword();
                //string encPwd = AuthenticationHelper.ComputeHash(username, unecrytedPwd);
                securityEntity.Item.LoginPassword = unecrytedPwd;
                var builder = new StringBuilder();
                builder.AppendKV("Username", username); //& IAC-494 
                builder.AppendKV("NewPassword", unecrytedPwd);
                var saveSecurityRequest = new SaveRequest<SecurityEntity>(securityEntity.Item);
                saveSecurityRequest.SetFlag("UpdatePassword", true);
                BusinessMessageResponse securityResponse = _Security.SaveUserSecurityEntity(saveSecurityRequest);
                var finalRequest = new GenericRequest(person.Item.ID, AuthToken.SendNewPassword, person.Item.CompanyID,
                                                                                            builder.ToString(), "UTC");
                var response = _CRM.SendNewPassword(finalRequest);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<SolutionMessageResponse>(ex);
            }
        }
        public GetListResponse<NotificationItem> GetNotificationItemListByPlanID(IDRequest request)
        {
            try
            {
                Log("GetNotificationItemListByPlanID", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationItemList);
                GetListResponse<NotificationItem> response = _CRM.GetNotificationItemListByPlanID(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationItem>>(ex);
            }
        }

        public BusinessMessageResponse SaveNotificationHistoryList(SaveListRequest<NotificationHistory> request)
        {
            try
            {
                Log("SaveNotificationHistoryList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveNotificationHistoryList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveNotificationHistoryList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetListResponse<NotificationHistory> GetNotificationHistoryList(IDRequest request)
        {
            try
            {
                Log("GetNotificationHistoryList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetNotificationHistoryList);
                GetListResponse<NotificationHistory> response = _CRM.GetNotificationHistoryList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationHistory>>(ex);
            }
        }
        public GetListResponse<NotificationHistory> GetNotificationHistoryListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetNotificationHistoryListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request,
                AuthToken.GetNotificationHistoryListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<NotificationHistory> response = _CRM.GetNotificationHistoryListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<NotificationHistory>>(ex);
            }
        }
        public BusinessMessageResponse SaveMessageItem(SaveRequest<MessageItem> request)
        {
            try
            {
                Log("SaveMessageItem", request);

                BusinessMessageResponse response = _CRM.SaveMessageItem(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<BusinessMessageResponse>(ex);
            }
        }

        public BusinessMessageResponse SaveMessageViewedByUser(IDRequest request)
        {
            try
            {
                Log("SaveMessageViewedByUser", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveMessageViewedByUser);
                BusinessMessageResponse response = _CRM.SaveMessageViewedByUser(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<BusinessMessageResponse>(ex);
            }
        }
        public GetListResponse<MessageItem> GetMessageListByUser(IDRequest request)
        {
            try
            {
                Log("GetMessageListByUser", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetMessageListByUser);
                TimeZoneConverter.GlobalizeRequest(request, session);
                var response = _CRM.GetMessageListByUser(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<MessageItem>>(ex);
            }
        }
        public GetUpdateCountResponse GetEmailGroupUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetEmailGroupUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetEmailGroupUpdateCount);
                var response = _CRM.GetEmailGroupUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }
        public GetItemResponse<EmailGroup> GetEmailGroup(IDRequest request)
        {
            try
            {
                Log("GetEmailGroup", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetEmailGroup);
                GetItemResponse<EmailGroup> response = _CRM.GetEmailGroup(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<EmailGroup>>(ex);
            }
        }
        public BusinessMessageResponse DeleteEmailGroup(IDRequest request)
        {
            try
            {
                Log("DeleteEmailGroup", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.DeleteEmailGroup);
                var response = _CRM.DeleteEmailGroup(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetListResponse<EmailGroup> GetEmailGroupListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetEmailGroupListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetEmailGroupListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<EmailGroup> response = _CRM.GetEmailGroupListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<EmailGroup>>(ex);
            }
        }

        public GetListResponse<EmailGroup> GetEmailGroupList(IDRequest request)
        {
            try
            {
                Log("GetEmailGroupList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetEmailGroupList);
                GetListResponse<EmailGroup> response = _CRM.GetEmailGroupList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<EmailGroup>>(ex);
            }
        }

        public BusinessMessageResponse SaveEmailGroupList(SaveListRequest<EmailGroup> request)
        {
            try
            {
                Log("SaveEmailGroupList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveEmailGroupList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveEmailGroupList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveEmailGroup(SaveRequest<EmailGroup> request)
        {
            try
            {
                Log("SaveEmailGroup", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveEmailGroup);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveEmailGroup(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse DeleteCompanyModule(IDRequest request)
        {
            try
            {
                Log("DeleteCompanyModule", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.DeleteCompanyModule);
                var response = _CRM.DeleteCompanyModule(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<CompanyModule> GetCompanyModule(IDRequest request)
        {
            try
            {
                Log("GetCompanyModule", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompanyModule);
                GetItemResponse<CompanyModule> response = _CRM.GetCompanyModule(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<CompanyModule>>(ex);
            }
        }

        public GetUpdateCountResponse GetCompanyModuleUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetCompanyModuleUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompanyModuleUpdateCount);
                var response = _CRM.GetCompanyModuleUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }
        public BusinessMessageResponse SaveCompanyModuleList(SaveListRequest<CompanyModule> request)
        {
            try
            {
                Log("SaveCompanyModuleList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveCompanyModuleList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveCompanyModuleList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveCompanyModule(SaveRequest<CompanyModule> request)
        {
            try
            {
                Log("SaveCompanyModule", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveCompanyModule);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveCompanyModule(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public GetListResponse<CompanyModule> GetCompanyModuleListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetCompanyModuleListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request,
                                                                                                                                 AuthToken.GetCompanyModuleListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<CompanyModule> response = _CRM.GetCompanyModuleListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<CompanyModule>>(ex);
            }
        }
        public GetUpdateCountResponse GetCompanyLocationUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetCompanyLocationUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompanyLocationUpdateCount);
                var response = _CRM.GetCompanyLocationUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public GetListResponse<CompanyModule> GetCompanyModuleList(IDRequest request)
        {
            try
            {
                Log("GetCompanyModuleList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompanyModuleList);
                GetListResponse<CompanyModule> response = _CRM.GetCompanyModuleList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<CompanyModule>>(ex);
            }
        }
        public GetListResponse<CompanyLocation> GetCompanyLocationListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetCompanyLocationListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request,
                                                                                                                                 AuthToken.GetCompanyLocationListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<CompanyLocation> response = _CRM.GetCompanyLocationListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<CompanyLocation>>(ex);
            }
        }

        public GetListResponse<CompanyLocation> GetCompanyLocationList(IDRequest request)
        {
            try
            {
                Log("GetCompanyLocationList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompanyLocationList);
                GetListResponse<CompanyLocation> response = _CRM.GetCompanyLocationList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<CompanyLocation>>(ex);
            }
        }

        public GetItemResponse<CompanyLocation> GetCompanyLocation(IDRequest request)
        {
            try
            {
                Log("GetCompanyLocation", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompanyLocation);
                GetItemResponse<CompanyLocation> response = _CRM.GetCompanyLocation(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<CompanyLocation>>(ex);
            }
        }
        public BusinessMessageResponse DeleteCompanyLocation(IDRequest request)
        {
            try
            {
                Log("DeleteCompanyLocation", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.DeleteCompanyLocation);
                var response = _CRM.DeleteCompanyLocation(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse SaveCompanyLocation(SaveRequest<CompanyLocation> request)
        {
            try
            {
                Log("SaveCompanyLocation", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveCompanyLocation);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveCompanyLocation(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse SaveCompanyLocationList(SaveListRequest<CompanyLocation> request)
        {
            try
            {
                Log("SaveCompanyLocationList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveCompanyLocationList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveCompanyLocationList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetUpdateCountResponse GetCompanyUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetCompanyUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompanyUpdateCount);
                var response = _CRM.GetCompanyUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }
        public GetItemResponse<Company> GetCompany(IDRequest request)
        {
            try
            {
                Log("GetCompany", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompany);
                GetItemResponse<Company> response = _CRM.GetCompany(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<Company>>(ex);
            }
        }
        public BusinessMessageResponse SaveCompany(SaveRequest<Company> request)
        {
            try
            {
                Log("SaveCompany", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveCompany);

                Company company = request.Item;
                if (company.Deleted)
                {
                }
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveCompany(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveCompanyList(SaveListRequest<Company> request)
        {
            try
            {
                Log("SaveCompanyList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveCompanyList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveCompanyList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public GetListResponse<Company> GetCompanyList(IDRequest request)
        {
            try
            {
                Log("GetCompanyList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompanyList);
                GetListResponse<Company> response = _CRM.GetCompanyList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Company>>(ex);
            }
        }
        public GetListResponse<Company> GetCompanyListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetCompanyListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetCompanyListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<Company> response = _CRM.GetCompanyListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Company>>(ex);
            }
        }
        public GetListResponse<Contact> GetContactListByParentId(IDRequest request)
        {
            try
            {
                Log("GetContactListByParentId", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetContactListByParentId);
                //TimeZoneConverter.GlobalizeObject(request.Item, session);|TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.GetContactListByParentId(request);
                //TimeZoneConverter.LocalizeObject(response.Item, session);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
            }
        }
        public GetListResponse<Contact> SearchContactList(IDRequest request)
        {
            try
            {
                Log("SearchContactList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SearchContactList);
                //TimeZoneConverter.GlobalizeObject(request.Item, session);|TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SearchContactList(request);
                //TimeZoneConverter.LocalizeObject(response.Item, session);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
            }
        }
        public BusinessMessageResponse DeleteContact(IDRequest request)
        {
            try
            {
                Log("DeleteContact", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.DeleteContact);
                var response = _CRM.DeleteContact(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<BusinessMessageResponse>(ex);
            }
        }
        public GetUpdateCountResponse GetContactUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetContactUpdateCount", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetContactUpdateCount);
                var response = _CRM.GetContactUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }
        public BusinessMessageResponse DeleteContactMapByContactNPerson(IDRequest request)
        {
            try
            {
                Log("DeleteContactMapByContactNPerson", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.DeleteContactMapByContactNPerson);
                var response = _CRM.DeleteContactMapByContactNPerson(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<BusinessMessageResponse>(ex);
            }
        }
        public GetItemResponse<Contact> GetContact(IDRequest request)
        {
            try
            {
                Log("GetContact", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetContact);
                GetItemResponse<Contact> response = _CRM.GetContact(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<Contact>>(ex);
            }
        }
        public GetListResponse<Contact> GetContactList(IDRequest request)
        {
            try
            {
                Log("GetContactList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetContactList);
                GetListResponse<Contact> response = _CRM.GetContactList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
            }
        }
        public BusinessMessageResponse SaveContactList(SaveListRequest<Contact> request)
        {
            try
            {
                Log("SaveContactList", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveContactList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _CRM.SaveContactList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse SaveContactMap(SaveRequest<ContactMap> request)
        {
            try
            {
                Log("SaveContactMap", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveContactMap);
                var response = _CRM.SaveContactMap(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<BusinessMessageResponse>(ex);
            }
        }
        public GetListResponse<Contact> GetContactListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetContactListByTimeStamp", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetContactListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                GetListResponse<Contact> response = _CRM.GetContactListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<Contact>>(ex);
            }
        }
        public BusinessMessageResponse SaveContact(SaveRequest<Contact> request)
        {
            try
            {
                Log("SaveContact", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.SaveContact);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _CRM.SaveContact(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        #endregion

        public GetItemResponse<ConfiguredSessionObject> Login(SessionRequest request)
        {
            try
            {
                Log("Login", request);
                GetItemResponse<ConfiguredSessionObject> result = _Security.Login(request);
                ConfiguredSessionObject session = result.Item;
                if (session != null)
                {
                    var req = new IDRequest(session.CRMID) { SID = request.SessionID };
                    if (result.Item.EntityType == 0) //User Login
                    {
                        GetItemResponse<Person> resp = _CRM.GetPerson(req);


                        if (ServiceMessageHelper.IsSuccess(resp))
                        {
                            Person person = resp.Item;
                            if (!person.Active || person.Deleted)
                            {
                                session = null; //if person associated is inactive or delete, we dont want it validated 
                            }
                            else
                            {
                                var seResp = _Security.GetUserSecurityEntity(new IDRequest(person.ID));
                                if (ServiceMessageHelper.IsSuccess(seResp))
                                {
                                    if (!seResp.Item.IsAdmin)
                                    {
                                        var companyResp = _CRM.GetCompany(new IDRequest(person.CompanyID));
                                        if (ServiceMessageHelper.IsSuccess(companyResp))
                                        {
                                            if (companyResp.Item != null && !companyResp.Item.Active)
                                            {
                                                session = null;
                                                result.StatusMessage = "Company is inactive";
                                            }
                                        }
                                    }
                                }
                            }
                           
                        }
                        else
                        {
                            //there is no person associated, so we dont want to validate it
                            session = null;
                        }
                    }
                   
                }
                result.Item = session;
                Log("Login->" + session, null);
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<ConfiguredSessionObject>>(ex);
            }
        }

    }
}
