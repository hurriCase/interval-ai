using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using R3;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Translator
{
    internal interface ITranslator
    {
        ReadOnlyReactiveProperty<bool> IsAvailable { get; }
        UniTask UpdateAvailable(CancellationToken token);
        UniTask<string> TranslateTextAsync(string text, CancellationToken token);
    }
}