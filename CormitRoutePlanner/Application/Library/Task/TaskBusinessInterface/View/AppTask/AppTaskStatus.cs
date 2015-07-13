using System.Runtime.Serialization;

namespace Imarda360Application.Task
{
	[DataContract]
	public enum AppTaskStatus : byte
	{
		[EnumMember]
		New = 0, // task has been created but is not scheduled yet
		[EnumMember]
		Queued = 1, // waiting for exeuction
		[EnumMember]
		QueueError = 2, // could not schedule
		[EnumMember]
		Success = 3, // successfully processed
		[EnumMember]
		Failure = 4, // failure trying to process
		[EnumMember]
		Incomplete = 5, // the work may be incomplete and should be looked at
		[EnumMember]
		Retries = 6, // too many retries, all failed
		[EnumMember]
		Exception = 7, // some exception in the tasks's work routine happened
		[EnumMember]
		TimedOut = 8, // took too long
		[EnumMember]
		Cancelled = 9, // removed by client.
	}
}