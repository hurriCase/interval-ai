using System;
using VContainer;

namespace Source.Scripts.Core.DI
{
    internal static class StateFactoryExtensions
    {
        internal static void RegisterStateFactory<TKey, TService, TImplementation>(
            this IContainerBuilder builder,
            Func<IObjectResolver, TKey, TImplementation> factory,
            Lifetime lifetime = Lifetime.Scoped)
            where TService : class
            where TImplementation : class, TService
        {
            builder.Register<IStateFactory<TKey, TService>>(
                resolver => new StateFactory<TKey, TService>(resolver, factory),
                lifetime);
        }
    }
}