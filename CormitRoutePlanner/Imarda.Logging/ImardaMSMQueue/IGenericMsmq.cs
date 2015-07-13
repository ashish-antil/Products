using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Imarda.Logging
{
	public interface IGenericMsmq<T>
		where T : class, new()
	{
		void StartProcessingQueue(Func<string, T, bool> processMsgFunc, IEqualityComparer<T> equalityComparer, bool runSynchronousDump);
		void Send(T item);
		void Stop();
		bool HasMessages();
		string SendWithId(T item);
		bool Remove(string msgId);
		void Receive();
        //Func<T, string> SerializationFunction { get; }
        //Func<string, T> DeserializationFunction { get; }
		T PeekMessage(); 
	}
}
