using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using ImardaCRMBusiness;


namespace Imarda360Application.CRM
{
	partial interface IImardaCRM
	{

		#region Operation Contracts for NotificationItem

		[OperationContract]
		GetListResponse<NotificationItem> GetNotificationItemListByNotificationPlanID(IDRequest request);

		//SRR IM-4126
		[OperationContract]
		GetListResponse<NotificationItem> GetNotificationItemListByPlanID(IDRequest request);
		//SRR IM-4126

		[OperationContract]
		GetListResponse<NotificationItem> GetNotificationTemplateList(IDRequest request);

		#endregion

	}
}