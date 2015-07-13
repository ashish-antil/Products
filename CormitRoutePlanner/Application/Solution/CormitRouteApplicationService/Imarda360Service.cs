using Imarda.Lib;

namespace Imarda360Application
{

	public class Imarda360Service : FernServiceBase.FernServiceBase
	{

		/// <summary>
		/// Passes the specific args for this Service to the base class
		/// </summary>
		public Imarda360Service() : base("ImardaSolutionService")
		{
			RegisterHost(new ImardaServiceHost(typeof(Imarda360)));
			//RegisterHost(new ImardaServiceHost(typeof(Security.ImardaSecurity)));
		}

		/// <summary>
		/// Main program entry point
		/// </summary>
		public static void Main()
		{
			Run(new[] {new Imarda360Service()});
		}
	}

}