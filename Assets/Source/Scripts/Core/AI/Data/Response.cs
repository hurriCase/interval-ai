using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.AI.Data
{
    [Serializable]
    internal sealed class Response
    {
        [JsonProperty("candidates")] public Candidate[] Candidates { get; private set; }
    }
}