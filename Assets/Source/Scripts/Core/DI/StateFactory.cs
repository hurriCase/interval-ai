using System;
using System.Collections.Generic;
using VContainer;

namespace Source.Scripts.Core.DI
{
    internal sealed class StateFactory<TKey, TService> : IStateFactory<TKey, TService>
        where TService : class
    {
        private readonly Dictionary<TKey, TService> _cachedServices = new();
        private readonly IObjectResolver _resolver;
        private readonly Func<IObjectResolver, TKey, TService> _factory;

        internal StateFactory(
            IObjectResolver resolver,
            Func<IObjectResolver, TKey, TService> factory)
        {
            _resolver = resolver;
            _factory = factory;
        }

        public TService GetOrCreate(TKey key)
        {
            if (_cachedServices.TryGetValue(key, out var service))
                return service;

            service = _factory(_resolver, key);
            _cachedServices[key] = service;
            return service;
        }

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