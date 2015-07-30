using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FernBusinessBase;

using System.ServiceModel;
using FernBusinessBase.Errors;


namespace Cormit.Application.RouteApplication
{
	/// <summary>
	/// Declaring the class FacadeImplementation to be an IImardaSolution. 
	/// </summary>
	public partial class FacadeImplementation : IImardaSolution
	{
		
		#region Singleton

		static FacadeImplementation()
		{
		}

		private static readonly IImardaSolution _Instance = new FacadeImplementation();

		public static IImardaSolution Instance { get { return _Instance; } }
		
		private FacadeImplementation()
		{
		}
		

		#endregion


	
	}
}
