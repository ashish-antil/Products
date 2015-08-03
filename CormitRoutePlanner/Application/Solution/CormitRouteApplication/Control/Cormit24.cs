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
using Imarda.Logging;
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

        private static readonly ErrorLogger _Log = ErrorLogger.GetLogger("C24");
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
            _CallLogger = new MethodCallLogger(_Log);

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
# region IimardaSecurity

        /// <summary>
        /// Change the security entity and person CompanyID.
        /// Use this in conjunction with Change
        /// </summary>
        /// <param name="request">.ID = new company ID, ["username"] = SecurityEntity.LoginUserName</param>
        /// <returns>personID wrapped in SimpleResponse</returns>
        public SimpleResponse<Guid> TransferUser(IDRequest request)
        {
            try
            {
                Log("TransferUser", request);
                _SessionManager.CheckPermissions(request, AuthToken.TransferUser);
                var response1 = _Security.TransferUser(request);
                Guid personID = response1.Value;
                var response2 = _CRM.GetPerson(new IDRequest(personID));
                Person person = response2.Item;
                person.CompanyID = request.ID;
                var response = _CRM.SavePerson(new SaveRequest<Person>(person));
                ErrorHandler.Check(response);
                LogResult(request, response);
                return new SimpleResponse<Guid>(person.ID);
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<SimpleResponse<Guid>>(ex);
            }
        }
        public BusinessMessageResponse SetDeletedSecurityEntityByCRMID(IDRequest request)
        {
            try
            {
                Log("SetDeletedSecurityEntityByCRMID", request);
                _SessionManager.CheckPermissions(request, AuthToken.SetDeletedSecurityEntityByCRMID);
                var response = _Security.SetDeletedSecurityEntityByCRMID(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public GetListResponse<SecurityObject> GetUnassignedSecurityObjects(IDRequest request)
        {
            try
            {
                Log("GetUnassignedSecurityObjects", request);
                var response = _Security.GetUnassignedSecurityObjects(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
            }
        }

        public GetListResponse<SecurityObject> GetAssignedSecurityObjects(IDRequest request)
        {
            try
            {
                Log("GetAssignedSecurityObjects", request);
                var response = _Security.GetAssignedSecurityObjects(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<SecurityObject>>(ex);
            }
        }
        public BusinessMessageResponse SetAcessTokenOnSession(SessionRequest request)
        {
            try
            {
                Log("SetAcessTokenOnSession", request);
                var response = _Security.SetAcessTokenOnSession(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse SaveUserSecurityEntity(SaveRequest<SecurityEntity> request)
        {
            try
            {
                Log("SaveUserSecurityEntity", request);
                _SessionManager.CheckPermissions(request, AuthToken.SaveUserSecurityEntity);
                var response = _Security.SaveUserSecurityEntity(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse SaveFeatureSupportList(SaveListRequest<FeatureSupport> request)
        {
            try
            {
                Log("SaveFeatureSupportList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveFeatureSupportList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _Security.SaveFeatureSupportList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveFeatureSupport(SaveRequest<FeatureSupport> request)
        {
            try
            {
                Log("SaveFeatureSupport", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveFeatureSupport);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _Security.SaveFeatureSupport(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteFeatureSupport(IDRequest request)
        {
            try
            {
                Log("DeleteFeatureSupport", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.DeleteFeatureSupport);
                var response = _Security.DeleteFeatureSupport(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<FeatureSupport> GetFeatureSupport(IDRequest request)
        {
            try
            {
                Log("GetFeatureSupport", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetFeatureSupport);
                var response = _Security.GetFeatureSupport(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<FeatureSupport>>(ex);
            }
        }

        public GetUpdateCountResponse GetFeatureSupportUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetFeatureSupportUpdateCount", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetFeatureSupportUpdateCount);
                var response = _Security.GetFeatureSupportUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }
        public BusinessMessageResponse SaveApplicationPlanList(SaveListRequest<ApplicationPlan> request)
        {
            try
            {
                Log("SaveApplicationPlanList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationPlanList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _Security.SaveApplicationPlanList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveApplicationPlan(SaveRequest<ApplicationPlan> request)
        {
            try
            {
                Log("SaveApplicationPlan", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationPlan);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _Security.SaveApplicationPlan(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteApplicationPlan(IDRequest request)
        {
            try
            {
                Log("DeleteApplicationPlan", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.DeleteApplicationPlan);
                var response = _Security.DeleteApplicationPlan(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<ApplicationPlan> GetApplicationPlan(IDRequest request)
        {
            try
            {
                Log("GetApplicationPlan", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationPlan);
                var response = _Security.GetApplicationPlan(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<ApplicationPlan>>(ex);
            }
        }

        public GetUpdateCountResponse GetApplicationPlanUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetApplicationPlanUpdateCount", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationPlanUpdateCount);
                var response = _Security.GetApplicationPlanUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public GetListResponse<ApplicationPlanFeature> GetApplicationPlanFeatureList(IDRequest request)
        {
            try
            {
                Log("GetApplicationPlanFeatureList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationPlanFeatureList);
                var response = _Security.GetApplicationPlanFeatureList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationPlanFeature>>(ex);
            }
        }

        public BusinessMessageResponse SaveApplicationPlanFeatureList(SaveListRequest<ApplicationPlanFeature> request)
        {
            try
            {
                Log("SaveApplicationPlanFeatureList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationPlanFeatureList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _Security.SaveApplicationPlanFeatureList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveApplicationPlanFeature(SaveRequest<ApplicationPlanFeature> request)
        {
            try
            {
                Log("SaveApplicationPlanFeature", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationPlanFeature);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _Security.SaveApplicationPlanFeature(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteApplicationPlanFeature(IDRequest request)
        {
            try
            {
                Log("DeleteApplicationPlanFeature", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.DeleteApplicationPlanFeature);
                var response = _Security.DeleteApplicationPlanFeature(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<ApplicationPlanFeature> GetApplicationPlanFeature(IDRequest request)
        {
            try
            {
                Log("GetApplicationPlanFeature", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationPlanFeature);
                var response = _Security.GetApplicationPlanFeature(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<ApplicationPlanFeature>>(ex);
            }
        }

        public BusinessMessageResponse SaveApplicationFeatureOwnerList(SaveListRequest<ApplicationFeatureOwner> request)
        {
            try
            {
                Log("SaveApplicationFeatureOwnerList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationFeatureOwnerList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _Security.SaveApplicationFeatureOwnerList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse SaveApplicationFeatureOwner(SaveRequest<ApplicationFeatureOwner> request)
        {
            try
            {
                Log("SaveApplicationFeatureOwner", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationFeatureOwner);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _Security.SaveApplicationFeatureOwner(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteApplicationFeatureOwner(IDRequest request)
        {
            try
            {
                Log("DeleteApplicationFeatureOwner", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.DeleteApplicationFeatureOwner);
                var response = _Security.DeleteApplicationFeatureOwner(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<ApplicationFeatureOwner> GetApplicationFeatureOwner(IDRequest request)
        {
            try
            {
                Log("GetApplicationFeatureOwner", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureOwner);
                var response = _Security.GetApplicationFeatureOwner(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<ApplicationFeatureOwner>>(ex);
            }
        }

        public GetUpdateCountResponse GetApplicationFeatureOwnerUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetApplicationFeatureOwnerUpdateCount", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureOwnerUpdateCount);
                var response = _Security.GetApplicationFeatureOwnerUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }
        public BusinessMessageResponse SaveApplicationFeatureList(SaveListRequest<ApplicationFeature> request)
        {
            try
            {
                Log("SaveApplicationFeatureList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationFeatureList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _Security.SaveApplicationFeatureList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse SaveApplicationFeatureCategoryList(SaveListRequest<ApplicationFeatureCategory> request)
        {
            try
            {
                Log("SaveApplicationFeatureCategoryList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationFeatureCategoryList);
                TimeZoneConverter.GlobalizeObjectList(request.List, session);
                var response = _Security.SaveApplicationFeatureCategoryList(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse SaveApplicationFeatureCategory(SaveRequest<ApplicationFeatureCategory> request)
        {
            try
            {
                Log("SaveApplicationFeatureCategory", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationFeatureCategory);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _Security.SaveApplicationFeatureCategory(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public BusinessMessageResponse DeleteApplicationFeatureCategory(IDRequest request)
        {
            try
            {
                Log("DeleteApplicationFeatureCategory", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.DeleteApplicationFeatureCategory);
                var response = _Security.DeleteApplicationFeatureCategory(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<ApplicationFeatureCategory> GetApplicationFeatureCategory(IDRequest request)
        {
            try
            {
                Log("GetApplicationFeatureCategory", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureCategory);
                var response = _Security.GetApplicationFeatureCategory(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<ApplicationFeatureCategory>>(ex);
            }
        }

        public GetUpdateCountResponse GetApplicationFeatureCategoryUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetApplicationFeatureCategoryUpdateCount", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureCategoryUpdateCount);
                var response = _Security.GetApplicationFeatureCategoryUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }

        public BusinessMessageResponse SaveApplicationFeature(SaveRequest<ApplicationFeature> request)
        {
            try
            {
                Log("SaveApplicationFeature", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.SaveApplicationFeature);
                TimeZoneConverter.GlobalizeObject(request.Item, session);
                var response = _Security.SaveApplicationFeature(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                Log("ResetPassword", request);
                var response = _Security.ResetPassword(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }
        public BusinessMessageResponse Logout(SessionRequest request)
        {
            Log("Logout", request);

            var response = _Security.Logout(request);
            LogResult(request, response);
            return response;
        }

        /// <summary>
        /// Check for existence of session object in cache.
        /// </summary>
        /// <param name="request">SessionID != null, other fields are not used</param>
        /// <returns>BusinessMessageResponse.StatusMessage == "OK" if authenticated </returns>
        public BusinessMessageResponse IsAuthenticated(SessionRequest request)
        {
            try
            {
                if (request.SessionID != Guid.Empty)
                {
                    // check session ID only
                    if (_SessionManager.GetSession(request.SessionID) != null)
                    {
                        return new BusinessMessageResponse();
                    }
                    else
                    {
                        return new BusinessMessageResponse { Status = false };
                    }
                }
                else
                {
                    // check user name and password
                    bool authenticated = false;
                    string user = request.Username;
                    string pwd = request.Password;
                    if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pwd))
                    {
                        return new BusinessMessageResponse { Status = false, StatusMessage = "no input" };
                    }
                    GetItemResponse<SecurityEntity> resp =
                        _Security.GetSecurityEntityByLoginUserName(new IDRequest(Guid.Empty, "username", user));
                    if (ServiceMessageHelper.IsSuccess(resp))
                    {
                        SecurityEntity se = resp.Item;
                        string hash2 = Convert.ToBase64String(AuthenticationHelper.ComputePasswordHash(se.Salt, pwd));
                        if (hash2 == se.LoginPassword)
                        {
                            authenticated = true;
                        }
                        else
                        {
                            string hash1 = AuthenticationHelper.ComputeHashOldStyle(user, pwd);
                            if (hash1 == se.LoginPassword)
                            {
                                authenticated = true;
                            }
                        }
                    }
                    return new BusinessMessageResponse { Status = authenticated };
                }
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

        public GetItemResponse<SecurityEntity> GetUserSecurityEntity(IDRequest request)
        {
            try
            {
                Log("GetUserSecurityEntity", request);
                SessionObject session = _SessionManager.CheckPermissions(request, AuthToken.GetUserSecurityEntity);

                GetItemResponse<SecurityEntity> response = RemoveSensitiveData(_Security.GetUserSecurityEntity(request));
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<SecurityEntity>>(ex);
            }
        }
        public GetListResponse<LogonLog> GetTopNLogonLogListBySecurityEntityID(IDRequest request)
        {
            try
            {
                Log("GetTopNLogonLogListBySecurityEntityID", request);
                SessionObject session = _SessionManager.CheckPermissions(request,
                                                                                                                                 AuthToken.GetTopNLogonLogListBySecurityEntityID);
                TimeZoneConverter.GlobalizeRequest(request, session);
                GetListResponse<LogonLog> response = _Security.GetTopNLogonLogListBySecurityEntityID(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<LogonLog>>(ex);
            }
        }
        public GetItemResponse<ConfiguredSessionObject> GetSessionByID(SessionRequest request)
        {
            try
            {
                var session = _SessionManager.GetSession(request.SessionID);
                var result = new GetItemResponse<ConfiguredSessionObject>();
                if (session != null)
                {
                    result.Item = (ConfiguredSessionObject)session;
                }
                else
                {
                    result = new GetItemResponse<ConfiguredSessionObject> { Status = false, StatusMessage = "Invalid Session, Please ReLogin" };
                }
                return result;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<ConfiguredSessionObject>>(ex);
            }
        }
        public GetItemResponse<ConfiguredSessionObject> GetSession(SessionRequest request)
        {
            try
            {
                Log("GetSessionByAcessToken", request);
                var response = _Security.GetSession(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<ConfiguredSessionObject>>(ex);
            }
        }
        public GetItemResponse<SecurityEntity> GetSecurityEntityByLoginUserName(IDRequest request)
        {
            try
            {
                Log("GetSecurityEntityByLoginUserName", request);
                SessionObject session = _SessionManager.CheckPermissions(request,
                                                                                                                                 AuthToken.GetSecurityEntityByLoginUserName);
                GetItemResponse<SecurityEntity> response = RemoveSensitiveData(_Security.GetSecurityEntityByLoginUserName(request));
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<SecurityEntity>>(ex);
            }
        }
        public GetListResponse<FeatureSupport> GetFeatureSupportListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetFeatureSupportListByTimeStamp", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetFeatureSupportListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                var response = _Security.GetFeatureSupportListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<FeatureSupport>>(ex);
            }
        }

        public GetListResponse<FeatureSupport> GetFeatureSupportList(IDRequest request)
        {
            try
            {
                Log("GetFeatureSupportList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetFeatureSupportList);
                var response = _Security.GetFeatureSupportList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<FeatureSupport>>(ex);
            }
        }
        public GenericList360Response<GetListResponse<SecurityObject>> GetAssignedAndUnAssignedSecurityObjects(
            IDRequest request)
        {
            try
            {
                Log("GetAssignedAndUnAssignedSecurityObjects", request);
                SessionObject session = _SessionManager.GetSession(request.SessionID);
                request["UserID"] = session.SecurityEntityID.ToString();
                var response = _Security.GetAssignedAndUnAssignedSecurityObjects(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GenericList360Response<GetListResponse<SecurityObject>>>(ex);
            }
        }

        public SolutionMessageResponse AssignSecurityObjectsToEntity(SaveListRequest<SecurityObject> request)
        {
            try
            {
                Log("AssignSecurityObjectsToEntity", request);
                var response = _Security.AssignSecurityObjectsToEntity(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<SolutionMessageResponse>(ex);
            }
        }

        public GetListResponse<ApplicationPlan> GetApplicationPlanListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetApplicationPlanListByTimeStamp", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationPlanListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                var response = _Security.GetApplicationPlanListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationPlan>>(ex);
            }
        }

        public GetListResponse<ApplicationPlan> GetApplicationPlanList(IDRequest request)
        {
            try
            {
                Log("GetApplicationPlanList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationPlanList);
                var response = _Security.GetApplicationPlanList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationPlan>>(ex);
            }
        }
        public GetUpdateCountResponse GetApplicationPlanFeatureUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetApplicationPlanFeatureUpdateCount", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationPlanFeatureUpdateCount);
                var response = _Security.GetApplicationPlanFeatureUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }
        public GetListResponse<ApplicationPlanFeature> GetApplicationPlanFeatureListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetApplicationPlanFeatureListByTimeStamp", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationPlanFeatureListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                var response = _Security.GetApplicationPlanFeatureListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationPlanFeature>>(ex);
            }
        }
        public GetUpdateCountResponse GetApplicationFeatureUpdateCount(GetUpdateCountRequest request)
        {
            try
            {
                Log("GetApplicationFeatureUpdateCount", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureUpdateCount);
                var response = _Security.GetApplicationFeatureUpdateCount(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
            }
        }
        public GetListResponse<ApplicationFeatureOwner> GetApplicationFeatureOwnerListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetApplicationFeatureOwnerListByTimeStamp", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureOwnerListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                var response = _Security.GetApplicationFeatureOwnerListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationFeatureOwner>>(ex);
            }
        }

        public GetListResponse<ApplicationFeatureOwner> GetApplicationFeatureOwnerList(IDRequest request)
        {
            try
            {
                Log("GetApplicationFeatureOwnerList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureOwnerList);
                var response = _Security.GetApplicationFeatureOwnerList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationFeatureOwner>>(ex);
            }
        }
        public GetListResponse<ApplicationFeature> GetApplicationFeatureListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetApplicationFeatureListByTimeStamp", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                var response = _Security.GetApplicationFeatureListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
            }
        }
        public GetListResponse<ApplicationFeature> GetApplicationFeatureListByOwnerID(IDRequest request)
        {
            try
            {
                Log("GetApplicationFeatureListByOwnerID", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureListByOwnerID);

                var response = _Security.GetApplicationFeatureListByOwnerID(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
            }
        }
        public GetListResponse<ApplicationFeature> GetApplicationFeatureListByCategoryID(IDRequest request)
        {
            try
            {
                Log("GetApplicationFeatureListByCategoryID", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureListByCategoryID);

                var response = _Security.GetApplicationFeatureListByCategoryID(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
            }
        }
        public GetListResponse<ApplicationFeature> GetApplicationFeatureList(IDRequest request)
        {
            try
            {
                Log("GetApplicationFeatureList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureList);
                var response = _Security.GetApplicationFeatureList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
            }
        }
        public GetListResponse<ApplicationFeatureCategory> GetApplicationFeatureCategoryListByTimeStamp(GetListByTimestampRequest request)
        {
            try
            {
                Log("GetApplicationFeatureCategoryListByTimeStamp", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureCategoryListByTimeStamp);
                TimeZoneConverter.Globalize(request, session);
                var response = _Security.GetApplicationFeatureCategoryListByTimeStamp(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationFeatureCategory>>(ex);
            }
        }
        public GetListResponse<ApplicationFeatureCategory> GetApplicationFeatureCategoryList(IDRequest request)
        {
            try
            {
                Log("GetApplicationFeatureCategoryList", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeatureCategoryList);
                var response = _Security.GetApplicationFeatureCategoryList(request);
                TimeZoneConverter.LocalizeObjectList(response.List, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetListResponse<ApplicationFeatureCategory>>(ex);
            }
        }
        public GetItemResponse<ApplicationFeature> GetApplicationFeature(IDRequest request)
        {
            try
            {
                Log("GetApplicationFeature", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.GetApplicationFeature);
                var response = _Security.GetApplicationFeature(request);
                TimeZoneConverter.LocalizeObject(response.Item, session);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle<GetItemResponse<ApplicationFeature>>(ex);
            }
        }
        public BusinessMessageResponse DeleteApplicationFeature(IDRequest request)
        {
            try
            {
                Log("DeleteApplicationFeature", request);
                var session = _SessionManager.CheckPermissions(request, AuthToken.DeleteApplicationFeature);
                var response = _Security.DeleteApplicationFeature(request);
                LogResult(request, response);
                return response;
            }
            catch (Exception ex)
            {
                return ErrorHandler.Handle(ex);
            }
        }

#endregion

        private static GetItemResponse<SecurityEntity> RemoveSensitiveData(GetItemResponse<SecurityEntity> r)
        {
            SecurityEntity se = r.Item;
            if (se != null)
            {
                se.Salt = Guid.Empty;
                se.LoginPassword = string.Empty;
            }
            return r;
        }
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
