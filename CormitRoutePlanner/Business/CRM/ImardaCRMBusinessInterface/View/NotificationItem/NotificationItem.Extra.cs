using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness
{
	partial interface IImardaCRM
	{

		#region Operation Contracts for NotificationItem

		[OperationContract]
		GetListResponse<NotificationItem> GetNotificationTemplateList(IDRequest request);

		[OperationContract]
		GetListResponse<NotificationItem> GetNotificationItemListByNotificationPlanID(IDRequest request);

		//SRR IM-4126
		[OperationContract]
		GetListResponse<NotificationItem> GetNotificationItemListByPlanID(IDRequest request);
		//SRR IM-4126

		[OperationContract]
		BusinessMessageResponse DeleteNotificationPlanItemsByPlanID(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SendNotificationPlan(GenericRequest request);

		[OperationContract]
		BusinessMessageResponse SendNotificationEmail(GenericRequest request);

		[OperationContract]
		BusinessMessageResponse SendNotificationSMS(GenericRequest request);

		[OperationContract]
		BusinessMessageResponse SendDeviceNotificationSMS(GenericRequest request);  //, string mobilePhone);

		[OperationContract]
		SimpleResponse<string> HttpPost(GenericRequest request);

		[OperationContract]
		SimpleResponse<string> GetNotification(GenericRequest request);

		#endregion

	}
}
