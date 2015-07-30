
using System;
using System.Collections.Generic;
using System.ServiceModel;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Lib;

using ImardaTaskBusiness;

namespace Cormit.Application.RouteApplication.Task
{
	/// <summary>
	/// This partial class handles the fixing of empty addresses in the unit trace.
	/// </summary>
	partial class i360TaskManager //AddressFixer
	{
		private const string OverQueryLimit = "OVER_QUERY_LIMIT";
		private const string ZeroResults = "ZERO_RESULTS";
		private const string GisCallFail = "GIS_CALL_FAIL";
		private static readonly TimeSpan _Delay = ConfigUtils.GetTimeSpan("AddressLookupDelay", TimeSpan.FromMinutes(30));
		private static readonly int _TaskPause = ConfigUtils.GetInt("AddressLookupTaskPause", 250);
		private static readonly byte _TaskManagerID = (byte)ConfigUtils.GetInt("AppTaskManagerID", 1); // = i360 application service task manager

		/// <summary>
		/// Lookup L/L in GIS and save to unit trace. If that fails, retry the task later by saving it with a later start time.
		/// </summary>
		/// <param name="task"></param>
		
		public static void ScheduleAddressTask(AddressTask task)
		{
			ScheduledTask stask = AppTaskHelper.Convert(task);
			stask.ID = SequentialGuid.NewDbGuid();
			stask.Arguments = XmlUtils.Serialize(task.Arguments, false);
			stask.StartTime = DateTime.UtcNow + _Delay;
			stask.DueTime = stask.StartTime + TimeSpan.FromMinutes(1);
			stask.Pause = _TaskPause;
			stask.ProgramID = (int) Programs.AddressFixer;
			stask.AlgorithmID = (byte) Algorithms.Address;
			stask.ManagerID = _TaskManagerID;

			try
			{
				var service = ImardaProxyManager.Instance.IImardaTaskProxy;
				ChannelInvoker.Invoke(delegate(out IClientChannel channel)
				{
					channel = service as IClientChannel;
					var response = service.SaveScheduledTask(new SaveRequest<ScheduledTask>(stask));
					ErrorHandler.Check(response);
				});
			}
			catch (Exception ex)
			{
				_Log.Error(ex);
			}
		}

	
	}
}