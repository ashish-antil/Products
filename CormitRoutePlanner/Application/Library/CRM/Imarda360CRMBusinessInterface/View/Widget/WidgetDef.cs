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
		#region Operation Contracts for Role
		[OperationContract]
		//List<WidgetDef> GetWidgets(Guid companyID, Guid userID);
		GetListResponse<WidgetDef> GetWidgetDefList(IDRequest request);
		

		[OperationContract]
		BusinessMessageResponse SaveWidgetDefList(SaveListRequest<WidgetDef> request);
		//bool SaveWidgets(List<WidgetDef> widgets);

		#endregion
	}
}
