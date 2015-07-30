using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using FernBusinessBase.Errors;
using Imarda.Lib;
using ImardaTaskBusiness;


namespace Cormit.Application.RouteApplication.Task
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public partial class ImardaTask : FernBusinessBase.BusinessBase, IImardaTask
	{
		public static readonly Guid InstanceID = SequentialGuid.NewDbGuid();
		public const string Description = "Imarda Task Scheduling";

	}
}

