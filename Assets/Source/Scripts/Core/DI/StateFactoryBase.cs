using System;
using System.Collections.Generic;

namespace Source.Scripts.Core.DI
{
    internal abstract class StateFactoryBase<TKey, TService> : IDisposable
        where TService : class
    {
        private readonly Dictionary<TKey, TService> _cachedServices = new();

        public TService GetOrCreate(TKey key)
        {
            if (_cachedServices.TryGetValue(key, out var service))
                return service;

            service = CreateService(key);
            _cachedServices[key] = service;
            return service;
        }

        protected abstract TService CreateService(TKey key);

        public void Dispose()
        {
            foreach (var service in _cachedServices.Values)
            {
                if (service is IDisposable disposable)
                    disposable.Dispose();
            }

            _cachedServices.Clear();
        }
    }
}