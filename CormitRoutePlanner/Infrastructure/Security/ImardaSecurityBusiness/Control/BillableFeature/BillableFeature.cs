using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.Data;
using System.ServiceModel;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{
		#region GetBillableFeaturesByCompanyID
		public GetListResponse<BillableFeature> GetBillableFeaturesByCompanyID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<BillableFeature>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<BillableFeature>());

				object[] args = new object[] { request.ID };
				using (IDataReader dr = db.ExecuteDataReader("SPGetBillableFeaturesByCompanyID", args))
				{
					response.List = new List<BillableFeature>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<BillableFeature>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<BillableFeature>>(ex);
			}

		}
		#endregion
	}
}
