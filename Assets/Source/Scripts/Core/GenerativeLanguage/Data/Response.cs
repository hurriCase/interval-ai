using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.GenerativeLanguage.Data
{
    [Serializable]
    internal sealed class Response
    {
        [JsonProperty("candidates")] public Candidate[] Candidates { get; private set; }
    }
}