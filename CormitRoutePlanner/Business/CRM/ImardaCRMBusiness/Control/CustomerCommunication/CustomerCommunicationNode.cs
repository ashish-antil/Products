using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaCRMBusiness
{
	partial class ImardaCRM
	{
 		public GetListResponse<CustomerCommunicationNode> GetCustomerCommunicationNodes(IDRequest request)
		{
			var response = new GetListResponse<CustomerCommunicationNode>();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<CustomerCommunicationNode>());
				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				var spName = "SPGetCustomerCommunicationNodes";
				using (IDataReader dr = db.ExecuteDataReader(spName, request.CompanyID))
				{
					response.List = new List<CustomerCommunicationNode>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<CustomerCommunicationNode>(dr));
					}
				}
				return response;
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<CustomerCommunicationNode>>(ex);
			}
		}
 
	}
}