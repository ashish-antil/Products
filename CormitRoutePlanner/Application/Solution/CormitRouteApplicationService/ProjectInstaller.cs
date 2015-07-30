using System;
using System.Collections.Generic;
using System.Text;
using FernServiceBase;

namespace Cormit.Application.RouteApplication
{
	public class ProjectInstaller : BaseProjectInstaller {
		public ProjectInstaller()
			: base("ImardaApplication", "ImardaApplicationService",
			"Imarda 360 Fleet Tracking Solution"
			) { }
	}
}