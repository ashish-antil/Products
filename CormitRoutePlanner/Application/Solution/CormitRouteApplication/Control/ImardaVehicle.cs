using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using FernBusinessBase.Errors;

namespace ImardaVehicleBusiness
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public partial class ImardaVehicle : FernBusinessBase.BusinessBase, IImardaVehicle 
	{
		public static readonly Guid InstanceID = Guid.NewGuid();
		public const string Description = "I360 Vehicle Application";

		public GetUpdateCountResponseList GetUpdateObjectList(GetUpdateCountRequestList requestList)
		{
			GetUpdateCountResponseList responseList = new GetUpdateCountResponseList();
			responseList.List = new List<GetUpdateCountResponse>();

			if (requestList != null && requestList.List != null)
			{
				foreach (GetUpdateCountRequest req in requestList.List)
				{
					GetUpdateCountResponse response = new GetUpdateCountResponse();
					switch (req.TypeName)
					{
						case "Vehicle": response = GetVehicleUpdateCount(req); break;
						default:
							break;
					}
					if (response != null && response.Count > 0)
					{
						responseList.List.Add(new GetUpdateCountResponse(req.TypeName, response.Count, req.Priority));
					}
				}
			}

			return responseList;
		}
		#region Get Vehicle
		public GetItemResponse<Vehicles> GetVehicle(IDRequest request)
		{
			try
			{
				return GenericGetEntity<Vehicles>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetItemResponse<Vehicles>>(ex);
			}
		}
		#endregion
		#region GetVehicleUpdateCount
		public GetUpdateCountResponse GetVehicleUpdateCount(GetUpdateCountRequest request)
		{
			return GetVehicleUpdateCount(request, false);
		}

		public GetUpdateCountResponse GetVehicleUpdateCount(GetUpdateCountRequest request)
		{
			GetUpdateCountResponse response = new GetUpdateCountResponse();

			try
			{
				response = GenericGetEntityUpdateCount<Vehicles>("Vehicle", request.TimeStamp, true, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetUpdateCountResponse>(ex);
			}

			return response;
		}
		#endregion
		#region GetVehicleListByTimeStamp
		public GetListResponse<Vehicles> GetVehicleListByTimeStamp(GetListByTimestampRequest request)
		{
			return GetVehicleListByTimeStamp(request, false);
		}

		public GetListResponse<Vehicles> GetVehicleListByTimeStamp(GetListByTimestampRequest request)
		{
			try
			{
				return GenericGetEntityListByTimestamp<Vehicles>("Vehicle", request.TimeStamp, request.Cap, true, request.IncludeInactive, request.ID, request.LastRecordID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Vehicles>>(ex);
			}
		}
		#endregion
		#region GetVehicleList
		public GetListResponse<Vehicles> GetVehicleList(IDRequest request)
		{
			return GetVehicleList(request, false);
		}

		public GetListResponse<Vehicles> GetVehicleList(IDRequest request)
		{
			try
			{
				return GenericGetEntityList<Vehicles>(request);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle<GetListResponse<Vehicles>>(ex);
			}
		}
		#endregion
		#region Adam Test

		public GetListResponse<Vehicles> GetVehiclesList2(IDRequest request)
		{
			try
			{
				return GenericGetEntityList2 <Vehicles>("VehicleId");
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		#endregion
		#region Save Vehicle
		public BusinessMessageResponse SaveVehicle(SaveRequest<Vehicles> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				Vehicles entity = request.Item;
				response =
						GenericSaveEntity<Vehicles>("Vehicle", true,
		entity.VehicleID,
		entity.Description
		);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region SaveVehicleList
		public BusinessMessageResponse SaveVehicleList(SaveListRequest<Vehicles> request)
		{
			return SaveVehicleList(request, false);
		}

		public BusinessMessageResponse SaveVehicleList(SaveListRequest<Vehicles> request)
		{
			var response = new BusinessMessageResponse();
			try
			{
				foreach (Vehicles entity in request.List)
				{
					response =
						GenericSaveEntity<Vehicles>("Vehicle", true,
		entity.VehicleID,
		entity.Description
		);
				}
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
			return response;
		}
		#endregion
		#region Delete Vehicle
		public BusinessMessageResponse DeleteVehicle(IDRequest request)
		{
			return DeleteVehicle(request, false);
		}

		public BusinessMessageResponse DeleteVehicle(IDRequest request)
		{
			try
			{
				return GenericDeleteEntity<Vehicles>("Vehicle", request.ID);
			}
			catch (Exception ex)
			{
				return ErrorHandler.Handle(ex);
			}
		}
		#endregion
	}
}