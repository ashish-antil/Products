using System;
using Imarda360.Infrastructure.ConfigurationService;
using ImardaConfigurationBusiness;
using FernBusinessBase;
using Imarda360.Infrastructure.ConfigurationService.ConfigDataTypes;

public class ConfigProxyContext : ConfigServiceContext, IConfigData
{
	private IImardaConfiguration _Proxy;

	public ConfigProxyContext(BaseContext ctx)
		: base(null, false, ctx)
	{
		_Proxy = ImardaProxyManager.Instance.IImardaConfigurationProxy;
		DataProvider = this;
	}

	protected override void Subdispose()
	{
		((IDisposable)_Proxy).Dispose();
	}


	public ConfigItem FindItem(object appParameter, Guid itemID)
	{
		IImardaConfiguration channel = _Proxy;
		ConfigRequest request = new ConfigRequest(itemID, appParameter, Context.Levels);
		GetItemResponse<ConfigValue> response = channel.GetConfigValue(request);
		if (response.Status)
		{
			var cv = response.Item;
			var version = ConfigItemVersion.Create(cv.Type, cv.Value, false, cv.UID);
			return new ConfigItem { ID = itemID, Versions = new ConfigItemVersion[] { version }, Parameter = CfgSystem.First };
		}
		else return null;
	}



	#region IConfigData Members

	public ConfigItem FindItem(Guid itemID)
	{
		return FindItem(null, itemID);
	}

	public ConfigItemVersion FindDefaultItemVersion(Guid itemID)
	{
		throw new System.NotImplementedException();
	}

	public int AddDefaultItem(Guid itemID, object data, bool update)
	{
		throw new System.NotImplementedException();
	}

	#endregion
}

