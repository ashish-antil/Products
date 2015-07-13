using System;
using System.Collections.Generic;
using Imarda.Lib;
using Imarda.Logging;

namespace FernBusinessBase.Interfaces.Processors
{
	public abstract class SimpleMsmqProcessor<TEntity, TResult> : SimpleThreadProcessor<TEntity>, ISimpleMsmqProcessor<TEntity, TResult>
		where TEntity : FullBusinessEntity, new()
		where TResult : BaseEntity, new()
	{
		protected SimpleMsmqProcessor(ILogger log)
			: base(log)
		{
			QueueName = Name; //use type name
			CreateQueue();
		}

		protected SimpleMsmqProcessor(ILogger log, string queueName)
			: base(log)
		{
			QueueName = queueName;
			CreateQueue();
		}

		public string QueueName { get; private set; }
        public GenericMsmq<TEntity> Queue { get; protected set; }
        //public ImardaGenericMsmQueue<TEntity> Queue { get; protected set; }
		public abstract void OnProcessingCompleted(TResult[] results, TEntity[] entities);

		private void CreateQueue()
		{
			// ReSharper disable once UseObjectOrCollectionInitializer
            //Queue = new ImardaGenericMsmQueue<TEntity>(QueueName, Log);
            Queue = new GenericMsmq<TEntity>(QueueName, Log);
            //Queue.SerializationFunction = XmlUtils.Serialize;
            //Queue.DeserializationFunction = XmlUtils.Deserialize<TEntity>;
		}

		public override List<TEntity> GetEntitiesToProcess()
		{
			var entity = Queue.PeekMessage();
			var lst = new List<TEntity>();
			if (null != entity)
			{
				lst.Add(entity);
			}
			return lst;
		}

		public override bool ProcessEntity(TEntity entity)
		{
			GetResultsForEntity(OnProcessingCompleted, new[] {entity});
			return true;
		}

		protected abstract bool GetResultsForEntity(Action<TResult[], TEntity[]> resultAction, TEntity[] entities);
	}
}