using System;

namespace ImardaConfigurationBusiness.Interfaces.Profiles
{
	public interface IProfileRule
	{
		Guid ID { get; }
		Guid ProfileId { get; }
		Guid ParentId { get; }
		Guid ValueKindId { get; }
		string ValueKindName { get; }
		string Description { get; }
		string Value1 { get; }
		string Value2 { get; }
	}
}
