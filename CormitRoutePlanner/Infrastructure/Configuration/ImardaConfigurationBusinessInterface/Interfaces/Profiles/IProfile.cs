using System;
using System.Collections.Generic;

namespace ImardaConfigurationBusiness.Interfaces.Profiles
{
	public interface IProfile
	{
		Guid ID { get; }
		Guid CompanyID { get; }
		Guid EntityID { get; }
		Guid ExtensionID { get; }		
		string Name { get; }
		string Description { get; }
		bool Active { get; }
		List<IProfileRule> Rules { get; set; }
		void InitializeRules();
		string Country { get; }
	}
}
