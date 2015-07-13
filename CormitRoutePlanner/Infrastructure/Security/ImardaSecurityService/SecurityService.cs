using System;
using System.Collections.Generic;
using System.ServiceModel;
using ImardaConfigurationBusiness;
using ImardaSecurityBusiness;
using Imarda.Lib;
using FernBusinessBase;

namespace Security {

	public class SecurityService : FernServiceBase.FernServiceBase {

		public SecurityService()
			: base("ImardaSecurity")
		{
	  RegisterHost(new ImardaServiceHost(typeof(ImardaSecurityBusiness.ImardaSecurity)));
			//AuthenticationServer server = new AuthenticationServer();
		}

		public static void Main() {
			System.ServiceProcess.ServiceBase[] ServicesToRun;
			// Change the following line to match.
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new SecurityService() };
			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary>
		/// Run startup scripts
		/// </summary>
		/// <param name="conn"></param>
		/*protected override void internalUpdateDatabase(System.Data.SqlClient.SqlConnection conn) {
			
		}*/

		protected override void OnStart(string[] args)
		{
			base.OnStart(args);

			// Initialize session cache with object expiry time
			IImardaConfiguration service = ImardaProxyManager.Instance.IImardaConfigurationProxy;
			ChannelInvoker.Invoke(delegate(out IClientChannel channel)
			{
				channel = service as IClientChannel;
				var request = new ConfigRequest(new Guid("fd17ad5d-1844-4737-bce0-451a90182fcf"), null);
				var response = service.GetConfigValue(request);
				if (response.Status)
				{
					int expiration = response.Item.As<int>(30);
					if (expiration > 0) SessionObjectCache.Instance.Expiration = TimeSpan.FromMinutes(expiration);
				}
			});
		}
	}

}