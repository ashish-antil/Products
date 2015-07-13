using System;
using FernBusinessBase;

#region

using System.Data.SqlClient;
using FernServiceBase;
using Imarda.Lib;

#endregion

// ReSharper disable once CheckNamespace
namespace ImardaAttributing
{
    public class ImardaAttributingService : FernBusinessServiceBase
    {
        /// <summary>
        ///     Passes the specific args for this Service to the base class.
        /// </summary>
        public ImardaAttributingService()
            : base("ImardaAttributing")
        {
            //RegisterHost(new ImardaServiceHost(typeof (ImardaAttributingBusiness.AttributeDefinition)));
            RegisterHost(new ImardaServiceHost(typeof(ImardaAttributingBusiness.ImardaAttributing)));
        }

        /// <summary>
        ///     Main program entry point.
        /// </summary>
        public static void Main()
        {
            Run(new ImardaAttributingService());
        }

        #region Methods to re-initialize, or update, the database.

        protected override void InternalUpdateDatabase(SqlConnection conn)
        {
        }

        #endregion
    }
}