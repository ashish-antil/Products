using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using FernBusinessBase;
using Imarda.Lib;

namespace ImardaSecurityBusiness {
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public partial class ImardaSecurity : FernBusinessBase.BusinessBase, IImardaSecurity 
	{
		public static readonly Guid InstanceID = SequentialGuid.NewDbGuid();
		public const string Description = "Security Infrastructure";

	}
}
