using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.ApiHelper;

namespace Source.Scripts.Core.Localization.Translator
{
    internal interface ITranslator : IApiService
    {
        UniTask<string> TranslateTextAsync(string text, CancellationToken token);
    }
}