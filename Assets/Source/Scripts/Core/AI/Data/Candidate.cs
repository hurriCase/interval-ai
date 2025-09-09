using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.AI.Data
{
    [Serializable]
    internal sealed class Candidate
    {
        [JsonProperty("content")] public Content Content { get; private set; }
    }
}