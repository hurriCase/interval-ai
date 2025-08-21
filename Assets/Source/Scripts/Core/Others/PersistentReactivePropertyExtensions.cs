using System;
using CustomUtils.Runtime.Storage;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Source.Scripts.Core.Others
{
    internal static class PersistentReactivePropertyExtensions
    {
        internal static void SubscribeAndRegister<TSelf, T>(
            this PersistentReactiveProperty<T> observable,
            TSelf self,
            Action<TSelf> onNext)
            where TSelf : MonoBehaviour
        {
            observable.Subscribe((self, onNext), static (_, tuple) => tuple.onNext(tuple.self))
                .RegisterTo(self.GetCancellationTokenOnDestroy());
        }

        internal static void SubscribeAndRegister<TSelf, T>(
            this PersistentReactiveProperty<T> observable,
            TSelf self,
            Action<T, TSelf> onNext)
            where TSelf : MonoBehaviour
        {
            observable.Subscribe((self, onNext), static (value, tuple) => tuple.onNext(value, tuple.self))
                .RegisterTo(self.GetCancellationTokenOnDestroy());
        }
    }
}