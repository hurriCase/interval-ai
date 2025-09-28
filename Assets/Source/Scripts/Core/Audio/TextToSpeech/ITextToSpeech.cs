using System.Threading;
using Cysharp.Threading.Tasks;
using Source.Scripts.Core.Api.Interfaces;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    internal interface ITextToSpeech : IApiService
    {
        UniTask SpeechTextAsync(string text, CancellationToken token);
    }
}