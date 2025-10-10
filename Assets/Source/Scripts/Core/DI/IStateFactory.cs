using System;

namespace Source.Scripts.Core.DI
{
    internal interface IStateFactory<in TKey, out TService> : IDisposable
        where TService : class
    {
        TService GetOrCreate(TKey key);
    }
}