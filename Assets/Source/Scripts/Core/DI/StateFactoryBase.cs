using System;
using System.Collections.Generic;
using VContainer;
using VContainer.Unity;

namespace Source.Scripts.Core.DI
{
    [Preserve]
    internal abstract class ResolverStateFactory<TKey, TService, TInterface> : IDisposable
        where TService : class
        where TInterface : class
    {
        private readonly Dictionary<TKey, LifetimeScope> _scopesByKey = new();
        private readonly Dictionary<TKey, TInterface> _cachedServices = new();

        private readonly LifetimeScope _parentScope;

        [Preserve]
        protected ResolverStateFactory(LifetimeScope parentScope)
        {
            _parentScope = parentScope;
        }

        public TInterface GetOrCreate(TKey key)
        {
            if (_cachedServices.TryGetValue(key, out var service))
                return service;

            var scope = CreateScopeForKey(key);
            _scopesByKey[key] = scope;

            service = scope.Container.Resolve<TInterface>();
            _cachedServices[key] = service;

            return service;
        }

        private LifetimeScope CreateScopeForKey(TKey key)
        {
            var scope = _parentScope.CreateChild(builder =>
            {
                builder.RegisterInstance(key);
                builder.Register<TService>(Lifetime.Scoped).AsImplementedInterfaces();
            });

            return scope;
        }

        public void Dispose()
        {
            foreach (var scope in _scopesByKey.Values)
                scope.Dispose();

            _scopesByKey.Clear();
            _cachedServices.Clear();
        }
    }
}