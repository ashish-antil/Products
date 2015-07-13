using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetListResponse<EmailGroup> GetEmailGroupListByCompanyID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<EmailGroup>("CompanyID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<EmailGroup>>(ex);
			}
		}
	}
}