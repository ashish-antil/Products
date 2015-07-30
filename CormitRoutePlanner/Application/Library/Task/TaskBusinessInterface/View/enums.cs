namespace Cormit.Application.RouteApplication.Task
{
	public enum Algorithms : byte
	{
		Default = 0,
		Reports = 10,
		Alerts = 20,
		System = 40,
		Address = 50, 
#if TaskHandler
		`Task` = `ProgramNumber`0,
#endif
	}

	public enum Programs
	{
		Unspecified = 0,
		ReportHandler = 1,
		AlertHandler = 2,
		SystemEventHandler = 4,
		AddressFixer = 5, 
#if TaskHandler
		`ProgramName` = `ProgramNumber`,
#endif
	}

	public static class TaskManagerParameters
	{
		public static byte ManagerID = 1;
	}
}
