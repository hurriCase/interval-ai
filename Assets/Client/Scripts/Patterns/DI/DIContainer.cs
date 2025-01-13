using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Scripts.Patterns.DI
{
    /// <summary>
    ///     Provides a dependency injection container for managing service registrations and resolutions.
    /// </summary>
    internal static class DIContainer
    {
        private static readonly Dictionary<Type, object> _services = new();
        private static readonly Dictionary<Type, Func<object>> _serviceFactories = new();

        /// <summary>
        ///     Registers a singleton instance of a service.
        /// </summary>
        /// <typeparam name="T">The type of service to register.</typeparam>
        /// <param name="instance">The singleton instance to register.</param>
        internal static void RegisterSingleton<T>(T instance)
        {
            var type = typeof(T);
            if (_services.ContainsKey(type))
                Debug.LogWarning("[DIContainer::RegisterSingleton] " +
                                 $"Service {type.Name} is already registered. Overwriting.");

            _services[type] = instance;
        }

        /// <summary>
        ///     Registers a factory method for creating service instances.
        /// </summary>
        /// <typeparam name="T">The type of service to register.</typeparam>
        /// <param name="factory">The factory method for creating service instances.</param>
        internal static void Register<T>(Func<T> factory)
            => _serviceFactories[typeof(T)] = () => factory();

        /// <summary>
        ///     Resolves a service instance by type.
        /// </summary>
        /// <param name="type">The type of service to resolve.</param>
        /// <returns>The resolved service instance.</returns>
        /// <exception cref="InvalidOperationException">Thrown when the requested service type is not registered.</exception>
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

        /// <summary>
        ///     Resolves a service instance by type.
        /// </summary>
        /// <typeparam name="T">The type of service to resolve.</typeparam>
        /// <returns>The resolved service instance.</returns>
        internal static T Resolve<T>() => (T)Resolve(typeof(T));

        /// <summary>
        ///     Clears a singleton dependency from the container.
        /// </summary>
        /// <typeparam name="T">The type of singleton service to clear.</typeparam>
        internal static void ClearSingletonDependency<T>()
        {
            _services.Remove(typeof(T));
        }

        /// <summary>
        ///     Clears all registered services and factories from the container.
        /// </summary>
        internal static void Clear()
        {
            _services.Clear();
            _serviceFactories.Clear();
        }
    }
}