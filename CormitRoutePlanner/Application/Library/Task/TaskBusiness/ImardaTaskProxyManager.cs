//using System.Web.Services.Description;
using FernBusinessBase;

namespace Imarda360Application.Task
{
	public class ImardaTaskProxyManager
	{
		#region ImardaTaskProxyManager is a Singleton

		public static ImardaTaskProxyManager Instance
		{
			get
			{
				if (_Instance == null) _Instance = new ImardaTaskProxyManager();
				return _Instance;
			}
		}

		private static ImardaTaskProxyManager _Instance;

		/// <summary>
		/// Constructs the one and only ImardaTaskProxyManager.
		/// </summary>
		private ImardaTaskProxyManager()
		{

		}

		#endregion

		private Proxy<ImardaTaskBusiness.IImardaTask> _IImardaTaskProxy
			= new Proxy<ImardaTaskBusiness.IImardaTask>("TaskTcpEndpoint");
		public ImardaTaskBusiness.IImardaTask IImardaTaskProxy
		{
			get { return _IImardaTaskProxy.GetChannel(); }
		}

		/// <summary>
		/// Access to the Proxy required for the TaskManager.
		/// </summary>
		/// <returns></returns>
		public Proxy<ImardaTaskBusiness.IImardaTask> GetTaskServiceProxy()
		{
			return _IImardaTaskProxy;
		}
	}
}

