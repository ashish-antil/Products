#region

using System.Runtime.Serialization;

#endregion

namespace ImardaBusinessBase
{
	[DataContract]
	public enum LoginMode : byte
	{
		[EnumMember]
		Normal = 0,
		
		[EnumMember]
		Relogin = 1,

		[EnumMember]
		IAC = 2,
		
		[EnumMember]
		Mobile = 18,
	}
}
