namespace FernBusinessBase.Interfaces.Processors
{
	/// <summary>
	/// Generic class starting a processing loop
	/// </summary>
	public interface ISimpleProcessor
	{
		bool Enabled { get; }
		bool Start();
		bool Started { get; }
		void Stop();
	}

	//public interface ISimpleProcessor<TEntity> : ISimpleProcessor
	//where TEntity : FullBusinessEntity, new()
	//{

	//}
}