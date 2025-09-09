using System;
using Newtonsoft.Json;

namespace Source.Scripts.Core.AI.Data
{
    [Serializable]
    internal sealed class Part
    {
        [JsonProperty("text")] public string Text { get; private set; }

        [JsonConstructor]
        internal Part(string text)
        {
            Text = text;
        }
    }
}