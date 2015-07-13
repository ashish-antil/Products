using System;
using System.Collections.Generic;
using System.Text;
using FernServiceBase;

namespace Imarda360Application
{
	public class ProjectInstaller : BaseProjectInstaller {
		public ProjectInstaller()
			: base("ImardaApplication", "ImardaApplicationService",
			"Imarda 360 Fleet Tracking Solution"
			) { }
	}
}