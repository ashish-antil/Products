using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace SecurityService {
	[RunInstaller(true)]
	public partial class ProjectInstaller : FernServiceBase.BaseProjectInstaller {
		public ProjectInstaller()
			: base("ImardaSecurity", "ImardaSecurityService",
			"Provides Access Control and Security Entity/Group Information") { }
	}
}