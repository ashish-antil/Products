using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		

		public GetListResponse<SecurityObjectMap> GetSecurityObjectMapList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<SecurityObjectMap>("SecurityObjectMap", true, request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<SecurityObjectMap>>(ex);
			}
		}

	}
}
