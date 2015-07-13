using System;
using System.Diagnostics;
using System.Messaging;
using System.Threading;
using Imarda.Lib;

// ReSharper disable once CheckNamespace
namespace Imarda.Logging
{
    public class GenericMsmq<T> : ImardaGenericMsmQueue<T>, IGenericMsmq<T>
        where T : class, new()
    {

        private readonly object _lockSend;
        private readonly ILogger _log;

        public GenericMsmq(string queueName, ILogger log)
            : base(queueName,log)
        {
            _log = log;
            _lockSend = new object();
        }

        protected string Serialize(T t)
        {
            //var serFunc = SerializationFunction ?? StringUtils.ObjectToString;
            //return serFunc(t);
            return XmlUtils.Serialize(t);
        }

        protected T Deserialize(string s)
        {
            //var deserFunc = DeserializationFunction ?? (s1 => StringUtils.StringToObject(s1) as T);
            //return deserFunc(s);
            return XmlUtils.Deserialize<T>(s);
        }


        //public Func<T, string> SerializationFunction { get; set; }
        //public Func<string, T> DeserializationFunction { get; set; }

        public void SendExt(T item)
        {
            try
            {
                lock (_lockSend)
                {
                    var body = Serialize(item);//StringUtils.ObjectToString(item); //to base64 string
                    Send(body);
                }
            }
            catch (Exception x)
            {
                _log.Error(x);
                //Debug.WriteLine(x);
            }
        }

        private T _lastMessage;
        public T PeekMessage()
        {
            try
            {
                var peek = SafePeek(false);

                if (peek != null)
                {
                    peek.Formatter = new XmlMessageFormatter(new[] { typeof(string) });
                    var s = peek.Body.ToString();

                    var message = Deserialize(s);

                    if (message != null)
                    {
                        var checkDuplicate = new Func<T, T, bool>((tCurrent, tLast) => (null == EqualityComparer)
                                                                                           ? 0 == String.CompareOrdinal(tCurrent.ToString(), tLast.ToString())
                                                                                           : EqualityComparer.Equals(tCurrent, tLast));

                        if (null != _lastMessage && checkDuplicate(message, _lastMessage)) //check and remove duplicate
                        {
                            Receive();
                        }
                        else
                        {
                            _lastMessage = message;

                            var idMessage = message as IMsmqIdMessage;
                            if (idMessage != null)
                            {
                                idMessage.MessageId = peek.Id;
                            }

                            return message;
                        }
                    }
                }
                else
                {
                    // Allow for possible queue outages
                    Thread.Sleep(1000);
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

            return null;
        }
    }
}
