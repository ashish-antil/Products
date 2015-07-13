using System;

namespace FernBusinessBase.Interfaces.ProcessingExtensions
{
	public interface IProcessingExtension
	{
		Guid ExtensionId { get; }
		void ExecuteCommand(object param);
	}
}