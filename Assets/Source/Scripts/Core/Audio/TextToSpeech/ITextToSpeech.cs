using System.Threading;
using Cysharp.Threading.Tasks;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    internal interface ITextToSpeech
    {
        UniTask SpeechTextAsync(string text, CancellationToken token);
    }
}