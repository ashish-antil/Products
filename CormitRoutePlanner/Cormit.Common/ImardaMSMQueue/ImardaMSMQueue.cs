using System.Diagnostics;
using System.Messaging;
using System.Threading;
using System;
//using MSMQ;

namespace Imarda.Lib
{
    public class ImardaMSMQueue
    {
        #region private

        private readonly string _QueueName;
        private MessageQueue _Queue;

        public IMessageFormatter Formatter
        {
            get { return _Queue.Formatter; }
        }

        public static bool Exists(string name)
        {
            return MessageQueue.Exists(name);
        }

        private void CreateMessageQueue()
        {
            if (MessageQueue.Exists(_QueueName))
            {
                _Queue = new MessageQueue(_QueueName);
            }
            else
            {
                _Queue = MessageQueue.Create(_QueueName, false);

                var acl = new AccessControlList();

                PrepareAccessControlEntries(acl);

                _Queue.SetPermissions(acl);
                _Queue.DefaultPropertiesToSend.Recoverable = true;

                _Queue.Formatter = new XmlMessageFormatter();
                //_queue.Formatter = new XmlMessageFormatter(new Type() { typeof(System.String) });
            }
        }

        private static void PrepareAccessControlEntries(AccessControlList acl)
        {
            string aclEntry = @"\Administrators";
            AddACLEntry(acl, aclEntry);

            aclEntry = @"\Everyone";
            AddACLEntry(acl, aclEntry);
        }

        private static void AddACLEntry(AccessControlList acl, string aclEntry)
        {
            var ace = new AccessControlEntry();
            ace.Trustee = new Trustee(aclEntry);
            ace.EntryType = AccessControlEntryType.Set;
            ace.GenericAccessRights = GenericAccessRights.All;
            ace.StandardAccessRights = StandardAccessRights.All;
            acl.Add(ace);
        }

        /*private object GetSyncLock()
        {
            // Interlocked.CompareExchange(ref _syncronizer, new object(), null);
            // return _syncronizer;

        }*/

        #endregion

        #region public

        public ImardaMSMQueue(string queueName)
        {
            _QueueName = queueName;
            CreateMessageQueue();
        }

        /// <summary>
        /// Creates a PerformanceCounter to read the number of messages in queue
        /// WARNING - Do catch and handle generic exceptions thrown by this method
        /// </summary>
        /// <returns>The number of messages or a negative value based on type of exception raised (see code) - does not trap all exceptions</returns>
        //[Obsolete]
        public int MSMQCount()
        {
            try
            {

                //try MSMQManagement first - does not work, only throws errors
                //MSMQManagement msmq = new MSMQManagement();
                //object machineName = _Queue.MachineName;
                //object path = _Queue.Path;
                //object formatName = _Queue.FormatName;
                //try
                //{
                //	msmq.Init(ref machineName, ref path, ref formatName);
                //	return msmq.MessageCount;
                //	return 0;
                //}
                //catch (Exception ex)
                {
                    try
                    {
                        //try PerformanceCounter
                        string formatNameStr = _Queue.FormatName;
                        var queuePath = formatNameStr.Split(new[] { ':' })[1];
                        var queueCounter = new PerformanceCounter("MSMQ Queue", "Messages in Queue", queuePath);
                        return (int)queueCounter.NextValue();
                    }
                    catch
                    {
                        //fall back to internal counter
                        return Count;
                    }
                }


                /*throw new NotImplementedException();*/

                //PG counting the MSMQ wiht performance counter is unreliable

                //trying to use
                //.\private$\rslstore (_Queue.Path)
                //always throws exceptions whereas
                //inz10pc025\private$\rslstore
                //works ok

                //var formatName = _Queue.FormatName;
                //var queuePath = formatName.Split(new[] { ':' })[1];
                //var queueCounter = new PerformanceCounter("MSMQ Queue", "Messages in Queue", queuePath);
                //return (int)queueCounter.NextValue();

                //note that I had exceptions also when the queue existed
                //which makes the whole process a bit unreliable
                //using the com msmq dll to access the count method that it exposes may be better
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
            catch (UnauthorizedAccessException)
            {
                return -2;
            }
        }

        private int _Count;

        /// <summary>
        /// Thic count is dynamic and will only return the count of messages added by the currently running app, e.g. not including those present in the queue before a restart
        /// </summary>
        public int Count
        {
            get { return _Count; }
        }

        public string SendWithId(object item)
        {
            var msg = new Message(item);
            _Queue.Send(msg);
            Interlocked.Increment(ref _Count);
            return msg.Id;
        }

        public void Send(object item)
        {
            _Queue.Send(item);
            Interlocked.Increment(ref _Count);
        }

        public void Send(object item, MessageQueueTransactionType type)
        {
            _Queue.Send(item, type);
            Interlocked.Increment(ref _Count);
        }

        public object ReceiveById(string msgId)
        {
            object o = _Queue.ReceiveById(msgId);
            Interlocked.Decrement(ref _Count);
            return o;
        }


        public object Receive()
        {
            object o = _Queue.Receive();
            Interlocked.Decrement(ref _Count);
            return o;
        }

        public object Receive(MessageQueueTransactionType type)
        {
            object o = _Queue.Receive(type);
            Interlocked.Decrement(ref _Count);
            return o;
        }

        public object Peek()
        {
            return _Queue.Peek();
        }

        public object Peek(TimeSpan timeout)
        {
            return _Queue.Peek(timeout);
        }

        public bool Transactional
        {
            get { return _Queue.Transactional; }
        }

        #endregion
    }
}