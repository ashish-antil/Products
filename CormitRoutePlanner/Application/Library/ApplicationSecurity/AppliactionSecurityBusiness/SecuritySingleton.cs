namespace Imarda360Application.Security
{
	partial class ImardaSecurity
	{
		#region Singleton

		static ImardaSecurity() 
		{
		}

		private static readonly IImardaSecurity _Instance = new ImardaSecurity();

		public static IImardaSecurity Instance { get { return _Instance; } }

		private ImardaSecurity()
		{
		}

		#endregion
	}
}
