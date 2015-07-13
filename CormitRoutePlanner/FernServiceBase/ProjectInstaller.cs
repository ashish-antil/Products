using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace FernServiceBase {
	[RunInstaller(true)]
	public partial class BaseProjectInstaller : Installer {
		public BaseProjectInstaller() {
			InitializeComponent();
		}

		public BaseProjectInstaller(string name, string displayName, string description) {
			InitializeComponent();
			serviceInstaller1.Description = description;
			serviceInstaller1.DisplayName = displayName;
			serviceInstaller1.ServiceName = name;
		}

        //& SPII-44
        public BaseProjectInstaller(string name, string displayName, string description, bool delayedStart = true) {
			InitializeComponent();
			serviceInstaller1.Description = description;
			serviceInstaller1.DisplayName = displayName;
			serviceInstaller1.ServiceName = name;
            serviceInstaller1.DelayedAutoStart = delayedStart;
        }
        //. SPII-44

		public BaseProjectInstaller(string name, string displayName, string description, string[] ServicesDependedOn)
		{
			InitializeComponent();
			serviceInstaller1.Description = description;
			serviceInstaller1.DisplayName = displayName;
			serviceInstaller1.ServiceName = name;
			serviceInstaller1.ServicesDependedOn = ServicesDependedOn;
		}
	}
}