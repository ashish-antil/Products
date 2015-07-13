using FernBusinessBase;
using Imarda360Application;


public class ImardaProxyManager
{
	#region ImardaProxyManager is a Singleton

	public static ImardaProxyManager Instance
	{
		get
		{
			if (_Instance == null) _Instance = new ImardaProxyManager();
			return _Instance;
		}
	}

	private static ImardaProxyManager _Instance;

	/// <summary>
	/// Constructs the one and only ImardaProxyManager.
	/// </summary>
	private ImardaProxyManager()
	{
	}

	#endregion
	#region IImarda360Service
	private Proxy<IImarda360> _IImarda360Proxy
			= new Proxy<IImarda360>("SolutionTcpEndpoint");
	public IImarda360 IImarda360Proxy
	{
		get { return _IImarda360Proxy.GetChannel(); }
	}

	#endregion


}


