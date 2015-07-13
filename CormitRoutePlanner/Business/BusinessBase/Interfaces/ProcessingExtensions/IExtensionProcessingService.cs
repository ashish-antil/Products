using System.ServiceModel;

namespace FernBusinessBase.Interfaces.ProcessingExtensions
{
	/// <summary>
	/// Defines methods expected by extension from service
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	[ServiceContract]
	public interface IExtensionProcessingService<TEntity>
		where TEntity : BaseEntity, IExtensionProcessingData, new()
	{
		[OperationContract]
		void ProcessExtensionData(SaveRequest<TEntity> request);
		[OperationContract]
		void ExecuteCommand(ObjectRequest request);
	}
}