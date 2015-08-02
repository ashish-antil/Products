using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;


namespace RecipeService
{
	[RunInstaller(true)]
	public partial class ProjectInstaller : Installer
	{
		public ProjectInstaller()
		{
			InitializeComponent();
		}

		protected override void OnAfterInstall(IDictionary savedState)
		{
			base.OnAfterInstall(savedState);
			using (var serviceController = new ServiceController(serviceInstaller1.ServiceName, Environment.MachineName))
			{
				serviceController.Start();
			}
		}
	}
}
