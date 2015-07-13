using System;
using System.ServiceModel;
using FernBusinessBase;
using FernBusinessBase.Extensions;
using Cormit.Application.Tracking;

namespace CormitRouteTrackingApplicationLib.Tracking
{
	partial class CormitRouteTracking
	{
		#region Singleton

		static CormitRouteTracking()
		{
		}

        private static readonly ICormitRouteTracking _Instance = new CormitRouteTracking();

		public static ICormitRouteTracking Instance { get { return _Instance; } }

        private CormitRouteTracking()
		{
			
		}

		#endregion
	}
}
