using System.Collections.Generic;

namespace FernBusinessBase.Interfaces.Processors
{
	internal interface ISimpleThreadProcessor<TEntity>
		where TEntity : FullBusinessEntity, new()
	{
		List<TEntity> GetEntitiesToProcess();
		bool ProcessEntity(TEntity entity);
		bool IsRunning { get; }
		int Sleep { get; }
	}
}