using System;
using System.Collections.Generic;
using System.Text;
using FernBusinessBase;
using FernBusinessBase.Errors;
using System.ServiceModel;
using Imarda.Lib;
namespace ImardaTrackingBusiness
{
	partial class ImardaTracking
	{

		public BusinessMessageResponse ProcessDeviceUpdate(ProcessDeviceUpdateRequest request)
		{
			var response = new BusinessMessageResponse();
			try
			{

				GetItemResponse<Unit> unitResponse = new GetItemResponse<Unit>();
				unitResponse = GetUnitByTrackID(new TrackIDRequest(request.Update.TrackID));
				//unitResponse.Item.ID

				if (unitResponse.Item != null)
				{

					// Add Unit Trace
					UnitTrace unitTrace = new UnitTrace();

					Update update = request.Update;

					unitTrace.OwnerID = unitResponse.Item.OwnerID;
					unitTrace.UnitID = unitResponse.Item.ID;
					unitTrace.CompanyID = unitResponse.Item.CompanyID;
					unitTrace.Active = true;
					unitTrace.ID = SequentialGuid.NewDbGuid();
					unitTrace.SpeedKph = decimal.Parse(update.Speed);
					unitTrace.PeakSpeedKph = decimal.Parse(update.PeakSpeed);
					unitTrace.TrackID = update.TrackID;
					unitTrace.Hdop = byte.Parse(update.HDOP);
					float rssi;
					if (!float.TryParse(update.RSSI, out rssi)) rssi = 0f;
					unitTrace.Rssi = rssi;
					//unitTrace.MsgID = update.MessageID;

					/*if (unitTrace.MsgID = "I") 
					{
							unitTrace.Ignition = true;
					}
					else
					{
							unitTrace.Ignition = false;
					}*/

					unitTrace.IPAddress = update.Host;
					unitTrace.Direction = update.Direction;
					unitTrace.EventType = update.MessageType;
					unitTrace.Lat = decimal.Parse(update.Lat);  //Replace with safe parse
					unitTrace.Lng = decimal.Parse(update.Lon);  //Replace with safe parse
					unitTrace.DisplayDate = DateTime.UtcNow;
					unitTrace.GPSDate = DateTime.UtcNow;
					unitTrace.ReceivedDate = DateTime.UtcNow;
					SaveRequest<UnitTrace> saveRequest = new SaveRequest<UnitTrace>();
					saveRequest.Item = unitTrace;
					response = SaveUnitTrace(saveRequest);



				}
				else
				{
					response.ErrorCode = "99";
					response.Status = false;
					response.StatusMessage = "ProcessDeviceUpdate Failed";
				}
			}
			catch
			{
				response.ErrorCode = "99";
				response.Status = false;
				response.StatusMessage = "ProcessDeviceUpdate Failed";
			}

			return response;




			/* request.Update.
			/*var response = new BusinessMessageResponse();
			*/

			//UnitTrace_000 unittrace = new UnitTrace_000();
			//unittrace.UnitID = request.Update.DeviceID;

			//SaveRequest<UnitTrace_000> saveRequest = new SaveRequest<UnitTrace_000>();
			//saveRequest.Item = unittrace;
			// response = SaveUnitTrace_000(saveRequest);

		}
	}
}
