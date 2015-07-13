
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;

namespace ImardaConfigurationBusiness 
{
	partial interface IImardaConfiguration 
	{

		#region Operation Contracts for Shortcut
		
		[OperationContract]
		GetListResponse<Shortcut> GetShortcutListByTimeStamp(GetListByTimestampRequest request);

		[OperationContract]
		GetListResponse<Shortcut> GetShortcutList(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveShortcutList(SaveListRequest<Shortcut> request);

		[OperationContract]
		BusinessMessageResponse UpdateShortcutList(SaveListRequest<Shortcut> request);	

		[OperationContract]
		BusinessMessageResponse SaveShortcut(SaveRequest<Shortcut> request);

		[OperationContract]
		BusinessMessageResponse DeleteShortcut(IDRequest request);

		[OperationContract]
		GetItemResponse<Shortcut> GetShortcut(IDRequest request);

		[OperationContract]
		GetUpdateCountResponse GetShortcutUpdateCount(GetUpdateCountRequest request);
		#endregion

	}
}