#region

using System.Collections.Generic;
using FernBusinessBase.ChannelHanding;
using Imarda.Lib.MVVM.Common;

#endregion

namespace FernBusinessBase.Model
{
    public class DuplexProxyStorage<TContract> : Disposable
        where TContract : class, IDuplexServiceFacadeBase
    {
        private readonly Dictionary<string, DuplexProxyStorageItem<TContract, ICallback>> _items;
        private readonly object _lock = new object();

        internal DuplexProxyStorage()
        {
            _items = new Dictionary<string, DuplexProxyStorageItem<TContract, ICallback>>();
        }

        internal TContract GetChannel<TCallback>(object caller, TCallback callback, string endpointConfigurationName)
            where TCallback : ICallback
        {
            DuplexProxyStorageItem<TContract, ICallback> item;
            lock (_lock)
            {
                if (!_items.TryGetValue(endpointConfigurationName, out item))
                {
                    item = new DuplexProxyStorageItem<TContract, ICallback>(endpointConfigurationName);
                    _items.Add(endpointConfigurationName, item);
                }
            }

            var proxy = item.Get(caller, callback);
            return proxy.ChannelFactory.CreateChannel();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var value in _items.Values)
                {
                    value.Dispose();
                }

                _items.Clear();
            }

            base.Dispose(disposing);
        }
    }

    public class DuplexProxyStorageItem<TChannel, TCallback> : Disposable
        where TChannel : class
    {
        private readonly string _endpointConfigurationName;
        private readonly object _lock = new object();
        private Dictionary<object, DuplexProxy<TChannel>> _clients;

        public DuplexProxyStorageItem(string endpointConfigurationName)
        {
            _endpointConfigurationName = endpointConfigurationName;
            _clients = new Dictionary<object, DuplexProxy<TChannel>>();
        }

        internal DuplexProxy<TChannel> Get(object keyReference, TCallback callback)
        {
            DuplexProxy<TChannel> proxy;
            lock (_lock)
            {
                if (_clients.TryGetValue(keyReference, out proxy))
                {
                    return proxy;
                }

                proxy = new DuplexProxy<TChannel>(callback, _endpointConfigurationName);
                _clients.Add(keyReference, proxy);
            }

            return proxy;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _clients.Clear();
                _clients = null;
            }

            base.Dispose(disposing);
        }
    }
}