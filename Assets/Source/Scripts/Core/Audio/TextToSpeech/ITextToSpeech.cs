using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Source.Scripts.Core.Audio.TextToSpeech
{
    internal interface ITextToSpeech
    {
        UniTask<AudioClip> SpeechTextAsync(string text, CancellationToken token);
    }
}