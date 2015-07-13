namespace FernBusinessBase.Interfaces.ProcessingExtensions
{
	/// <summary>
	/// Defines service methods used to process extension data
	/// </summary>
	public interface IExtensionProcessor
	{
		bool ProcessExtensionData<TEntity>(TEntity data) where TEntity : BaseEntity, IExtensionProcessingData,new();
		void ExecuteCommand(object param);
	}

}
