using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using ImardaCRMBusiness;
using Imarda360Base;


namespace Imarda360Application.CRM
{
	partial interface IImardaCRM
	{

		#region Extra Operation Contracts for NotificationPlan
		[OperationContract]
		SolutionMessageResponse InstallNotificationItemsToNotificationPlan(SaveListRequest<NotificationItem> entities);
		[OperationContract]
		SolutionMessageResponse SendNewPassword(GenericRequest req);

		#endregion

		[OperationContract]
		SolutionMessageResponse SendNotification(IDRequest request);
	}
}

