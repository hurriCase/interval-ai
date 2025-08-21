using System;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Source.Scripts.Core.Others
{
    internal static class ObservableExtensions
    {
        internal static void SubscribeAndRegister<TSelf, T>(
            this Observable<T> observable,
            TSelf self,
            Action<TSelf> onNext)
            where TSelf : MonoBehaviour
        {
            observable.Subscribe((self, onNext), static (_, tuple) => tuple.onNext(tuple.self))
                .RegisterTo(self.GetCancellationTokenOnDestroy());
        }

        internal static void SubscribeAndRegister<TSelf, T>(
            this Observable<T> observable,
            TSelf self,
            Action<T, TSelf> onNext)
            where TSelf : MonoBehaviour
        {
            observable.Subscribe((self, onNext), static (value, tuple) => tuple.onNext(value, tuple.self))
                .RegisterTo(self.GetCancellationTokenOnDestroy());
        }

        internal static void SubscribeAndRegister<TSelf, T, TTuple>(
            this Observable<T> observable,
            TSelf self,
            TTuple tuple,
            Action<TTuple, TSelf> onNext)
            where TSelf : MonoBehaviour
        {
            observable.Subscribe((self, tuple, onNext),
                    static (_, state) => state.onNext(state.tuple, state.self))
                .RegisterTo(self.GetCancellationTokenOnDestroy());
        }

        internal static void SubscribeAndRegister<TSelf, T, TTuple>(
            this Observable<T> observable,
            TSelf self,
            TTuple tuple,
            Action<T, TSelf, TTuple> onNext)
            where TSelf : MonoBehaviour
        {
            observable.Subscribe((self, tuple, onNext),
                    static (value, state) => state.onNext(value, state.self, state.tuple))
                .RegisterTo(self.GetCancellationTokenOnDestroy());
        }
    }
}