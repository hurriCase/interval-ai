using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Source.Scripts.Core.GenerativeLanguage.Data
{
    [Serializable]
    internal sealed class ChatRequest
    {
        [JsonProperty("contents")] public List<Content> Contents { get; private set; }

        [JsonConstructor]
        internal ChatRequest(List<Content> contents)
        {
            Contents = contents;
        }
    }
}