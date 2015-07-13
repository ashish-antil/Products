using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Imarda.Logging;

namespace FernBusinessBase.Interfaces.Processors
{
	/// <summary>
	/// Generic class to process some entity
	/// </summary>
	public abstract class SimpleThreadProcessor<TEntity> : SimpleProcessorBase, ISimpleThreadProcessor<TEntity> ,ILoggable
		where TEntity: FullBusinessEntity, new()
	{
		private Thread _processingThread;
		private const int DefaultSleep = 100;

		protected SimpleThreadProcessor(ILogger log)
			:base(log)
		{
			Sleep = DefaultSleep;
		}

		protected override bool StartProcessing()
		{
			try
			{
				_processingThread = new Thread(ThreadStart) { Name = Name };
				IsRunning = true;
				_processingThread.Start();
				Log.VerboseFormat("{0} - Starting Processing Thread", Name);
			}
			catch (Exception e)
			{
				Log.Error(e);
				return false;
			}
			return true;
		}

		protected override void StopProcessing()
		{
			IsRunning=false;
		}

		protected virtual void HandleProcessEntityResult(TEntity entity, bool result)
		{
			Log.VerboseFormat("{0} - Processing returns {1} for entity: {2}", Name, result, entity.ToString());
		}

		public abstract bool ProcessEntity(TEntity entity);

		public abstract List<TEntity> GetEntitiesToProcess();

		private void ThreadStart(object state)
		{
			while (IsRunning)
			{
				Thread.Sleep(Sleep);
				var entities = GetEntitiesToProcess();
				if (entities.Any())
				{
					foreach (var entity in entities)
					{
						Log.VerboseFormat("{0} - Processing entity: {1}", Name,entity.ToString());
						var result = ProcessEntity(entity);
						HandleProcessEntityResult(entity, result);
					}
				}
				else
				{
					Log.VerboseFormat("{0} - No entities to process..", Name);
				}
			}
		}

		public bool IsRunning { get; protected set; }
		public int Sleep { get; protected set; }
	}

}
