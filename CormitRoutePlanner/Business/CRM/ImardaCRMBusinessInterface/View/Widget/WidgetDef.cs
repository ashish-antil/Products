using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaCRMBusiness 
{
	partial interface IImardaCRM 
	{
 
		#region Operation Contracts for WidgetDef
		[OperationContract]
		GetListResponse<WidgetDef> GetWidgetDefList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveWidgetDefList(SaveListRequest<WidgetDef> request);

		#endregion
	}
}
