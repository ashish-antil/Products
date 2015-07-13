using System.ServiceModel;
using ImardaNotificationBusiness;
using FernBusinessBase;
using ImardaAttributingBusiness;
using ImardaCRMBusiness;
using ImardaConfigurationBusiness;

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
	#region ImardaNotificationService
	private Proxy<IImardaNotification> _IImardaNotificationProxy
			= new Proxy<IImardaNotification>("NotificationTcpEndpoint");
	public IImardaNotification IImardaNotificationProxy
	{
		get { return _IImardaNotificationProxy.GetChannel(); }
	}
	#endregion

	#region ImardaConfigurationService
	private Proxy<IImardaConfiguration> _IImardaConfigurationProxy
			= new Proxy<IImardaConfiguration>("ConfigurationTcpEndpoint");
	public IImardaConfiguration IImardaConfigurationProxy
	{
		get { return _IImardaConfigurationProxy.GetChannel(); }
	}
	#endregion

    #region ImardaAttributingService
    private Proxy<IImardaAttributing> _IImardaAttributingProxy
            = new Proxy<IImardaAttributing>("AttributingTcpEndpoint");
    public IImardaAttributing IImardaAttributingProxy
    {
        get { return _IImardaAttributingProxy.GetChannel(); }
    }
    #endregion
}


