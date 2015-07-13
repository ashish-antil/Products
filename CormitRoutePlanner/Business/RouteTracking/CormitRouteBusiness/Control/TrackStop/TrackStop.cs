#region

using System;
using FernBusinessBase;
using FernBusinessBase.Errors;

#endregion

// ReSharper disable once CheckNamespace
namespace ImardaTrackingBusiness
{
	partial class ImardaTracking
	{

		public BusinessMessageResponse SaveTrackStopList(SaveListRequest<TrackStop> request)
		{
			throw new NotImplementedException();
		}

		public GetListResponse<TrackStop> GetTrackStopListsByTrackerId(IDRequest request)
		{
			var response = new GetListResponse<TrackStop>();
			try
			{
				var db = ImardaDatabase.CreateDatabase(Util.GetConnName<Unit>());

				var args = new object[] {request.ID};

				using (var dr = db.ExecuteDataReader("SPGetTrackStopListsByTrackId", args))
				{
					while (dr.Read())
					{
						var t = GetFromData<TrackStop>(dr);
						response.List.Add(t);
					}
					return response;
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<TrackStop>>(ex);
			}
		}
	}
}