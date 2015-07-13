using System;
using System.Collections.Generic;
using FernBusinessBase;
using System.Data;
using FernBusinessBase.Errors;

namespace ImardaSecurityBusiness
{
	partial class ImardaSecurity
	{

		#region GetApplicationFeatureListByCategoryID
		public GetListResponse<ApplicationFeature> GetApplicationFeatureListByCategoryID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<ApplicationFeature>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ApplicationFeature>());

				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				int topn = request.Get<int>("TopN", int.MaxValue);

				Guid categoryid = Guid.Empty;
				request.Get<Guid>("CategoryID", out categoryid);

				using (IDataReader dr = db.ExecuteDataReader("SPGetApplicationFeatureListByCategoryID", includeInactive, topn, request.CompanyID, categoryid))
				{
					response.List = new List<ApplicationFeature>();
					while (dr.Read()) response.List.Add(GetFromData<ApplicationFeature>(dr));
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
			}
		}
		#endregion

		#region GetApplicationFeatureListByOwnerID
		public GetListResponse<ApplicationFeature> GetApplicationFeatureListByOwnerID(IDRequest request)
		{
			try
			{
				var response = new GetListResponse<ApplicationFeature>();
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<ApplicationFeature>());

				bool includeInactive = request.HasSome(RetrievalOptions.IncludeInactive);
				int topn = request.Get<int>("TopN", int.MaxValue);

				Guid ownerid = Guid.Empty;
				request.Get<Guid>("OwnerID", out ownerid);

				using (IDataReader dr = db.ExecuteDataReader("SPGetApplicationFeatureListByOwnerID", includeInactive, topn, request.CompanyID, ownerid))
				{
					response.List = new List<ApplicationFeature>();
					while (dr.Read())
					{
						response.List.Add(GetFromData<ApplicationFeature>(dr));
					}

					return response;
				}

			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<ApplicationFeature>>(ex);
			}
		}
		#endregion
	}
}
