using Imarda.Lib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Messaging;
using System.Threading;

// ReSharper disable once CheckNamespace
namespace Imarda.Logging
{
	/// <summary>
	/// This class manages a generic MSMQueue
	/// It is used by RSL (Road Speed Limits)
	/// </summary>
	/// <typeparam name="T"></typeparam>
    public class ImardaGenericMsmQueue<T> : Lib.ImardaMSMQueue /*, IGenericMsmq<T>*/
		where T : class, new()
	{
		private const string QueueFolder = @".\private$\";
		private const int SafeCountTimeOut = 3000;
		private const int QuickSafeCountTimeOut = 100;
		private const int SafeCountRetry = 3;
		private readonly object _lockSend;
		private readonly ILogger _log;
		private readonly string _queueName;
		private IEqualityComparer<T> _equalityComparer;
		private Func<string, T, bool> _processMsgFunc;
		private Thread _queueThread;
		private bool _run;
		private bool _runSynchronousDump;

		public ImardaGenericMsmQueue(string queueName, ILogger log)
			: base(QueueFolder + queueName)
		{
			_queueName = queueName;
			_log = log;
			_lockSend = new object();
		}

		public IEqualityComparer<T> EqualityComparer { get; protected set; }

		public Func<string, T, bool> ProcessMsgFunc { get; protected set; }

		public bool IsRunning
		{
			get { return _run; }
		}

		public void Stop()
		{
			_log.Info(_queueName + " Stop");
			_run = false;
		}

		/// <summary>
		/// Starts process the queue using the Processing function provided
		/// </summary>
		/// <param name="processMsgFunc">called with the message body and message id and must return true for the message to be removed from the queue and processing to continue</param>
		/// <param name="equalityComparer">equality comparer used to filter duplicate if none is provided, string comparison is used on message body ToString</param>
		/// <param name="runSynchronousDump">used to synchronously process the whole queue on caller's thread, used to dump a storage queue in another one on restart for instance</param>
		public void StartProcessingQueue(Func<string, T, bool> processMsgFunc, IEqualityComparer<T> equalityComparer = null, bool runSynchronousDump = false)
		{
			if (null == processMsgFunc)
			{
				throw new ArgumentException("processMsgFunc");
			}
			EqualityComparer = _equalityComparer = equalityComparer;
			_runSynchronousDump = runSynchronousDump;
			_run = true;
			_log.Info(_queueName + " Start Processing Queue");

			ProcessMsgFunc =_processMsgFunc = processMsgFunc;

			if (runSynchronousDump)
			{
				ProcessQueue();
			}
			else
			{
				_queueThread = new Thread(ProcessQueue) {Name = "RslSpeedCheckMsgProcessQueue"};
				_queueThread.Start();
			}
		}

		/// <summary>
		/// Remove a message anywhere in the queue based on its Id
		/// </summary>
		/// <param name="msgId"></param>
		/// <returns>True if message was found and removed</returns>
		public bool Remove(string msgId)
		{
			try
			{
				return null != ReceiveById(msgId);
			}
			catch (Exception x)
			{
				_log.ErrorFormat("Remove msgId: {0} Exception: {1}", new object[] { msgId, x.Message });
				_log.Debug("StackTrace:");
				_log.Debug(Environment.StackTrace);
				return false;
			}
		}

		public string SendWithId(T item)
		{
			try
			{
				lock (_lockSend)
				{
					var body = /*Serialize(item);//*/ StringUtils.ObjectToString(item); //to base64 string
					return SendWithId(body);
				}
			}
			catch (Exception x)
			{
				_log.Error(x);
				Debug.WriteLine(x);
			}
			return null;
		}

		public void Send(T item)
		{
			try
			{
				lock (_lockSend)
				{
					var body = /*Serialize(item);//*/StringUtils.ObjectToString(item); //to base64 string
					Send(body);
				}
			}
			catch (Exception x)
			{
				_log.Error(x);
                //Debug.WriteLine(x);
			}
		}

		public bool HasMessages()
		{
			try
			{
				var result = false;
				var i = 0;
				while (i < SafeCountRetry)
				{
					var peek = SafePeek(true, SafeCountTimeOut);
					if (null != peek)
					{
						result = true;
						break;
					}
					i++;
				}
				return result;
			}
			catch (Exception ex)
			{
				_log.Error(ex);
			}
			return false;
		}

		public new void Receive()
		{
			base.Receive();
		}

		[Obsolete]
		public int GetSafeCount()
		{
			return GetSafeCount(SafeCountTimeOut, SafeCountRetry);
		}

		[Obsolete]
		public int GetSafeCount(bool quickCount)
		{
			return GetSafeCount(QuickSafeCountTimeOut, SafeCountRetry);
		}

		/// <summary>
		///     Attempts to read Queue count for a given number of times with a time out
		///     Used when counting the queue on start is critical but delay is not
		///		The override with quickCount use a much smaller timeout 
		/// </summary>
		/// <param name="timeOut"></param>
		/// <param name="numRetries"></param>
		/// <returns></returns>
		[Obsolete]
		private int GetSafeCount(int timeOut, int numRetries)
		{
			try
			{
				/*throw new NotImplementedException();*/
				var result = -1;
				var i = 0;
				while (i < SafeCountRetry)
				{
					result = MSMQCount();
					if (result >= 0)
					{
						break;
					}
					i++;
					Thread.Sleep(SafeCountTimeOut);
				}
				return result;
			}
			catch (Exception ex)
			{
				_log.Error(ex);
			}
			return -1;
		}

		protected Message SafePeek(bool nonBlockingPeek, int timeOutMs = 0)
		{
			//Note: Peek() w/o time span waits indefinitely and blocks the thread
			//Peek w or w/o timespan always returns the 1st message in the queue

			Message peek = null;

			//if _runSynchronousDump we don't want to wait indefinitely for peek to return when the queue is empty
			if (nonBlockingPeek)
			{
				try
				{
					var timeOut = timeOutMs > 0 ? new TimeSpan(0, 0, 0, 0, timeOutMs) : TimeSpan.Zero;
					peek = Peek(timeOut) as Message;
				}
				catch (MessageQueueException e)
				{
					//bury the timeout generated exception otherwise rethrow
					if (e.MessageQueueErrorCode != MessageQueueErrorCode.IOTimeout)
					{
						throw;
					}
				}
			}

			else
			{
/*				//we want this to crash in debug instead of simply being "silently" null
#if DEBUG
				try
				{
					peek = (Message)Peek();
				}
				catch (Exception)
				{
					throw;
				}
#else*/
				peek = Peek() as Message;
/*#endif*/
			}

			return peek;
		}

		private void ProcessQueue()
		{
			T lastMessage = null;

			while (_run)
			{
				try
				{
					var peek = SafePeek(_runSynchronousDump);

					if (peek != null)
					{
						//Formatter was erratically returning xml desserialization error hence string64 instead of T
						//peek.Formatter = new XmlMessageFormatter(new[] { typeof(T) });
						peek.Formatter = new XmlMessageFormatter(new[] {typeof (string)});
						var s = peek.Body as string;

                        var message = StringUtils.StringToObject(s) as T; //from base64 string

                        //var message = Deserialize(s);

						if (message != null)
						{
							var checkDuplicate = new Func<T, T, bool>((tCurrent, tLast) => (null == _equalityComparer)
								                                                               ? 0 == String.CompareOrdinal(tCurrent.ToString(), tLast.ToString())
								                                                               : _equalityComparer.Equals(tCurrent, tLast));

							if (null != lastMessage && checkDuplicate(message, lastMessage)) //check duplicate
							{
								Receive();
							}
							else
							{
								if (_processMsgFunc(peek.Id, message))
								{
									Receive();
									lastMessage = message;
								}
								else
								{
									lastMessage = null; // msg is re-read from queue and should not be rejected as duplicate
									Thread.Sleep(1000);
								}
							}
						}
					}
					else
					{
						// Allow for possible queue outages
						Thread.Sleep(1000);

						if (_runSynchronousDump)
						{
							//stop processing the queue when no more message to retrieve
							_run = SafePeek(_runSynchronousDump) != null;
							if (!_run) //finished dumping so erase reference to last message as queue is now empty
							{
								lastMessage = null;
							}
						}
					}
				}
				catch (MessageQueueException ex)
				{
					HandleInternal(ex);
					Thread.Sleep(5000);
				}
				catch (Exception ex)
				{
					HandleInternal(ex);
					Thread.Sleep(1000);
				}
			} //while
		}

		protected void HandleInternal(Exception ex)
		{
			try
			{
				_log.ErrorFormat("Imarda Framework Exception {0}", ex);
			}
			// ReSharper disable once EmptyGeneralCatchClause
			catch
			{
				// well, now what? can't even log the error
			}
		}


	}
}