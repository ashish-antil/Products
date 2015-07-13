using System;
using System.Collections.Generic;
using System.Text;

namespace Imarda360.Infrastructure.ConfigurationService
{
	public interface IConfigContext
	{
		string Locale {get;}
		Guid CompanyID { get;}
		Guid RoleTypeID { get; }
		Guid UserID { get; }
	}
}
