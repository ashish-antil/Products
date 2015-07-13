using System;
using System.ServiceModel;
using FernBusinessBase;
using FernBusinessBase.Errors;
using ImardaAlertingBusiness;

namespace Imarda360Application.Task
{
	/// <summary>
	/// This is the task handing program: AddressFixer.
	/// </summary>
	partial class i360TaskManager
	{
		private void RunAddress(AddressTask task)
		{
			_Log.InfoFormat("Run Address {0}", task.ID);

			AddressTask.Args args = task.Arguments;

			BusinessMessageResponse resp = null;
			//TODO fill in the body of the program here
			ErrorHandler.Check(resp);
		}
	}
}
