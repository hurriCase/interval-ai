using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace Source.Scripts.Core.Localization.Translator
{
    internal interface ITranslator
    {
        [MustUseReturnValue]
        UniTask<string> TranslateTextAsync(string text, SystemLanguage targetLanguage, CancellationToken token);

        [MustUseReturnValue]
        UniTask<string> TranslateTextAsync(string text, CancellationToken token);
    }
}