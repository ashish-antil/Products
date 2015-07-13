#region

using System.Data.SqlClient;
using FernServiceBase;
using Imarda.Lib;

#endregion

// ReSharper disable once CheckNamespace
namespace ImardaTracking
{
    public class ImardaTrackingService : FernBusinessServiceBase
    {
        /// <summary>
        ///     Passes the specific args for this Service to the base class.
        /// </summary>
        public ImardaTrackingService()
            : base("CormitRouteTracking")
        {
            RegisterHost(new ImardaServiceHost(typeof (Cormit.Business.RouteTracking.CormitRouteTracking)));
        }

        /// <summary>
        ///     Main program entry point.
        /// </summary>
        public static void Main()
        {
            Run(new ImardaTrackingService());
        }

        #region Methods to re-initialize, or update, the database.

        protected override void InternalUpdateDatabase(SqlConnection conn)
        {
        }

        #endregion
    }
}