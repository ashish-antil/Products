/***********************************************************************
Auto Generated Code

Generated by   : adam-Laptop\adam
Date Generated : 28/04/2009 1:33 PM
Copyright @2009 CodeGenerator
***********************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using ImardaSecurityBusiness;
using Imarda360Base;
using FernBusinessBase;
namespace Cormit.Application.RouteApplication.Security
{

	[ServiceContract]
	public partial interface IImardaSecurity : IServerFacadeBase
	{
		[OperationContract]
		BusinessMessageResponse IsAuthenticated(SessionRequest request);

		[OperationContract]
		GetItemResponse<ConfiguredSessionObject> GetSessionByID(SessionRequest request);

		[OperationContract]
		GetItemResponse<ConfiguredSessionObject> Login(SessionRequest request);

		[OperationContract]
		BusinessMessageResponse Logout(SessionRequest request);

		[OperationContract]
		BusinessMessageResponse ResetPassword(ResetPasswordRequest request);

		[OperationContract]
		GetItemResponse<ConfiguredSessionObject> GetSession(SessionRequest request);

		[OperationContract]
		BusinessMessageResponse SetAcessTokenOnSession(SessionRequest request);

		[OperationContract]
		GetItemResponse<SecurityEntity> GetUserSecurityEntity(IDRequest request);

		[OperationContract]
		GetItemResponse<SecurityEntity> GetSecurityEntityByLoginUserName(IDRequest request);

		[OperationContract]
		BusinessMessageResponse SaveUserSecurityEntity(SaveRequest<SecurityEntity> request);

		/// <summary>
		/// Set the Deleted flag of the SecurityEntity with the given CRM ID.
		/// </summary>
		/// <param name="request">ID = CRM ID, ["Deleted"] true/false: deleted flag</param>
		/// <returns></returns>
		[OperationContract]
		BusinessMessageResponse SetDeletedSecurityEntityByCRMID(IDRequest request);

		#region SecurityObjects For Entity
		[OperationContract]
		GetListResponse<SecurityObject> GetUnassignedSecurityObjects(IDRequest request);

		[OperationContract]
		GetListResponse<SecurityObject> GetAssignedSecurityObjects(IDRequest request);

		[OperationContract]
		GenericList360Response<GetListResponse<SecurityObject>> GetAssignedAndUnAssignedSecurityObjects(IDRequest request);

		[OperationContract]
		SolutionMessageResponse AssignSecurityObjectsToEntity(SaveListRequest<SecurityObject> objects);
		#endregion

		[OperationContract]
		GetListResponse<LogonLog> GetTopNLogonLogListBySecurityEntityID(IDRequest request);
	}
}

