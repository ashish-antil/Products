using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaSecurityBusiness;
using System.ServiceModel;
using Imarda360Base;
using Imarda.Lib;

namespace Cormit.Application.RouteApplication.Security
{
	partial class ImardaSecurity
	{
		/// <summary>
		/// Change the security entity company
		/// </summary>
		/// <param name="request">.ID = new company ID, ["username"] = SecurityEntity.LoginUserName</param>
		/// <returns>CRMID</returns>
		public SimpleResponse<Guid> TransferUser(IDRequest request)
		{
			try
			{
				SimpleResponse<Guid> appResponse = null;
				var service = ImardaProxyManager.Instance.IImardaSecurityProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					var resp = service.GetSecurityEntityByLoginUserName(request);
					ErrorHandler.CheckItem(resp);
					var securityEntity = resp.Item;
					securityEntity.CompanyID = request.ID;
					var resp2 = service.SaveSecurityEntity(new SaveRequest<SecurityEntity>(securityEntity));
					ErrorHandler.Check(resp2);
					appResponse = new SimpleResponse<Guid>(securityEntity.CRMId);
				});
				return appResponse;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<SimpleResponse<Guid>>(ex);
			}
		}
}
}