using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Patterns.DI.Base
{
    internal static class DIContainer
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();
        private static readonly Dictionary<Type, Func<object>> _serviceFactories = new Dictionary<Type, Func<object>>();

        internal static void RegisterSingleton<T>(T instance)
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
                Debug.LogWarning("[DIContainer::RegisterSingleton] " +
                                 $"Service {type.Name} is already registered. Overwriting.");

            _services[type] = instance;
        }

        internal static void Register<T>(Func<T> factory)
            => _serviceFactories[typeof(T)] = () => factory();

        internal static object Resolve(Type type)
        {
            if (_services.TryGetValue(type, out var instance))
                return instance;

            if (_serviceFactories.TryGetValue(type, out var factory))
            {
                var resolvedInstance = factory();
                _services[type] = resolvedInstance;
                return resolvedInstance;
            }

            throw new InvalidOperationException($"[DIContainer::Resolve] No registration for type {type.Name}");
        }

        internal static T Resolve<T>() => (T)Resolve(typeof(T));

        internal static void Clear()
        {
            _services.Clear();
            _serviceFactories.Clear();
        }
    }
}