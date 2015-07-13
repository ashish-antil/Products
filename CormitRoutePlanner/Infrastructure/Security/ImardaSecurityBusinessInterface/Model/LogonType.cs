namespace ImardaSecurityBusiness
{
	public static class LogonType
	{
		public const byte UserLogon = 0;
		public const byte Impersonation = 1;
		public const byte UserLogoff = 2;
		public const byte SessionExpired = 3;
		public const byte AutoLogoff = 4;

	}
	public static class AuthenticationResult
	{
		public const short Undefined = -1;
		public const short Success = 0;
		public const short SecurityEntityNotFound = 1;
		public const short IACPermissionNotFound = 2;
		public const short LoginDisabled = 3;
		public const short WrongPassword = 4;
		public const short CompanyInactive = 5;

	}
}
