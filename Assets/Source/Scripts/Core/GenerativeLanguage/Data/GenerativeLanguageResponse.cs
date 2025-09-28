using System;
using Newtonsoft.Json;
using Source.Scripts.Core.Api.Interfaces;

namespace Source.Scripts.Core.GenerativeLanguage.Data
{
    [Serializable]
    internal sealed class GenerativeLanguageResponse : IValidatable
    {
        [JsonProperty("candidates")] public Candidate[] Candidates { get; private set; }

        public bool IsValid() => Candidates is { Length: > 0 } && Candidates[0].Content.Parts is { Length: > 0 };
        internal string GetText() => Candidates[0].Content.Parts[0].Text;
    }
}