using Imarda.Logging;

namespace FernBusinessBase.Interfaces.Processors
{
	public interface ISimpleMsmqProcessor<TEntity,TResult>
		where TEntity : FullBusinessEntity, new()
		where TResult : BaseEntity, new()
	{
        GenericMsmq<TEntity> Queue { get; }
		void OnProcessingCompleted(TResult[] results, TEntity[] entities);
	}
}