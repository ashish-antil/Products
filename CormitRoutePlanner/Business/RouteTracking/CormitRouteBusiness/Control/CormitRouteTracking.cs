#region

using System.ServiceModel;
using FernBusinessBase;
using Imarda.Lib;
using System;
using Imarda.Logging;
#endregion

namespace Cormit.Business.RouteTracking
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public partial class CormitRouteTracking : BusinessBase, ICormitRouteTracking
	{
	    public static readonly Guid InstanceID = SequentialGuid.NewDbGuid();
	    public const string Description = "RouteTracking Business";

	    private readonly ErrorLogger _Log = ErrorLogger.GetLogger("RouteTracking");
		
		private DateTime? _ResumeCaching;

		

		public CormitRouteTracking()
		{
			try
			{

			}
			catch (Exception ex)
			{
				_Log.ErrorFormat("CormitRouteTracking Constructor Exception {0}", ex);
			}
		}

        //public override SimpleResponse<string> GetAttributes(IDRequest req)
        //{
        //    return GetAttributes<Unit>(req.ID); // can use any <T> that gets the right database connection! 
        //}

	}
}

