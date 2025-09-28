using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    internal interface ITextToSpeech
    {
        ReadOnlyReactiveProperty<bool> IsAvailable { get; }
        UniTask SpeechTextAsync(string text, CancellationToken token);
    }
}