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
		public GetListResponse<Solution> GetSolutionList()
		{
			try
			{
				return GenericGetEntityList<Solution>(true, new Guid[] { });
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Solution>>(ex);
			}
		}



		public BusinessMessageResponse SaveSolutionList(SaveListRequest<Solution> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Solution item in request.List)
				{
					response = GenericSaveEntity<Solution>("Solution", new object[] { item.ID, item.SolutionName });
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}

			return response;
		}



		public BusinessMessageResponse DeleteSolution(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Solution>("Solution", new object[] { request.ID });
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
	}
}
