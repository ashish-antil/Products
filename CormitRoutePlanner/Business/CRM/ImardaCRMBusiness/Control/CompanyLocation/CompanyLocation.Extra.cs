using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
		public GetListResponse<CompanyLocation> GetCompanyLocationListByCompanyID(IDRequest request)
		{
			try
			{
				return GenericGetRelated<CompanyLocation>("CompanyID", request.ID, false);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<CompanyLocation>>(ex);
			}
		}
	}
}
