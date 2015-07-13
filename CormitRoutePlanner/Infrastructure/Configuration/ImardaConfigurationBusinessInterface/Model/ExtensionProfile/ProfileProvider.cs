using FernBusinessBase;

namespace ImardaConfigurationBusiness.Model.ExtensionProfile
{
	/// <summary>
	/// The profileProvider defines the service which will load the profiles from the DB and provides methods to do so
	/// </summary>
	public class ProfileProvider : ProfileProviderBase
	{
		public ProfileProvider(Proxy<IImardaConfiguration> proxy) : base(proxy)
		{
		}
	}
}
